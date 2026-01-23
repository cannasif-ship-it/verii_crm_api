using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IProductPricingGroupByService
    {
        Task<ApiResponse<PagedResponse<ProductPricingGroupByDto>>> GetAllProductPricingGroupBysAsync(PagedRequest request);
        Task<ApiResponse<ProductPricingGroupByDto>> GetProductPricingGroupByByIdAsync(int id);
        Task<ApiResponse<ProductPricingGroupByDto>> CreateProductPricingGroupByAsync(CreateProductPricingGroupByDto createDto);
        Task<ApiResponse<ProductPricingGroupByDto>> UpdateProductPricingGroupByAsync(int id, UpdateProductPricingGroupByDto updateDto);
        Task<ApiResponse<object>> DeleteProductPricingGroupByAsync(int id);
    }
}
