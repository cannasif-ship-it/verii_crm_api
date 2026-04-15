using AutoMapper;
using crm_api.Helpers;
using crm_api.Modules.Definitions.Application.Dtos;
using crm_api.Modules.Definitions.Domain.Entities;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Modules.Definitions.Application.Services
{
    public class SalesRepCodeUserMatchService : ISalesRepCodeUserMatchService
    {
        private static readonly string[] SearchableColumns =
        {
            "SalesRepCode.SalesRepCodeValue",
            "SalesRepCode.Name",
            "User.FirstName",
            "User.LastName",
            "User.Username",
            "User.Email"
        };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SalesRepCodeUserMatchService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<SalesRepCodeUserMatchGetDto>>> GetAllSalesRepCodeUserMatchesAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var columnMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "salesRepCodeId", nameof(SalesRepCodeUserMatch.SalesRepCodeId) },
                    { "userId", nameof(SalesRepCodeUserMatch.UserId) },
                    { "salesRepCode", "SalesRepCode.SalesRepCodeValue" },
                    { "salesRepName", "SalesRepCode.Name" },
                    { "username", "User.Username" },
                    { "userEmail", "User.Email" }
                };

                var query = _unitOfWork.SalesRepCodeUserMatches.Query()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .Include(x => x.SalesRepCode)
                    .Include(x => x.User)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .ApplySearch(request.Search, SearchableColumns)
                    .ApplyFilters(request.Filters, request.FilterLogic, columnMapping)
                    .ApplySorting(request.SortBy ?? nameof(SalesRepCodeUserMatch.Id), request.SortDirection, columnMapping);

                var totalCount = await query.CountAsync().ConfigureAwait(false);
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return ApiResponse<PagedResponse<SalesRepCodeUserMatchGetDto>>.SuccessResult(
                    new PagedResponse<SalesRepCodeUserMatchGetDto>
                    {
                        Items = items.Select(x => _mapper.Map<SalesRepCodeUserMatchGetDto>(x)).ToList(),
                        TotalCount = totalCount,
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    },
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SalesRepCodeUserMatchGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.GetAllSalesRepCodeUserMatchesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SalesRepCodeUserMatchGetDto>> GetSalesRepCodeUserMatchByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SalesRepCodeUserMatches.Query()
                    .AsNoTracking()
                    .Include(x => x.SalesRepCode)
                    .Include(x => x.User)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (entity == null)
                {
                    return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchNotFound"),
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchNotFound"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<SalesRepCodeUserMatchGetDto>.SuccessResult(
                    _mapper.Map<SalesRepCodeUserMatchGetDto>(entity),
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.GetSalesRepCodeUserMatchByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SalesRepCodeUserMatchGetDto>> CreateSalesRepCodeUserMatchAsync(SalesRepCodeUserMatchCreateDto dto)
        {
            try
            {
                var validationError = await ValidateReferencesAsync(dto.SalesRepCodeId, dto.UserId).ConfigureAwait(false);
                if (validationError != null)
                {
                    return validationError;
                }

                if (await MatchExistsAsync(dto.SalesRepCodeId, dto.UserId).ConfigureAwait(false))
                {
                    return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchAlreadyExists"),
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                var entity = _mapper.Map<SalesRepCodeUserMatch>(dto);
                entity.CreatedDate = DateTimeProvider.Now;

                await _unitOfWork.SalesRepCodeUserMatches.AddAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return await GetSalesRepCodeUserMatchByIdAsync(entity.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.CreateSalesRepCodeUserMatchExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SalesRepCodeUserMatchGetDto>> UpdateSalesRepCodeUserMatchAsync(long id, SalesRepCodeUserMatchUpdateDto dto)
        {
            try
            {
                var entity = await _unitOfWork.SalesRepCodeUserMatches.GetByIdForUpdateAsync(id).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchNotFound"),
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var validationError = await ValidateReferencesAsync(dto.SalesRepCodeId, dto.UserId).ConfigureAwait(false);
                if (validationError != null)
                {
                    return validationError;
                }

                var duplicateExists = await _unitOfWork.SalesRepCodeUserMatches.Query()
                    .AnyAsync(x => !x.IsDeleted
                        && x.Id != id
                        && x.SalesRepCodeId == dto.SalesRepCodeId
                        && x.UserId == dto.UserId)
                    .ConfigureAwait(false);

                if (duplicateExists)
                {
                    return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchAlreadyExists"),
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;

                await _unitOfWork.SalesRepCodeUserMatches.UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return await GetSalesRepCodeUserMatchByIdAsync(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.UpdateSalesRepCodeUserMatchExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteSalesRepCodeUserMatchAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.SalesRepCodeUserMatches.SoftDeleteAsync(id).ConfigureAwait(false);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchNotFound"),
                        _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.SalesRepCodeUserMatchDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.InternalServerError"),
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.DeleteSalesRepCodeUserMatchExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<bool> MatchExistsAsync(long salesRepCodeId, long userId)
        {
            return await _unitOfWork.SalesRepCodeUserMatches.Query()
                .AnyAsync(x => !x.IsDeleted && x.SalesRepCodeId == salesRepCodeId && x.UserId == userId)
                .ConfigureAwait(false);
        }

        private async Task<ApiResponse<SalesRepCodeUserMatchGetDto>?> ValidateReferencesAsync(long salesRepCodeId, long userId)
        {
            var salesRepCodeExists = await _unitOfWork.SalesRepCodes.Query()
                .AnyAsync(x => x.Id == salesRepCodeId && !x.IsDeleted)
                .ConfigureAwait(false);

            if (!salesRepCodeExists)
            {
                return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                    _localizationService.GetLocalizedString("SalesRepCodeService.SalesRepCodeNotFound"),
                    StatusCodes.Status404NotFound);
            }

            var userExists = await _unitOfWork.Users.Query()
                .AnyAsync(x => x.Id == userId && !x.IsDeleted)
                .ConfigureAwait(false);

            if (!userExists)
            {
                return ApiResponse<SalesRepCodeUserMatchGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.UserNotFound"),
                    _localizationService.GetLocalizedString("SalesRepCodeUserMatchService.UserNotFound"),
                    StatusCodes.Status404NotFound);
            }

            return null;
        }
    }
}
