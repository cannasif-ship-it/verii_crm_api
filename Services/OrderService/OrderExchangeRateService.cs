using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services
{
    public class OrderExchangeRateService : IOrderExchangeRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public OrderExchangeRateService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<OrderExchangeRateGetDto>>> GetAllOrderExchangeRatesAsync(PagedRequest request)
        {
            try
            {
                if (request == null)
                {
                    request = new PagedRequest();
                }

                if (request.Filters == null)
                {
                    request.Filters = new List<Filter>();
                }

                var query = _unitOfWork.OrderExchangeRates
                    .Query()
                    .Where(e => !e.IsDeleted)
                    .Include(e => e.Order)
                    .Include(e => e.CreatedByUser)
                    .Include(e => e.UpdatedByUser)
                    .Include(e => e.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(OrderExchangeRate.Id);
                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<OrderExchangeRateGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<OrderExchangeRateGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<OrderExchangeRateGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("OrderExchangeRateService.RatesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<OrderExchangeRateGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderExchangeRateService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderExchangeRateService.GetAllRatesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderExchangeRateGetDto>> GetOrderExchangeRateByIdAsync(long id)
        {
            try
            {
                var exchangeRate = await _unitOfWork.OrderExchangeRates
                    .Query()
                    .Include(e => e.Order)
                    .Include(e => e.CreatedByUser)
                    .Include(e => e.UpdatedByUser)
                    .Include(e => e.DeletedByUser)
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

                if (exchangeRate == null)
                {
                    return ApiResponse<OrderExchangeRateGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var exchangeRateDto = _mapper.Map<OrderExchangeRateGetDto>(exchangeRate);
                return ApiResponse<OrderExchangeRateGetDto>.SuccessResult(exchangeRateDto, _localizationService.GetLocalizedString("OrderExchangeRateService.RateRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderExchangeRateGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderExchangeRateService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderExchangeRateService.GetByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderExchangeRateGetDto>> CreateOrderExchangeRateAsync(OrderExchangeRateCreateDto createDto)
        {
            try
            {
                var exchangeRate = _mapper.Map<OrderExchangeRate>(createDto);
                await _unitOfWork.OrderExchangeRates.AddAsync(exchangeRate);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var exchangeRateWithNav = await _unitOfWork.OrderExchangeRates
                    .Query()
                    .Include(e => e.Order)
                    .Include(e => e.CreatedByUser)
                    .Include(e => e.UpdatedByUser)
                    .Include(e => e.DeletedByUser)
                    .FirstOrDefaultAsync(e => e.Id == exchangeRate.Id && !e.IsDeleted);

                if (exchangeRateWithNav == null)
                {
                    return ApiResponse<OrderExchangeRateGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var exchangeRateDto = _mapper.Map<OrderExchangeRateGetDto>(exchangeRateWithNav);
                return ApiResponse<OrderExchangeRateGetDto>.SuccessResult(exchangeRateDto, _localizationService.GetLocalizedString("OrderExchangeRateService.RateCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderExchangeRateGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderExchangeRateService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderExchangeRateService.CreateRateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderExchangeRateGetDto>> UpdateOrderExchangeRateAsync(long id, OrderExchangeRateUpdateDto updateDto)
        {
            try
            {
                var exchangeRate = await _unitOfWork.OrderExchangeRates.GetByIdAsync(id);
                if (exchangeRate == null)
                {
                    return ApiResponse<OrderExchangeRateGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateDto, exchangeRate);
                await _unitOfWork.OrderExchangeRates.UpdateAsync(exchangeRate);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var exchangeRateWithNav = await _unitOfWork.OrderExchangeRates
                    .Query()
                    .Include(e => e.Order)
                    .Include(e => e.CreatedByUser)
                    .Include(e => e.UpdatedByUser)
                    .Include(e => e.DeletedByUser)
                    .FirstOrDefaultAsync(e => e.Id == exchangeRate.Id && !e.IsDeleted);

                if (exchangeRateWithNav == null)
                {
                    return ApiResponse<OrderExchangeRateGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var exchangeRateDto = _mapper.Map<OrderExchangeRateGetDto>(exchangeRateWithNav);
                return ApiResponse<OrderExchangeRateGetDto>.SuccessResult(exchangeRateDto, _localizationService.GetLocalizedString("OrderExchangeRateService.RateUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderExchangeRateGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderExchangeRateService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderExchangeRateService.UpdateRateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteOrderExchangeRateAsync(long id)
        {
            try
            {
                var exchangeRate = await _unitOfWork.OrderExchangeRates.GetByIdAsync(id);
                if (exchangeRate == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        _localizationService.GetLocalizedString("OrderExchangeRateService.RateNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.OrderExchangeRates.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("OrderExchangeRateService.RateDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderExchangeRateService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderExchangeRateService.DeleteRateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<OrderExchangeRateGetDto>>> GetOrderExchangeRatesByOrderIdAsync(long orderId)
        {
            try
            {
                var exchangeRates = await _unitOfWork.OrderExchangeRates
                    .Query()
                    .Where(e => e.OrderId == orderId && !e.IsDeleted)
                    .Include(e => e.CreatedByUser)
                    .Include(e => e.UpdatedByUser)
                    .Include(e => e.DeletedByUser)
                    .ToListAsync();

                var dtos = _mapper.Map<List<OrderExchangeRateGetDto>>(exchangeRates);
                return ApiResponse<List<OrderExchangeRateGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("OrderExchangeRateService.RatesByOrderRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OrderExchangeRateGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderExchangeRateService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderExchangeRateService.GetByOrderIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
