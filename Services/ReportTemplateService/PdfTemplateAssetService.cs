using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace crm_api.Services
{
    public class PdfTemplateAssetService : IPdfTemplateAssetService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".heic", ".heif" };
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

        public async Task<ApiResponse<PdfTemplateAssetDto>> UploadAsync(IFormFile file, long userId, long? templateId = null)
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
                var assetsPath = Path.Combine(uploadsBasePath, "pdf-template-assets");
                var ownerFolder = templateId.HasValue && templateId.Value > 0
                    ? Path.Combine("templates", templateId.Value.ToString())
                    : userId.ToString();
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

                var relativeUrl = templateId.HasValue && templateId.Value > 0
                    ? $"/uploads/pdf-template-assets/templates/{templateId.Value}/{storedFileName}"
                    : $"/uploads/pdf-template-assets/{userId}/{storedFileName}";
                var entity = new PdfTemplateAsset
                {
                    OriginalFileName = Path.GetFileName(file.FileName),
                    StoredFileName = storedFileName,
                    RelativeUrl = relativeUrl,
                    ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                    SizeBytes = file.Length,
                    CreatedBy = userId,
                    CreatedDate = DateTimeProvider.Now,
                };

                await _unitOfWork.Repository<PdfTemplateAsset>().AddAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<PdfTemplateAssetDto>.SuccessResult(
                    new PdfTemplateAssetDto
                    {
                        Id = entity.Id,
                        OriginalFileName = entity.OriginalFileName,
                        StoredFileName = entity.StoredFileName,
                        RelativeUrl = entity.RelativeUrl,
                        ContentType = entity.ContentType,
                        SizeBytes = entity.SizeBytes,
                        CreatedDate = entity.CreatedDate,
                        UpdatedDate = entity.UpdatedDate,
                        DeletedDate = entity.DeletedDate,
                        IsDeleted = entity.IsDeleted,
                    },
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
    }
}
