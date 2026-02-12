using AutoMapper;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services
{
    public class OrderNotesService : IOrderNotesService
    {
        private const int MaxNoteCount = 15;
        private const int MaxNoteLength = 100;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public OrderNotesService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<OrderNotesGetDto>>> GetAllOrderNotesAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var query = _unitOfWork.OrderNotes.Query()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .ApplyFilters(request.Filters, request.FilterLogic)
                    .ApplySorting(request.SortBy ?? nameof(OrderNotes.Id), request.SortDirection);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .Select(x => _mapper.Map<OrderNotesGetDto>(x))
                    .ToListAsync();

                var pagedResponse = new PagedResponse<OrderNotesGetDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<OrderNotesGetDto>>.SuccessResult(pagedResponse, "Order notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<OrderNotesGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderNotesGetDto>> GetOrderNotesByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.OrderNotes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<OrderNotesGetDto>.ErrorResult("Order notes not found.", "Order notes not found.", StatusCodes.Status404NotFound);
                }

                var dto = _mapper.Map<OrderNotesGetDto>(entity);
                return ApiResponse<OrderNotesGetDto>.SuccessResult(dto, "Order notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderNotesGetDto>> GetNotesByOrderIdAsync(long orderId)
        {
            try
            {
                var orderExists = await _unitOfWork.Orders.Query().AnyAsync(x => x.Id == orderId && !x.IsDeleted);
                if (!orderExists)
                {
                    return ApiResponse<OrderNotesGetDto>.ErrorResult("Order not found.", "Order not found.", StatusCodes.Status404NotFound);
                }

                var entity = await _unitOfWork.OrderNotes.Query().AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId && !x.IsDeleted);
                if (entity == null)
                {
                    return ApiResponse<OrderNotesGetDto>.SuccessResult(new OrderNotesGetDto { OrderId = orderId }, "Order notes retrieved.");
                }

                return ApiResponse<OrderNotesGetDto>.SuccessResult(_mapper.Map<OrderNotesGetDto>(entity), "Order notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderNotesDto>> CreateOrderNotesAsync(CreateOrderNotesDto createOrderNotesDto)
        {
            try
            {
                var orderExists = await _unitOfWork.Orders.Query().AnyAsync(x => x.Id == createOrderNotesDto.OrderId && !x.IsDeleted);
                if (!orderExists)
                {
                    return ApiResponse<OrderNotesDto>.ErrorResult("Order not found.", "Order not found.", StatusCodes.Status404NotFound);
                }

                var exists = await _unitOfWork.OrderNotes.Query().AnyAsync(x => x.OrderId == createOrderNotesDto.OrderId && !x.IsDeleted);
                if (exists)
                {
                    return ApiResponse<OrderNotesDto>.ErrorResult("Order notes already exist for this order.", "Order notes already exist for this order.", StatusCodes.Status400BadRequest);
                }

                var entity = _mapper.Map<OrderNotes>(createOrderNotesDto);
                entity.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.OrderNotes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<OrderNotesDto>.SuccessResult(_mapper.Map<OrderNotesDto>(entity), "Order notes created.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderNotesDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderNotesDto>> UpdateOrderNotesAsync(long id, UpdateOrderNotesDto updateOrderNotesDto)
        {
            try
            {
                var existing = await _unitOfWork.OrderNotes.GetByIdAsync(id);
                if (existing == null || existing.IsDeleted)
                {
                    return ApiResponse<OrderNotesDto>.ErrorResult("Order notes not found.", "Order notes not found.", StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateOrderNotesDto, existing);
                existing.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.OrderNotes.UpdateAsync(existing);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<OrderNotesDto>.SuccessResult(_mapper.Map<OrderNotesDto>(existing), "Order notes updated.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderNotesDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderNotesGetDto>> UpdateNotesListByOrderIdAsync(long orderId, UpdateOrderNotesListDto request)
        {
            try
            {
                var orderExists = await _unitOfWork.Orders.Query().AnyAsync(x => x.Id == orderId && !x.IsDeleted);
                if (!orderExists)
                {
                    return ApiResponse<OrderNotesGetDto>.ErrorResult("Order not found.", "Order not found.", StatusCodes.Status404NotFound);
                }

                var notes = (request?.Notes ?? new List<string>())
                    .Where(x => x != null)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                if (notes.Count > MaxNoteCount)
                {
                    return ApiResponse<OrderNotesGetDto>.ErrorResult($"A maximum of {MaxNoteCount} notes is allowed.", $"A maximum of {MaxNoteCount} notes is allowed.", StatusCodes.Status400BadRequest);
                }

                if (notes.Any(x => x.Length > MaxNoteLength))
                {
                    return ApiResponse<OrderNotesGetDto>.ErrorResult($"Each note can be at most {MaxNoteLength} characters.", $"Each note can be at most {MaxNoteLength} characters.", StatusCodes.Status400BadRequest);
                }

                var entity = await _unitOfWork.OrderNotes.Query().FirstOrDefaultAsync(x => x.OrderId == orderId && !x.IsDeleted);
                if (entity == null)
                {
                    entity = new OrderNotes { OrderId = orderId, CreatedDate = DateTime.UtcNow };
                    await _unitOfWork.OrderNotes.AddAsync(entity);
                }
                else
                {
                    entity.UpdatedDate = DateTime.UtcNow;
                    await _unitOfWork.OrderNotes.UpdateAsync(entity);
                }

                entity.Note1 = notes.ElementAtOrDefault(0);
                entity.Note2 = notes.ElementAtOrDefault(1);
                entity.Note3 = notes.ElementAtOrDefault(2);
                entity.Note4 = notes.ElementAtOrDefault(3);
                entity.Note5 = notes.ElementAtOrDefault(4);
                entity.Note6 = notes.ElementAtOrDefault(5);
                entity.Note7 = notes.ElementAtOrDefault(6);
                entity.Note8 = notes.ElementAtOrDefault(7);
                entity.Note9 = notes.ElementAtOrDefault(8);
                entity.Note10 = notes.ElementAtOrDefault(9);
                entity.Note11 = notes.ElementAtOrDefault(10);
                entity.Note12 = notes.ElementAtOrDefault(11);
                entity.Note13 = notes.ElementAtOrDefault(12);
                entity.Note14 = notes.ElementAtOrDefault(13);
                entity.Note15 = notes.ElementAtOrDefault(14);

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<OrderNotesGetDto>.SuccessResult(_mapper.Map<OrderNotesGetDto>(entity), "Order notes updated.");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteOrderNotesAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.OrderNotes.GetByIdAsync(id);
                if (existing == null || existing.IsDeleted)
                {
                    return ApiResponse<object>.ErrorResult("Order notes not found.", "Order notes not found.", StatusCodes.Status404NotFound);
                }

                await _unitOfWork.OrderNotes.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<object>.SuccessResult(null, "Order notes deleted.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
