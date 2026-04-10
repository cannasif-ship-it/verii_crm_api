using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<ApiResponse<PdfTemplateAssetDto>> UploadAsync(IFormFile file, long userId, long? templateId = null, string? assetScope = null)
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
    }
}
