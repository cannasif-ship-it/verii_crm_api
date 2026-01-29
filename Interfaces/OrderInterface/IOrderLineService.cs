using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IOrderLineService
    {
        Task<ApiResponse<PagedResponse<OrderLineGetDto>>> GetAllOrderLinesAsync(PagedRequest request);
        Task<ApiResponse<OrderLineGetDto>> GetOrderLineByIdAsync(long id);
        Task<ApiResponse<OrderLineDto>> CreateOrderLineAsync(CreateOrderLineDto createOrderLineDto);
        Task<ApiResponse<List<OrderLineDto>>> CreateOrderLinesAsync(List<CreateOrderLineDto> createOrderLineDtos);
        Task<ApiResponse<OrderLineDto>> UpdateOrderLineAsync(long id, UpdateOrderLineDto updateOrderLineDto);
        Task<ApiResponse<object>> DeleteOrderLineAsync(long id);
        Task<ApiResponse<List<OrderLineGetDto>>> GetOrderLinesByOrderIdAsync(long orderId);
    }
}
