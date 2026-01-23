using crm_api.DTOs;
using crm_api.Data;

namespace crm_api.Interfaces
{
    public interface IProductPricingService
    {
        Task<ApiResponse<PagedResponse<ProductPricingGetDto>>> GetAllProductPricingsAsync(PagedRequest request);
        Task<ApiResponse<ProductPricingGetDto>> GetProductPricingByIdAsync(long id);
        Task<ApiResponse<ProductPricingGetDto>> CreateProductPricingAsync(ProductPricingCreateDto createProductPricingDto);
        Task<ApiResponse<ProductPricingGetDto>> UpdateProductPricingAsync(long id, ProductPricingUpdateDto updateProductPricingDto);
        Task<ApiResponse<object>> DeleteProductPricingAsync(long id);
    }
}
