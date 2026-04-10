using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using crm_api.Modules.PdfBuilder.Domain.Enums;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    public class PdfTemplateAssetService : IPdfTemplateAssetService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".heic", ".heif" };
        private const string PdfDesignerFolder = "pdf-designer";
        private const string QuickQuotationFolder = "quick-quotation";
        private const string ReportBuilderFolder = "report-builder";
        private const string LegacyAssetBaseFolder = "pdf-template-assets";
        private const string DraftUsersFolder = "users";
        private const long MaxFileSizeBytes = 10 * 1024 * 1024;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<PdfTemplateAssetService> _logger;

        public PdfTemplateAssetService(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment environment,
            ILocalizationService localizationService,
            ILogger<PdfTemplateAssetService> logger)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
            _localizationService = localizationService;
            _logger = logger;
        }

        public async Task<ApiResponse<PdfTemplateAssetDto>> UploadAsync(
            IFormFile file,
            long userId,
            long? templateId = null,
            string? assetScope = null,
            string? elementId = null,
            int? pageNumber = null,
            long? tempQuotattionId = null,
            long? tempQuotattionLineId = null,
            string? productCode = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return ApiResponse<PdfTemplateAssetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("FileUploadService.FileRequired"),
                        null,
                        400);
                }

                if (file.Length > MaxFileSizeBytes)
                {
                    return ApiResponse<PdfTemplateAssetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("FileUploadService.FileSizeExceeded", MaxFileSizeBytes / (1024 * 1024)),
                        null,
                        400);
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return ApiResponse<PdfTemplateAssetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("FileUploadService.InvalidFileFormat", string.Join(", ", AllowedExtensions)),
                        null,
                        400);
                }

                var uploadsBasePath = Path.Combine(_environment.ContentRootPath, "uploads");
                var assetsPath = uploadsBasePath;
                var ownerFolder = await BuildOwnerFolderAsync(templateId, userId, assetScope).ConfigureAwait(false);
                var shouldStoreAsQuickQuotationImage = await ShouldStoreAsQuickQuotationImageAsync(templateId, assetScope).ConfigureAwait(false);
                var assetPath = Path.Combine(assetsPath, ownerFolder);

                Directory.CreateDirectory(uploadsBasePath);
                Directory.CreateDirectory(assetsPath);
                Directory.CreateDirectory(assetPath);

                var storedFileName = $"{Guid.NewGuid():N}{extension}";
                var fullPath = Path.Combine(assetPath, storedFileName);

                await using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    await file.CopyToAsync(fileStream).ConfigureAwait(false);
                    await fileStream.FlushAsync().ConfigureAwait(false);
                }

                var relativeUrl = $"/uploads/{ownerFolder.Replace('\\', '/')}/{storedFileName}";
                var dto = new PdfTemplateAssetDto
                {
                    OriginalFileName = Path.GetFileName(file.FileName),
                    StoredFileName = storedFileName,
                    RelativeUrl = relativeUrl,
                    ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                    SizeBytes = file.Length,
                    ReportTemplateId = templateId,
                    ElementId = elementId,
                    PageNumber = pageNumber,
                    TempQuotattionId = tempQuotattionId,
                    TempQuotattionLineId = tempQuotattionLineId,
                    ProductCode = productCode,
                    CreatedDate = DateTimeProvider.Now,
                    IsDeleted = false,
                };

                if (shouldStoreAsQuickQuotationImage)
                {
                    var entity = new QuickQuotationImage
                    {
                        OriginalFileName = dto.OriginalFileName,
                        StoredFileName = dto.StoredFileName,
                        RelativeUrl = dto.RelativeUrl,
                        ContentType = dto.ContentType,
                        SizeBytes = dto.SizeBytes,
                        CreatedBy = userId,
                        CreatedDate = dto.CreatedDate,
                    };

                    await _unitOfWork.Repository<QuickQuotationImage>().AddAsync(entity).ConfigureAwait(false);
                    await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                    if (tempQuotattionId.HasValue && tempQuotattionLineId.HasValue)
                    {
                        await UpsertQuickQuotationImageUsageAsync(
                            entity.Id,
                            tempQuotattionId.Value,
                            tempQuotattionLineId.Value,
                            productCode).ConfigureAwait(false);
                        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                    }

                    dto.Id = entity.Id;
                    dto.UpdatedDate = entity.UpdatedDate;
                    dto.DeletedDate = entity.DeletedDate;
                }
                else
                {
                    var entity = new PdfTemplateAsset
                    {
                        OriginalFileName = dto.OriginalFileName,
                        StoredFileName = dto.StoredFileName,
                        RelativeUrl = dto.RelativeUrl,
                        ContentType = dto.ContentType,
                        SizeBytes = dto.SizeBytes,
                        CreatedBy = userId,
                        CreatedDate = dto.CreatedDate,
                    };

                    await _unitOfWork.Repository<PdfTemplateAsset>().AddAsync(entity).ConfigureAwait(false);
                    await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                    if (templateId.HasValue && templateId.Value > 0 && !string.IsNullOrWhiteSpace(elementId))
                    {
                        await UpsertPdfImageUsageAsync(
                            entity.Id,
                            templateId.Value,
                            elementId,
                            pageNumber ?? 1,
                            await ResolveRuleTypeAsync(templateId.Value, assetScope).ConfigureAwait(false)).ConfigureAwait(false);
                        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                    }

                    dto.Id = entity.Id;
                    dto.UpdatedDate = entity.UpdatedDate;
                    dto.DeletedDate = entity.DeletedDate;
                }

                return ApiResponse<PdfTemplateAssetDto>.SuccessResult(
                    dto,
                    _localizationService.GetLocalizedString("FileUploadService.FileUploadedSuccessfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading PDF template asset");
                return ApiResponse<PdfTemplateAssetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("FileUploadService.FileUploadError"),
                    _localizationService.GetLocalizedString("FileUploadService.FileUploadExceptionMessage", ex.Message),
                    500);
            }
        }

        public async Task NormalizeTemplateAssetPathsAsync(ReportTemplateData templateData, long userId, long templateId, DocumentRuleType ruleType)
        {
            if (templateData?.Elements == null || templateData.Elements.Count == 0 || templateId <= 0)
                return;

            var uploadsBasePath = Path.Combine(_environment.ContentRootPath, "uploads");
            var assetsBasePath = uploadsBasePath;
            var targetFolder = BuildTemplateFolder(ruleType, templateId);
            var targetDirectory = Path.Combine(assetsBasePath, targetFolder);

            Directory.CreateDirectory(uploadsBasePath);
            Directory.CreateDirectory(assetsBasePath);
            Directory.CreateDirectory(targetDirectory);

            foreach (var imageElement in templateData.Elements.Where(x =>
                         string.Equals(x.Type, "image", StringComparison.OrdinalIgnoreCase) &&
                         !string.IsNullOrWhiteSpace(x.Value)))
            {
                var currentValue = imageElement.Value!.Trim();
                var draftRelativePath = TryGetDraftRelativePath(currentValue, userId);
                if (draftRelativePath == null)
                    continue;

                var sourceFile = Path.Combine(uploadsBasePath, draftRelativePath);
                if (!File.Exists(sourceFile))
                    continue;

                var fileName = Path.GetFileName(sourceFile);
                if (string.IsNullOrWhiteSpace(fileName))
                    continue;

                var destinationFile = Path.Combine(targetDirectory, fileName);
                if (!File.Exists(destinationFile))
                {
                    await using var input = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                    await using var output = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
                    await input.CopyToAsync(output).ConfigureAwait(false);
                    await output.FlushAsync().ConfigureAwait(false);
                }

                imageElement.Value = $"/uploads/{targetFolder.Replace('\\', '/')}/{fileName}";
            }

            await SyncTemplateImageUsagesAsync(templateData, templateId, ruleType).ConfigureAwait(false);
        }

        public async Task SyncTemplateImageUsagesAsync(ReportTemplateData templateData, long templateId, DocumentRuleType ruleType)
        {
            if (templateData?.Elements == null || templateId <= 0)
                return;

            var desiredUsages = templateData.Elements
                .Where(x => string.Equals(x.Type, "image", StringComparison.OrdinalIgnoreCase))
                .Where(x => !string.IsNullOrWhiteSpace(x.Value) && x.Value!.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
                .SelectMany(x =>
                {
                    var pageNumbers = (x.PageNumbers != null && x.PageNumbers.Count > 0 ? x.PageNumbers : new List<int> { 1 })
                        .Where(page => page > 0)
                        .Distinct()
                        .ToList();

                    return pageNumbers.Select(page => new DesiredPdfImageUsage(
                        RelativeUrl: x.Value!.Trim(),
                        ElementId: x.Id?.Trim() ?? string.Empty,
                        PageNumber: page));
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.ElementId))
                .Distinct()
                .ToList();

            var existingUsages = await _unitOfWork.Repository<PdfImageUsage>()
                .Query(tracking: true, ignoreQueryFilters: true)
                .Where(x => x.ReportTemplateId == templateId)
                .ToListAsync()
                .ConfigureAwait(false);

            if (desiredUsages.Count == 0)
            {
                foreach (var stale in existingUsages.Where(x => !x.IsDeleted))
                {
                    stale.IsDeleted = true;
                    stale.DeletedDate = DateTimeProvider.Now;
                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                return;
            }

            var relativeUrls = desiredUsages.Select(x => x.RelativeUrl).Distinct().ToList();
            var assets = await _unitOfWork.Repository<PdfTemplateAsset>()
                .Query()
                .Where(x => relativeUrls.Contains(x.RelativeUrl))
                .Select(x => new { x.Id, x.RelativeUrl })
                .ToListAsync()
                .ConfigureAwait(false);

            var assetByUrl = assets.ToDictionary(x => x.RelativeUrl, x => x.Id, StringComparer.OrdinalIgnoreCase);
            var desiredKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var desired in desiredUsages)
            {
                if (!assetByUrl.TryGetValue(desired.RelativeUrl, out var assetId))
                    continue;

                var key = BuildPdfUsageKey(templateId, desired.ElementId, desired.PageNumber);
                desiredKeys.Add(key);

                var existing = existingUsages.FirstOrDefault(x =>
                    x.ReportTemplateId == templateId &&
                    string.Equals(x.ElementId, desired.ElementId, StringComparison.OrdinalIgnoreCase) &&
                    x.PageNumber == desired.PageNumber);

                if (existing == null)
                {
                    var usage = new PdfImageUsage
                    {
                        PdfTemplateAssetId = assetId,
                        ReportTemplateId = templateId,
                        UsageType = PdfImageUsageType.TemplateElement,
                        ElementId = desired.ElementId,
                        PageNumber = desired.PageNumber,
                        RuleType = ruleType,
                    };

                    await _unitOfWork.Repository<PdfImageUsage>().AddAsync(usage).ConfigureAwait(false);
                    existingUsages.Add(usage);
                    continue;
                }

                existing.PdfTemplateAssetId = assetId;
                existing.RuleType = ruleType;
                existing.UsageType = PdfImageUsageType.TemplateElement;
                existing.IsDeleted = false;
                existing.DeletedDate = null;
                await _unitOfWork.Repository<PdfImageUsage>().UpdateAsync(existing).ConfigureAwait(false);
            }

            foreach (var stale in existingUsages.Where(x => !x.IsDeleted))
            {
                var staleKey = BuildPdfUsageKey(stale.ReportTemplateId, stale.ElementId, stale.PageNumber);
                if (desiredKeys.Contains(staleKey))
                    continue;

                stale.IsDeleted = true;
                stale.DeletedDate = DateTimeProvider.Now;
                await _unitOfWork.Repository<PdfImageUsage>().UpdateAsync(stale).ConfigureAwait(false);
            }

            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task BindQuickQuotationImageAsync(string? relativeUrl, long tempQuotattionId, long tempQuotattionLineId, string? productCode = null)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl) || tempQuotattionId <= 0 || tempQuotattionLineId <= 0)
                return;

            var image = await _unitOfWork.Repository<QuickQuotationImage>()
                .Query()
                .Where(x => x.RelativeUrl == relativeUrl)
                .Select(x => new { x.Id })
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (image == null)
                return;

            await UpsertQuickQuotationImageUsageAsync(image.Id, tempQuotattionId, tempQuotattionLineId, productCode).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ClearQuickQuotationImageBindingsForLineAsync(long tempQuotattionLineId)
        {
            if (tempQuotattionLineId <= 0)
                return;

            var usages = await _unitOfWork.Repository<QuickQuotationImageUsage>()
                .Query(tracking: true, ignoreQueryFilters: true)
                .Where(x => x.TempQuotattionLineId == tempQuotattionLineId)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var usage in usages.Where(x => !x.IsDeleted))
            {
                usage.IsDeleted = true;
                usage.DeletedDate = DateTimeProvider.Now;
                await _unitOfWork.Repository<QuickQuotationImageUsage>().UpdateAsync(usage).ConfigureAwait(false);
            }

            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task<string> BuildOwnerFolderAsync(long? templateId, long userId, string? assetScope)
        {
            if (templateId.HasValue && templateId.Value > 0)
            {
                var ruleType = await ResolveRuleTypeAsync(templateId.Value, assetScope).ConfigureAwait(false);
                return BuildTemplateFolder(ruleType, templateId.Value, assetScope);
            }

            return Path.Combine(ResolveBaseFolder(assetScope), DraftUsersFolder, userId.ToString());
        }

        private async Task<bool> ShouldStoreAsQuickQuotationImageAsync(long? templateId, string? assetScope)
        {
            if (string.Equals(assetScope, QuickQuotationFolder, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!templateId.HasValue || templateId.Value <= 0)
                return false;

            var ruleType = await ResolveRuleTypeAsync(templateId.Value, assetScope).ConfigureAwait(false);
            return ruleType == DocumentRuleType.FastQuotation;
        }

        private async Task<DocumentRuleType> ResolveRuleTypeAsync(long templateId, string? assetScope)
        {
            if (string.Equals(assetScope, QuickQuotationFolder, StringComparison.OrdinalIgnoreCase))
                return DocumentRuleType.FastQuotation;

            var templateRuleType = await _unitOfWork.Repository<ReportTemplate>().Query()
                .Where(x => x.Id == templateId && !x.IsDeleted)
                .Select(x => x.RuleType)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return templateRuleType;
        }

        private static string BuildTemplateFolder(DocumentRuleType ruleType, long templateId, string? assetScope = null)
        {
            if (string.Equals(assetScope, ReportBuilderFolder, StringComparison.OrdinalIgnoreCase))
                return Path.Combine(ReportBuilderFolder, templateId.ToString());

            if (string.Equals(assetScope, QuickQuotationFolder, StringComparison.OrdinalIgnoreCase) ||
                ruleType == DocumentRuleType.FastQuotation)
            {
                return Path.Combine(QuickQuotationFolder, templateId.ToString());
            }

            return Path.Combine(PdfDesignerFolder, templateId.ToString());
        }

        private static string ResolveBaseFolder(string? assetScope)
        {
            if (string.Equals(assetScope, QuickQuotationFolder, StringComparison.OrdinalIgnoreCase))
                return QuickQuotationFolder;

            if (string.Equals(assetScope, ReportBuilderFolder, StringComparison.OrdinalIgnoreCase))
                return ReportBuilderFolder;

            if (string.Equals(assetScope, "template", StringComparison.OrdinalIgnoreCase))
                return PdfDesignerFolder;

            return PdfDesignerFolder;
        }

        private async Task UpsertPdfImageUsageAsync(long assetId, long templateId, string elementId, int pageNumber, DocumentRuleType ruleType)
        {
            var existing = await _unitOfWork.Repository<PdfImageUsage>()
                .Query(tracking: true, ignoreQueryFilters: true)
                .FirstOrDefaultAsync(x =>
                    x.ReportTemplateId == templateId &&
                    x.ElementId == elementId &&
                    x.PageNumber == pageNumber)
                .ConfigureAwait(false);

            if (existing == null)
            {
                var usage = new PdfImageUsage
                {
                    PdfTemplateAssetId = assetId,
                    ReportTemplateId = templateId,
                    UsageType = PdfImageUsageType.TemplateElement,
                    ElementId = elementId,
                    PageNumber = pageNumber,
                    RuleType = ruleType,
                };

                await _unitOfWork.Repository<PdfImageUsage>().AddAsync(usage).ConfigureAwait(false);
                return;
            }

            existing.PdfTemplateAssetId = assetId;
            existing.RuleType = ruleType;
            existing.IsDeleted = false;
            existing.DeletedDate = null;
            await _unitOfWork.Repository<PdfImageUsage>().UpdateAsync(existing).ConfigureAwait(false);
        }

        private async Task UpsertQuickQuotationImageUsageAsync(long imageId, long tempQuotattionId, long tempQuotattionLineId, string? productCode)
        {
            var existing = await _unitOfWork.Repository<QuickQuotationImageUsage>()
                .Query(tracking: true, ignoreQueryFilters: true)
                .FirstOrDefaultAsync(x => x.TempQuotattionLineId == tempQuotattionLineId)
                .ConfigureAwait(false);

            if (existing == null)
            {
                var usage = new QuickQuotationImageUsage
                {
                    QuickQuotationImageId = imageId,
                    TempQuotattionId = tempQuotattionId,
                    TempQuotattionLineId = tempQuotattionLineId,
                    ProductCode = string.IsNullOrWhiteSpace(productCode) ? string.Empty : productCode.Trim(),
                };

                await _unitOfWork.Repository<QuickQuotationImageUsage>().AddAsync(usage).ConfigureAwait(false);
                return;
            }

            existing.QuickQuotationImageId = imageId;
            existing.TempQuotattionId = tempQuotattionId;
            existing.ProductCode = string.IsNullOrWhiteSpace(productCode) ? string.Empty : productCode.Trim();
            existing.IsDeleted = false;
            existing.DeletedDate = null;
            await _unitOfWork.Repository<QuickQuotationImageUsage>().UpdateAsync(existing).ConfigureAwait(false);
        }

        private static string BuildPdfUsageKey(long templateId, string elementId, int pageNumber)
            => $"{templateId}:{elementId}:{pageNumber}";

        private static string? TryGetDraftRelativePath(string currentValue, long userId)
        {
            if (!currentValue.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
                return null;

            var normalized = currentValue.Replace('\\', '/');
            var draftPrefixes = new[]
            {
                $"/uploads/{PdfDesignerFolder}/{DraftUsersFolder}/{userId}/",
                $"/uploads/{QuickQuotationFolder}/{DraftUsersFolder}/{userId}/",
                $"/uploads/{ReportBuilderFolder}/{DraftUsersFolder}/{userId}/",
                $"/uploads/{LegacyAssetBaseFolder}/{DraftUsersFolder}/{userId}/",
                $"/uploads/{LegacyAssetBaseFolder}/{userId}/",
            };

            if (draftPrefixes.Any(prefix => normalized.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                return normalized["/uploads/".Length..];

            return null;
        }

        private sealed record DesiredPdfImageUsage(string RelativeUrl, string ElementId, int PageNumber);
    }
}
