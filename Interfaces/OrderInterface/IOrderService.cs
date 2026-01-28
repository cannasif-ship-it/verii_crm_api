using crm_api.DTOs;
using crm_api.Models;

namespace crm_api.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<PagedResponse<OrderGetDto>>> GetAllOrdersAsync(PagedRequest request);
        Task<ApiResponse<OrderGetDto>> GetOrderByIdAsync(long id);
        Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<ApiResponse<OrderDto>> UpdateOrderAsync(long id, UpdateOrderDto updateOrderDto);
        Task<ApiResponse<object>> DeleteOrderAsync(long id);
        Task<ApiResponse<List<OrderGetDto>>> GetOrdersByPotentialCustomerIdAsync(long potentialCustomerId);
        Task<ApiResponse<List<OrderGetDto>>> GetOrdersByRepresentativeIdAsync(long representativeId);
        Task<ApiResponse<List<OrderGetDto>>> GetOrdersByStatusAsync(int status);
        Task<ApiResponse<bool>> OrderExistsAsync(long id);
        Task<ApiResponse<OrderGetDto>> CreateOrderBulkAsync(OrderBulkCreateDto bulkDto);
        Task<ApiResponse<OrderGetDto>> CreateRevisionOfOrderAsync(long orderId);
        Task<ApiResponse<List<PricingRuleLineGetDto>>> GetPriceRuleOfOrderAsync(string customerCode,long salesmenId,DateTime orderDate);
        Task<ApiResponse<List<PriceOfProductDto>>> GetPriceOfProductAsync(List<PriceOfProductRequestDto> request);
        Task<ApiResponse<bool>> StartApprovalFlowAsync(StartApprovalFlowDto request);
        Task<ApiResponse<List<ApprovalActionGetDto>>> GetWaitingApprovalsAsync();
        Task<ApiResponse<bool>> ApproveAsync(ApproveActionDto request);
        Task<ApiResponse<bool>> RejectAsync(RejectActionDto request);
        Task<ApiResponse<PagedResponse<OrderGetDto>>> GetRelatedOrders(PagedRequest request);
        Task<ApiResponse<List<ApprovalScopeUserDto>>> GetOrderRelatedUsersAsync(long userId);
    }
}
