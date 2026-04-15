using AutoMapper;
using crm_api.Helpers;
using crm_api.Modules.Definitions.Application.Dtos;
using crm_api.Modules.Definitions.Domain.Entities;
using crm_api.Modules.Integrations.Application.Services;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Modules.Definitions.Application.Services
{
    public class SalesRepCodeService : ISalesRepCodeService
    {
        private static readonly string[] SearchableColumns =
        {
            nameof(SalesRepCode.SalesRepCodeValue),
            nameof(SalesRepCode.SalesRepDescription),
            nameof(SalesRepCode.Name)
        };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalesRepCodeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IErpService erpService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<PagedResponse<SalesRepCodeGetDto>>> GetAllSalesRepCodesAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var columnMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "salesRepCode", nameof(SalesRepCode.SalesRepCodeValue) }
                };

                var query = _unitOfWork.SalesRepCodes.Query()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .ApplySearch(request.Search, SearchableColumns)
                    .ApplyFilters(request.Filters, request.FilterLogic, columnMapping)
                    .ApplySorting(request.SortBy ?? nameof(SalesRepCode.Id), request.SortDirection, columnMapping);

                var totalCount = await query.CountAsync().ConfigureAwait(false);
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var pagedResponse = new PagedResponse<SalesRepCodeGetDto>
                {
                    Items = items.Select(x => _mapper.Map<SalesRepCodeGetDto>(x)).ToList(),
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<SalesRepCodeGetDto>>.SuccessResult(
                    pagedResponse,
                    _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SalesRepCodeGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeService.GetAllSalesRepCodesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SalesRepCodeGetDto>> GetSalesRepCodeByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SalesRepCodes.Query()
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (entity == null)
                {
                    return ApiResponse<SalesRepCodeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<SalesRepCodeGetDto>.SuccessResult(
                    _mapper.Map<SalesRepCodeGetDto>(entity),
                    _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesRepCodeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeService.GetSalesRepCodeByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SalesRepCodeGetDto>> CreateSalesRepCodeAsync(SalesRepCodeCreateDto dto)
        {
            try
            {
                NormalizeDto(dto);

                var duplicateExists = await SalesRepCodeExistsAsync(dto.BranchCode, dto.SalesRepCode).ConfigureAwait(false);
                if (duplicateExists)
                {
                    return ApiResponse<SalesRepCodeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeAlreadyExists"),
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                var entity = _mapper.Map<SalesRepCode>(dto);
                entity.CreatedDate = DateTimeProvider.Now;

                await _unitOfWork.SalesRepCodes.AddAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return await GetSalesRepCodeByIdAsync(entity.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesRepCodeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeService.CreateSalesRepCodeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SalesRepCodeGetDto>> UpdateSalesRepCodeAsync(long id, SalesRepCodeUpdateDto dto)
        {
            try
            {
                NormalizeDto(dto);

                var entity = await _unitOfWork.SalesRepCodes.GetByIdForUpdateAsync(id).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SalesRepCodeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var duplicateExists = await _unitOfWork.SalesRepCodes.Query()
                    .AnyAsync(x => !x.IsDeleted
                        && x.Id != id
                        && x.BranchCode == dto.BranchCode
                        && x.SalesRepCodeValue == dto.SalesRepCode)
                    .ConfigureAwait(false);

                if (duplicateExists)
                {
                    return ApiResponse<SalesRepCodeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeAlreadyExists"),
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;

                await _unitOfWork.SalesRepCodes.UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return await GetSalesRepCodeByIdAsync(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesRepCodeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeService.UpdateSalesRepCodeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteSalesRepCodeAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.SalesRepCodes.SoftDeleteAsync(id).ConfigureAwait(false);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                        _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeService.DeleteSalesRepCodeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SalesRepCodeSyncResponseDto>> SyncSalesRepCodesFromErpAsync()
        {
            try
            {
                var branchScope = ResolveBranchScope();
                var erpRows = new List<CariPlasiyerDto>();

                if (branchScope.Count == 0)
                {
                    var erpResponse = await _erpService.GetCariPlasiyerAsync().ConfigureAwait(false);
                    if (!erpResponse.Success || erpResponse.Data == null)
                    {
                        return ApiResponse<SalesRepCodeSyncResponseDto>.ErrorResult(
                            _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                            erpResponse?.Message ?? _localizationService.GetLocalizedString("SalesRepCodeService.ErpSyncFailed"),
                            erpResponse?.StatusCode ?? StatusCodes.Status500InternalServerError);
                    }

                    erpRows.AddRange(erpResponse.Data);
                }
                else
                {
                    foreach (var branchCode in branchScope)
                    {
                        var erpResponse = await _erpService.GetCariPlasiyerAsync(branchCode.ToString(), null).ConfigureAwait(false);
                        if (!erpResponse.Success || erpResponse.Data == null)
                        {
                            return ApiResponse<SalesRepCodeSyncResponseDto>.ErrorResult(
                                _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                                erpResponse?.Message ?? _localizationService.GetLocalizedString("SalesRepCodeService.ErpSyncFailed"),
                                erpResponse?.StatusCode ?? StatusCodes.Status500InternalServerError);
                        }

                        erpRows.AddRange(erpResponse.Data);
                    }
                }

                var dedupedRows = erpRows
                    .Where(x => !string.IsNullOrWhiteSpace(x.PlasiyerKodu))
                    .GroupBy(x => new { x.SubeKodu, Code = x.PlasiyerKodu.Trim().ToUpperInvariant() })
                    .Select(g => g.First())
                    .ToList();

                var existing = await _unitOfWork.SalesRepCodes
                    .Query(tracking: true, ignoreQueryFilters: true)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var existingByKey = existing.ToDictionary(
                    x => BuildKey(x.BranchCode, x.SalesRepCodeValue),
                    StringComparer.OrdinalIgnoreCase);

                var createdCount = 0;
                var updatedCount = 0;
                var deactivatedCount = 0;
                var touchedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var row in dedupedRows)
                {
                    var normalizedCode = row.PlasiyerKodu.Trim();
                    var normalizedDescription = NormalizeNullable(row.PlasiyerAciklama);
                    var normalizedName = NormalizeNullable(row.Isim);
                    var key = BuildKey(row.SubeKodu, normalizedCode);
                    touchedKeys.Add(key);

                    if (!existingByKey.TryGetValue(key, out var entity))
                    {
                        await _unitOfWork.SalesRepCodes.AddAsync(new SalesRepCode
                        {
                            BranchCode = row.SubeKodu,
                            SalesRepCodeValue = normalizedCode,
                            SalesRepDescription = normalizedDescription,
                            Name = normalizedName,
                            CreatedDate = DateTimeProvider.Now,
                            IsDeleted = false
                        }).ConfigureAwait(false);
                        createdCount++;
                        continue;
                    }

                    var changed = false;
                    if (entity.SalesRepDescription != normalizedDescription) { entity.SalesRepDescription = normalizedDescription; changed = true; }
                    if (entity.Name != normalizedName) { entity.Name = normalizedName; changed = true; }
                    if (entity.IsDeleted)
                    {
                        entity.IsDeleted = false;
                        entity.DeletedDate = null;
                        entity.DeletedBy = null;
                        changed = true;
                    }

                    if (changed)
                    {
                        entity.UpdatedDate = DateTimeProvider.Now;
                        updatedCount++;
                    }
                }

                var scopedExisting = branchScope.Count == 0
                    ? existing
                    : existing.Where(x => branchScope.Contains(x.BranchCode)).ToList();

                foreach (var entity in scopedExisting.Where(x => !x.IsDeleted))
                {
                    var key = BuildKey(entity.BranchCode, entity.SalesRepCodeValue);
                    if (touchedKeys.Contains(key))
                        continue;

                    entity.IsDeleted = true;
                    entity.DeletedDate = DateTimeProvider.Now;
                    deactivatedCount++;
                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<SalesRepCodeSyncResponseDto>.SuccessResult(
                    new SalesRepCodeSyncResponseDto
                    {
                        CreatedCount = createdCount,
                        UpdatedCount = updatedCount,
                        DeactivatedCount = deactivatedCount
                    },
                    _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodesSynced"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesRepCodeSyncResponseDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeService.SyncSalesRepCodesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<bool> SalesRepCodeExistsAsync(short branchCode, string salesRepCode)
        {
            return await _unitOfWork.SalesRepCodes.Query()
                .AnyAsync(x => !x.IsDeleted && x.BranchCode == branchCode && x.SalesRepCodeValue == salesRepCode)
                .ConfigureAwait(false);
        }

        private static string BuildKey(short branchCode, string salesRepCode)
            => $"{branchCode}|{salesRepCode.Trim()}";

        private List<short> ResolveBranchScope()
        {
            var branchScope = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
            return string.IsNullOrWhiteSpace(branchScope)
                ? new List<short>()
                : branchScope.Split(',')
                    .Select(x => x.Trim())
                    .Where(x => short.TryParse(x, out _))
                    .Select(short.Parse)
                    .Distinct()
                    .ToList();
        }

        private static void NormalizeDto(SalesRepCodeCreateDto dto)
        {
            dto.SalesRepCode = (dto.SalesRepCode ?? string.Empty).Trim();
            dto.SalesRepDescription = NormalizeNullable(dto.SalesRepDescription);
            dto.Name = NormalizeNullable(dto.Name);
        }

        private static void NormalizeDto(SalesRepCodeUpdateDto dto)
        {
            dto.SalesRepCode = (dto.SalesRepCode ?? string.Empty).Trim();
            dto.SalesRepDescription = NormalizeNullable(dto.SalesRepDescription);
            dto.Name = NormalizeNullable(dto.Name);
        }

        private static string? NormalizeNullable(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
