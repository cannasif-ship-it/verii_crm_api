using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Data;
using Microsoft.AspNetCore.Http;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace crm_api.Services
{
    public class OrderLineService : IOrderLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;

        public OrderLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, CmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
        }

        public async Task<ApiResponse<PagedResponse<OrderLineGetDto>>> GetAllOrderLinesAsync(PagedRequest request)
        {
            try
            {
                var query = _context.OrderLines
                    .AsNoTracking()
                    .Where(ql => !ql.IsDeleted)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(OrderLine.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .Select(x => _mapper.Map<OrderLineGetDto>(x))
                    .ToListAsync();

                var pagedResponse = new PagedResponse<OrderLineGetDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<OrderLineGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("OrderLineService.OrderLinesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<OrderLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderLineService.GetAllExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<OrderLineGetDto>> GetOrderLineByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.OrderLines.GetByIdAsync(id);
                if (line == null)
                {
                    return ApiResponse<OrderLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderLineService.OrderLineNotFound"),
                        _localizationService.GetLocalizedString("OrderLineService.OrderLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var dto = _mapper.Map<OrderLineGetDto>(line);
                return ApiResponse<OrderLineGetDto>.SuccessResult(dto, _localizationService.GetLocalizedString("OrderLineService.OrderLineRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderLineService.GetByIdExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<OrderLineDto>> CreateOrderLineAsync(CreateOrderLineDto createOrderLineDto)
        {
            try
            {
                var entity = _mapper.Map<OrderLine>(createOrderLineDto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.OrderLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<OrderLineDto>(entity);
                return ApiResponse<OrderLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("OrderLineService.OrderLineCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderLineService.CreateExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<OrderLineDto>> UpdateOrderLineAsync(long id, UpdateOrderLineDto updateOrderLineDto)
        {
            try
            {
                var existing = await _unitOfWork.OrderLines.GetByIdAsync(id);
                if (existing == null)
                {
                    return ApiResponse<OrderLineDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderLineService.OrderLineNotFound"),
                        _localizationService.GetLocalizedString("OrderLineService.OrderLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateOrderLineDto, existing);
                existing.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.OrderLines.UpdateAsync(existing);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<OrderLineDto>(existing);
                return ApiResponse<OrderLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("OrderLineService.OrderLineUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderLineService.UpdateExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<object>> DeleteOrderLineAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.OrderLines.GetByIdAsync(id);
                if (existing == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderLineService.OrderLineNotFound"),
                        _localizationService.GetLocalizedString("OrderLineService.OrderLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.OrderLines.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("OrderLineService.OrderLineDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderLineService.DeleteExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<OrderLineGetDto>>> GetOrderLinesByOrderIdAsync(long orderId)
        {
            try
            {
                var dtos = await _unitOfWork.OrderLines
                    .Query()
                    .Where(q => q.OrderId == orderId && !q.IsDeleted)
                    .Join(
                        _unitOfWork.Stocks.Query(),
                        ql => ql.ProductCode,
                        s => s.ErpStockCode,
                        (ql, s) => new
                        {
                            OrderLine = ql,
                            ProductName = s.StockName,
                            GroupCode = s.GrupKodu
                        })
                    .Select(x => new OrderLineGetDto
                    {
                        OrderId = x.OrderLine.OrderId,
                        ProductCode = x.OrderLine.ProductCode,
                        ProductName = x.ProductName,
                        GroupCode = x.GroupCode,
                        Quantity = x.OrderLine.Quantity,
                        UnitPrice = x.OrderLine.UnitPrice,
                        DiscountRate1 = x.OrderLine.DiscountRate1,
                        DiscountAmount1 = x.OrderLine.DiscountAmount1,
                        DiscountRate2 = x.OrderLine.DiscountRate2,
                        DiscountAmount2 = x.OrderLine.DiscountAmount2,
                        DiscountRate3 = x.OrderLine.DiscountRate3,
                        DiscountAmount3 = x.OrderLine.DiscountAmount3,
                        VatRate = x.OrderLine.VatRate,
                        VatAmount = x.OrderLine.VatAmount,
                        LineTotal = x.OrderLine.LineTotal,
                        LineGrandTotal = x.OrderLine.LineGrandTotal,
                        Description = x.OrderLine.Description,
                        PricingRuleHeaderId = x.OrderLine.PricingRuleHeaderId,
                        RelatedStockId = x.OrderLine.RelatedStockId,
                        RelatedProductKey = x.OrderLine.RelatedProductKey,
                        IsMainRelatedProduct = x.OrderLine.IsMainRelatedProduct,
                        ApprovalStatus = x.OrderLine.ApprovalStatus
                    })
                    .ToListAsync();

                return ApiResponse<List<OrderLineGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("OrderLineService.OrderLinesByOrderRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OrderLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderLineService.GetByOrderIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
