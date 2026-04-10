using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using crm_api.Helpers;
using crm_api.UnitOfWork;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    public class PdfTablePresetService : IPdfTablePresetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PdfTablePresetService> _logger;
        private readonly ILocalizationService _localizationService;

        public PdfTablePresetService(
            IUnitOfWork unitOfWork,
            ILogger<PdfTablePresetService> logger,
            ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<PdfTablePresetDto>>> GetAllAsync(PdfTablePresetListRequest request)
        {
            try
            {
                var query = _unitOfWork.Repository<PdfTablePreset>().Query()
                    .Where(x => !x.IsDeleted)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    var search = request.Search.Trim();
                    query = query.Where(x => x.Name.Contains(search) || x.Key.Contains(search));
                }

                if (request.RuleType.HasValue)
                    query = query.Where(x => x.RuleType == request.RuleType.Value);
                if (request.IsActive.HasValue)
                    query = query.Where(x => x.IsActive == request.IsActive.Value);

                var totalCount = await query.CountAsync().ConfigureAwait(false);
                var items = await query
                    .OrderBy(x => x.RuleType)
                    .ThenBy(x => x.Name)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return ApiResponse<PagedResponse<PdfTablePresetDto>>.SuccessResult(
                    new PagedResponse<PdfTablePresetDto>
                    {
                        Items = items.Select(MapToDto).ToList(),
                        TotalCount = totalCount,
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                    },
                    "PDF table presets retrieved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving PDF table presets");
                return ApiResponse<PagedResponse<PdfTablePresetDto>>.ErrorResult(
                    "Error retrieving PDF table presets",
                    _localizationService.GetLocalizedString("ReportTemplateService.InternalServerError"),
                    500);
            }
        }

        public async Task<ApiResponse<PdfTablePresetDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.Repository<PdfTablePreset>().Query()
                    .Where(x => x.Id == id && !x.IsDeleted)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (entity == null)
                    return ApiResponse<PdfTablePresetDto>.ErrorResult("PDF table preset not found.", null, 404);

                return ApiResponse<PdfTablePresetDto>.SuccessResult(MapToDto(entity), "PDF table preset retrieved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving PDF table preset with ID {Id}", id);
                return ApiResponse<PdfTablePresetDto>.ErrorResult(
                    "Error retrieving PDF table preset",
                    _localizationService.GetLocalizedString("ReportTemplateService.InternalServerError"),
                    500);
            }
        }

        public async Task<ApiResponse<PdfTablePresetDto>> CreateAsync(CreatePdfTablePresetDto dto, long userId)
        {
            if (dto.Columns == null || dto.Columns.Count == 0)
                return ApiResponse<PdfTablePresetDto>.ErrorResult("At least one table column is required.", null, 400);

            try
            {
                var key = dto.Key.Trim();
                var existing = await _unitOfWork.Repository<PdfTablePreset>().Query()
                    .AnyAsync(x => !x.IsDeleted && x.Key == key)
                    .ConfigureAwait(false);
                if (existing)
                    return ApiResponse<PdfTablePresetDto>.ErrorResult("Preset key already exists.", null, 400);

                var entity = new PdfTablePreset
                {
                    RuleType = dto.RuleType,
                    Name = dto.Name.Trim(),
                    Key = key,
                    ColumnsJson = JsonSerializer.Serialize(dto.Columns, PdfReportTemplateJsonOptions.CamelCase),
                    TableOptionsJson = dto.TableOptions == null ? null : JsonSerializer.Serialize(dto.TableOptions, PdfReportTemplateJsonOptions.CamelCase),
                    IsActive = dto.IsActive,
                    CreatedBy = userId,
                    CreatedDate = DateTimeProvider.Now,
                };

                await _unitOfWork.Repository<PdfTablePreset>().AddAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                return ApiResponse<PdfTablePresetDto>.SuccessResult(MapToDto(entity), "PDF table preset created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PDF table preset");
                return ApiResponse<PdfTablePresetDto>.ErrorResult(
                    "Error creating PDF table preset",
                    _localizationService.GetLocalizedString("ReportTemplateService.InternalServerError"),
                    500);
            }
        }

        public async Task<ApiResponse<PdfTablePresetDto>> UpdateAsync(long id, UpdatePdfTablePresetDto dto, long userId)
        {
            if (dto.Columns == null || dto.Columns.Count == 0)
                return ApiResponse<PdfTablePresetDto>.ErrorResult("At least one table column is required.", null, 400);

            try
            {
                var entity = await _unitOfWork.Repository<PdfTablePreset>().Query()
                    .Where(x => x.Id == id && !x.IsDeleted)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
                if (entity == null)
                    return ApiResponse<PdfTablePresetDto>.ErrorResult("PDF table preset not found.", null, 404);

                var key = dto.Key.Trim();
                var duplicateKey = await _unitOfWork.Repository<PdfTablePreset>().Query()
                    .AnyAsync(x => !x.IsDeleted && x.Id != id && x.Key == key)
                    .ConfigureAwait(false);
                if (duplicateKey)
                    return ApiResponse<PdfTablePresetDto>.ErrorResult("Preset key already exists.", null, 400);

                entity.RuleType = dto.RuleType;
                entity.Name = dto.Name.Trim();
                entity.Key = key;
                entity.ColumnsJson = JsonSerializer.Serialize(dto.Columns, PdfReportTemplateJsonOptions.CamelCase);
                entity.TableOptionsJson = dto.TableOptions == null ? null : JsonSerializer.Serialize(dto.TableOptions, PdfReportTemplateJsonOptions.CamelCase);
                entity.IsActive = dto.IsActive;
                entity.UpdatedBy = userId;
                entity.UpdatedDate = DateTimeProvider.Now;

                await _unitOfWork.Repository<PdfTablePreset>().UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                return ApiResponse<PdfTablePresetDto>.SuccessResult(MapToDto(entity), "PDF table preset updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PDF table preset with ID {Id}", id);
                return ApiResponse<PdfTablePresetDto>.ErrorResult(
                    "Error updating PDF table preset",
                    _localizationService.GetLocalizedString("ReportTemplateService.InternalServerError"),
                    500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.Repository<PdfTablePreset>().Query()
                    .Where(x => x.Id == id && !x.IsDeleted)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
                if (entity == null)
                    return ApiResponse<bool>.ErrorResult("PDF table preset not found.", null, 404);

                var deleted = await _unitOfWork.Repository<PdfTablePreset>().SoftDeleteAsync(id).ConfigureAwait(false);
                if (!deleted)
                    return ApiResponse<bool>.ErrorResult("PDF table preset not found.", null, 404);

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                return ApiResponse<bool>.SuccessResult(true, "PDF table preset deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PDF table preset with ID {Id}", id);
                return ApiResponse<bool>.ErrorResult(
                    "Error deleting PDF table preset",
                    _localizationService.GetLocalizedString("ReportTemplateService.InternalServerError"),
                    500);
            }
        }

        private static PdfTablePresetDto MapToDto(PdfTablePreset entity)
        {
            return new PdfTablePresetDto
            {
                Id = entity.Id,
                RuleType = entity.RuleType,
                Name = entity.Name,
                Key = entity.Key,
                Columns = Deserialize<List<TableColumn>>(entity.ColumnsJson) ?? new List<TableColumn>(),
                TableOptions = Deserialize<TableOptions>(entity.TableOptionsJson),
                IsActive = entity.IsActive,
                CreatedByUserId = entity.CreatedBy,
                UpdatedByUserId = entity.UpdatedBy,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
            };
        }

        private static T? Deserialize<T>(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;
            return JsonSerializer.Deserialize<T>(json, PdfReportTemplateJsonOptions.CamelCase);
        }
    }
}
