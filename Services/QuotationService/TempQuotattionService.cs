using AutoMapper;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace crm_api.Services
{
    public class TempQuotattionService : ITempQuotattionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public TempQuotattionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<TempQuotattionGetDto>>> GetAllAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var columnMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "customerName", "Customer.CustomerName" }
                };

                var query = _unitOfWork.TempQuotattions.Query()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .Include(x => x.Customer)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .ApplyFilters(request.Filters, request.FilterLogic, columnMapping);

                var sortBy = request.SortBy ?? nameof(TempQuotattion.Id);
                query = query.ApplySorting(sortBy, request.SortDirection, columnMapping);

                var totalCount = await query.CountAsync().ConfigureAwait(false);
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync().ConfigureAwait(false);

                var dtos = items.Select(x => _mapper.Map<TempQuotattionGetDto>(x)).ToList();
                var pagedResponse = new PagedResponse<TempQuotattionGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<TempQuotattionGetDto>>.SuccessResult(
                    pagedResponse,
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<TempQuotattionGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionGetDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattions.Query()
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted).ConfigureAwait(false);

                if (entity == null)
                {
                    return ApiResponse<TempQuotattionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<TempQuotattionGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionGetDto>(entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionGetDto>> CreateAsync(TempQuotattionCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<TempQuotattion>(dto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.OfferDate = DateTime.UtcNow;

                await _unitOfWork.TempQuotattions.AddAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                var created = await _unitOfWork.TempQuotattions.Query()
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted).ConfigureAwait(false);

                return ApiResponse<TempQuotattionGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionGetDto>(created ?? entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionGetDto>> UpdateAsync(long id, TempQuotattionUpdateDto dto)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattions.GetByIdAsync(id).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<TempQuotattionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.TempQuotattions.UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                var updated = await _unitOfWork.TempQuotattions.Query()
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted).ConfigureAwait(false);

                return ApiResponse<TempQuotattionGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionGetDto>(updated ?? entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionGetDto>> SetApprovedAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattions.GetByIdAsync(id).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<TempQuotattionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                entity.IsApproved = true;
                entity.ApprovedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.TempQuotattions.UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                var approved = await _unitOfWork.TempQuotattions.Query()
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted).ConfigureAwait(false);

                return ApiResponse<TempQuotattionGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionGetDto>(approved ?? entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionApproved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattions.GetByIdAsync(id).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.TempQuotattions.SoftDeleteAsync(id).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<TempQuotattionLineGetDto>>> GetLinesByHeaderIdAsync(long tempQuotattionId)
        {
            try
            {
                var exists = await _unitOfWork.TempQuotattions.Query().AnyAsync(x => x.Id == tempQuotattionId && !x.IsDeleted).ConfigureAwait(false);
                if (!exists)
                {
                    return ApiResponse<List<TempQuotattionLineGetDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var lines = await _unitOfWork.TempQuotattionLines.Query()
                    .AsNoTracking()
                    .Where(x => x.TempQuotattionId == tempQuotattionId && !x.IsDeleted)
                    .ToListAsync().ConfigureAwait(false);

                var dtos = lines.Select(_mapper.Map<TempQuotattionLineGetDto>).ToList();
                return ApiResponse<List<TempQuotattionLineGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLinesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TempQuotattionLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionLineGetDto>> GetLineByIdAsync(long lineId)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattionLines.Query()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == lineId && !x.IsDeleted).ConfigureAwait(false);

                if (entity == null)
                {
                    return ApiResponse<TempQuotattionLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<TempQuotattionLineGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionLineGetDto>(entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionLineGetDto>> CreateLineAsync(TempQuotattionLineCreateDto dto)
        {
            try
            {
                var exists = await _unitOfWork.TempQuotattions.Query().AnyAsync(x => x.Id == dto.TempQuotattionId && !x.IsDeleted).ConfigureAwait(false);
                if (!exists)
                {
                    return ApiResponse<TempQuotattionLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var entity = _mapper.Map<TempQuotattionLine>(dto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.TempQuotattionLines.AddAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<TempQuotattionLineGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionLineGetDto>(entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<TempQuotattionLineGetDto>>> CreateLinesAsync(List<TempQuotattionLineCreateDto> dtos)
        {
            try
            {
                if (dtos == null || dtos.Count == 0)
                {
                    return ApiResponse<List<TempQuotattionLineGetDto>>.SuccessResult(
                        new List<TempQuotattionLineGetDto>(),
                        _localizationService.GetLocalizedString("TempQuotattionService.NoLineToCreate"));
                }

                var headerIds = dtos.Select(x => x.TempQuotattionId).Distinct().ToList();
                var existingHeaderIds = await _unitOfWork.TempQuotattions.Query()
                    .Where(x => headerIds.Contains(x.Id) && !x.IsDeleted)
                    .Select(x => x.Id)
                    .ToListAsync().ConfigureAwait(false);

                if (existingHeaderIds.Count != headerIds.Count)
                {
                    return ApiResponse<List<TempQuotattionLineGetDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var entities = _mapper.Map<List<TempQuotattionLine>>(dtos);
                foreach (var entity in entities)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }

                await _unitOfWork.TempQuotattionLines.AddAllAsync(entities).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                var response = entities.Select(_mapper.Map<TempQuotattionLineGetDto>).ToList();
                return ApiResponse<List<TempQuotattionLineGetDto>>.SuccessResult(response, _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLinesCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TempQuotattionLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionLineGetDto>> UpdateLineAsync(long lineId, TempQuotattionLineUpdateDto dto)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattionLines.GetByIdAsync(lineId).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<TempQuotattionLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.TempQuotattionLines.UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<TempQuotattionLineGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionLineGetDto>(entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteLineAsync(long lineId)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattionLines.GetByIdAsync(lineId).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.TempQuotattionLines.SoftDeleteAsync(lineId).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionLineDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<TempQuotattionExchangeLineGetDto>>> GetExchangeLinesByHeaderIdAsync(long tempQuotattionId)
        {
            try
            {
                var exists = await _unitOfWork.TempQuotattions.Query().AnyAsync(x => x.Id == tempQuotattionId && !x.IsDeleted).ConfigureAwait(false);
                if (!exists)
                {
                    return ApiResponse<List<TempQuotattionExchangeLineGetDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var rows = await _unitOfWork.TempQuotattionExchangeLines.Query()
                    .AsNoTracking()
                    .Where(x => x.TempQuotattionId == tempQuotattionId && !x.IsDeleted)
                    .ToListAsync().ConfigureAwait(false);

                var dtos = rows.Select(_mapper.Map<TempQuotattionExchangeLineGetDto>).ToList();
                return ApiResponse<List<TempQuotattionExchangeLineGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLinesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TempQuotattionExchangeLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionExchangeLineGetDto>> GetExchangeLineByIdAsync(long exchangeLineId)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattionExchangeLines.Query()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == exchangeLineId && !x.IsDeleted).ConfigureAwait(false);

                if (entity == null)
                {
                    return ApiResponse<TempQuotattionExchangeLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<TempQuotattionExchangeLineGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionExchangeLineGetDto>(entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionExchangeLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionExchangeLineGetDto>> CreateExchangeLineAsync(TempQuotattionExchangeLineCreateDto dto)
        {
            try
            {
                var exists = await _unitOfWork.TempQuotattions.Query().AnyAsync(x => x.Id == dto.TempQuotattionId && !x.IsDeleted).ConfigureAwait(false);
                if (!exists)
                {
                    return ApiResponse<TempQuotattionExchangeLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var entity = _mapper.Map<TempQuotattionExchangeLine>(dto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.TempQuotattionExchangeLines.AddAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<TempQuotattionExchangeLineGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionExchangeLineGetDto>(entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionExchangeLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TempQuotattionExchangeLineGetDto>> UpdateExchangeLineAsync(long exchangeLineId, TempQuotattionExchangeLineUpdateDto dto)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattionExchangeLines.GetByIdAsync(exchangeLineId).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<TempQuotattionExchangeLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.TempQuotattionExchangeLines.UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<TempQuotattionExchangeLineGetDto>.SuccessResult(
                    _mapper.Map<TempQuotattionExchangeLineGetDto>(entity),
                    _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TempQuotattionExchangeLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteExchangeLineAsync(long exchangeLineId)
        {
            try
            {
                var entity = await _unitOfWork.TempQuotattionExchangeLines.GetByIdAsync(exchangeLineId).ConfigureAwait(false);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineNotFound"),
                        _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.TempQuotattionExchangeLines.SoftDeleteAsync(exchangeLineId).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("TempQuotattionService.TempQuotattionExchangeLineDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("TempQuotattionService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
