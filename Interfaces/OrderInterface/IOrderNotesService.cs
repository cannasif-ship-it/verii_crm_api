using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IOrderNotesService
    {
        Task<ApiResponse<PagedResponse<OrderNotesGetDto>>> GetAllOrderNotesAsync(PagedRequest request);
        Task<ApiResponse<OrderNotesGetDto>> GetOrderNotesByIdAsync(long id);
        Task<ApiResponse<OrderNotesGetDto>> GetNotesByOrderIdAsync(long orderId);
        Task<ApiResponse<OrderNotesDto>> CreateOrderNotesAsync(CreateOrderNotesDto createOrderNotesDto);
        Task<ApiResponse<OrderNotesDto>> UpdateOrderNotesAsync(long id, UpdateOrderNotesDto updateOrderNotesDto);
        Task<ApiResponse<OrderNotesGetDto>> UpdateNotesListByOrderIdAsync(long orderId, UpdateOrderNotesListDto request);
        Task<ApiResponse<object>> DeleteOrderNotesAsync(long id);
    }
}
