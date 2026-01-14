using cms_webapi.DTOs;
using cms_webapi.Data;

namespace cms_webapi.Interfaces
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
