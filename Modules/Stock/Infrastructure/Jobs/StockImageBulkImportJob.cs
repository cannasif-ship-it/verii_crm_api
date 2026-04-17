using System.IO.Compression;
using crm_api.Data;
using crm_api.Helpers;
using crm_api.Modules.Stock.Domain.Entities;
using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using StockEntity = crm_api.Modules.Stock.Domain.Entities.Stock;

namespace crm_api.Modules.Stock.Infrastructure.Jobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 3600)]
    [AutomaticRetry(Attempts = 1)]
    public class StockImageBulkImportJob : IStockImageBulkImportJob
    {
        private static readonly HashSet<string> WrapperFolderNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "resimler",
            "gorseller",
            "görseller",
            "images",
            "image",
            "photos",
            "pictures"
        };

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp", ".heic", ".heif"
        };

        private const int SaveBatchSize = 200;

        private readonly CmsDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<StockImageBulkImportJob> _logger;

        public StockImageBulkImportJob(
            CmsDbContext db,
            IWebHostEnvironment environment,
            ILogger<StockImageBulkImportJob> logger)
        {
            _db = db;
            _environment = environment;
            _logger = logger;
        }

        public async Task ExecuteAsync(string archivePath, string originalFileName)
        {
            if (string.IsNullOrWhiteSpace(archivePath) || !File.Exists(archivePath))
            {
                _logger.LogWarning("Stock bulk image import skipped because archive file was not found: {ArchivePath}", archivePath);
                return;
            }

            var startedAt = DateTime.UtcNow;
            var importedCount = 0;
            var skippedCount = 0;
            var missingStockCount = 0;

            try
            {
                using var archiveStream = new FileStream(archivePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);

                var imageEntries = archive.Entries
                    .Where(IsSupportedImageEntry)
                    .ToList();

                var stockCodes = imageEntries
                    .Select(GetStockCodeFromEntry)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Cast<string>()
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var stockMap = await LoadStockMapAsync(stockCodes).ConfigureAwait(false);
                var sortOrders = await LoadSortOrdersAsync(stockMap.Values.Select(x => x.Id).Distinct().ToList()).ConfigureAwait(false);
                var stockHasPrimary = await LoadPrimaryFlagsAsync(stockMap.Values.Select(x => x.Id).Distinct().ToList()).ConfigureAwait(false);

                var stockImagesFolder = Path.Combine(_environment.ContentRootPath, "uploads", "stock-images");
                Directory.CreateDirectory(stockImagesFolder);

                foreach (var entry in imageEntries)
                {
                    var stockCode = GetStockCodeFromEntry(entry);
                    if (string.IsNullOrWhiteSpace(stockCode))
                    {
                        skippedCount++;
                        continue;
                    }

                    if (!stockMap.TryGetValue(stockCode, out var stock))
                    {
                        missingStockCount++;
                        continue;
                    }

                    var extension = Path.GetExtension(entry.Name);
                    var storedFileName = $"{Guid.NewGuid():N}{extension}";
                    var stockFolder = Path.Combine(stockImagesFolder, stock.Id.ToString());
                    Directory.CreateDirectory(stockFolder);

                    var filePath = Path.Combine(stockFolder, storedFileName);
                    await using (var entryStream = entry.Open())
                    await using (var outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true))
                    {
                        await entryStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    }

                    var nextSortOrder = sortOrders.TryGetValue(stock.Id, out var currentSortOrder)
                        ? currentSortOrder + 1
                        : 1;
                    sortOrders[stock.Id] = nextSortOrder;

                    var isPrimary = !stockHasPrimary.GetValueOrDefault(stock.Id);
                    if (isPrimary)
                    {
                        stockHasPrimary[stock.Id] = true;
                    }

                    _db.StockImages.Add(new StockImage
                    {
                        StockId = stock.Id,
                        FilePath = $"/uploads/stock-images/{stock.Id}/{storedFileName}",
                        AltText = Path.GetFileNameWithoutExtension(entry.Name),
                        SortOrder = nextSortOrder,
                        IsPrimary = isPrimary,
                        CreatedDate = DateTimeProvider.Now,
                        IsDeleted = false
                    });

                    importedCount++;

                    if (importedCount % SaveBatchSize == 0)
                    {
                        await _db.SaveChangesAsync().ConfigureAwait(false);
                    }
                }

                await _db.SaveChangesAsync().ConfigureAwait(false);

                _logger.LogInformation(
                    "Stock bulk image import completed. File={OriginalFileName}, Imported={ImportedCount}, MissingStock={MissingStockCount}, Skipped={SkippedCount}, DurationMs={DurationMs}",
                    originalFileName,
                    importedCount,
                    missingStockCount,
                    skippedCount,
                    (DateTime.UtcNow - startedAt).TotalMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock bulk image import failed for archive {OriginalFileName}", originalFileName);
                throw;
            }
            finally
            {
                try
                {
                    if (File.Exists(archivePath))
                    {
                        File.Delete(archivePath);
                    }
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogWarning(cleanupEx, "Stock bulk image import archive cleanup failed for {ArchivePath}", archivePath);
                }
            }
        }

        private static bool IsSupportedImageEntry(ZipArchiveEntry entry)
        {
            if (entry.Length <= 0 || string.IsNullOrWhiteSpace(entry.Name))
            {
                return false;
            }

            var extension = Path.GetExtension(entry.Name);
            return AllowedExtensions.Contains(extension);
        }

        private static string? GetStockCodeFromEntry(ZipArchiveEntry entry)
        {
            var normalized = entry.FullName.Replace('\\', '/').Trim('/');
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return null;
            }

            var segments = normalized
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (segments.Length < 2)
            {
                return null;
            }

            if (segments.Length >= 3 && WrapperFolderNames.Contains(segments[0]))
            {
                return segments[1];
            }

            return segments[0];
        }

        private async Task<Dictionary<string, StockEntity>> LoadStockMapAsync(List<string> stockCodes)
        {
            var result = new Dictionary<string, StockEntity>(StringComparer.OrdinalIgnoreCase);
            const int chunkSize = 1000;

            for (var i = 0; i < stockCodes.Count; i += chunkSize)
            {
                var chunk = stockCodes.Skip(i).Take(chunkSize).ToList();
                var stocks = await _db.Stocks
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted && chunk.Contains(x.ErpStockCode))
                    .Select(x => new StockEntity
                    {
                        Id = x.Id,
                        ErpStockCode = x.ErpStockCode,
                        StockName = x.StockName,
                        BranchCode = x.BranchCode
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

                foreach (var stock in stocks)
                {
                    result[stock.ErpStockCode] = stock;
                }
            }

            return result;
        }

        private async Task<Dictionary<long, int>> LoadSortOrdersAsync(List<long> stockIds)
        {
            if (stockIds.Count == 0)
            {
                return new Dictionary<long, int>();
            }

            return await _db.StockImages
                .AsNoTracking()
                .Where(x => !x.IsDeleted && stockIds.Contains(x.StockId))
                .GroupBy(x => x.StockId)
                .Select(x => new { StockId = x.Key, MaxSortOrder = x.Max(y => y.SortOrder) })
                .ToDictionaryAsync(x => x.StockId, x => x.MaxSortOrder)
                .ConfigureAwait(false);
        }

        private async Task<Dictionary<long, bool>> LoadPrimaryFlagsAsync(List<long> stockIds)
        {
            if (stockIds.Count == 0)
            {
                return new Dictionary<long, bool>();
            }

            return await _db.StockImages
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsPrimary && stockIds.Contains(x.StockId))
                .Select(x => x.StockId)
                .Distinct()
                .ToDictionaryAsync(x => x, _ => true)
                .ConfigureAwait(false);
        }
    }
}
