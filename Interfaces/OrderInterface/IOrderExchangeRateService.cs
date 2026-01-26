using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IOrderExchangeRateService
    {
        Task<ApiResponse<PagedResponse<OrderExchangeRateGetDto>>> GetAllOrderExchangeRatesAsync(PagedRequest request);
        Task<ApiResponse<OrderExchangeRateGetDto>> GetOrderExchangeRateByIdAsync(long id);
        Task<ApiResponse<OrderExchangeRateGetDto>> CreateOrderExchangeRateAsync(OrderExchangeRateCreateDto createDto);
        Task<ApiResponse<OrderExchangeRateGetDto>> UpdateOrderExchangeRateAsync(long id, OrderExchangeRateUpdateDto updateDto);
        Task<ApiResponse<object>> DeleteOrderExchangeRateAsync(long id);
        Task<ApiResponse<List<OrderExchangeRateGetDto>>> GetOrderExchangeRatesByOrderIdAsync(long orderId);
    }
}
