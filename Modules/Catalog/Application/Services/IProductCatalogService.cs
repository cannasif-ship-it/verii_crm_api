namespace crm_api.Modules.Catalog.Application.Services
{
    public interface IProductCatalogService
    {
        Task<ApiResponse<List<ProductCatalogGetDto>>> GetCatalogsAsync();
        Task<ApiResponse<string>> UploadCategoryImageAsync(IFormFile file);
        Task<ApiResponse<ProductCatalogGetDto>> GetCatalogByIdAsync(long id);
        Task<ApiResponse<ProductCatalogGetDto>> CreateCatalogAsync(ProductCatalogCreateDto request);
        Task<ApiResponse<ProductCatalogGetDto>> UpdateCatalogAsync(long id, ProductCatalogUpdateDto request);
        Task<ApiResponse<object>> DeleteCatalogAsync(long id);
        Task<ApiResponse<List<CatalogCategoryNodeDto>>> GetCatalogCategoriesAsync(long catalogId, long? parentCatalogCategoryId);
        Task<ApiResponse<CatalogCategoryNodeDto>> CreateCatalogCategoryAsync(long catalogId, CatalogCategoryCreateDto request);
        Task<ApiResponse<CatalogCategoryNodeDto>> UpdateCatalogCategoryAsync(long catalogId, long catalogCategoryId, CatalogCategoryUpdateDto request);
        Task<ApiResponse<object>> ReorderCatalogCategoriesAsync(long catalogId, CatalogCategoryReorderDto request);
        Task<ApiResponse<object>> DeleteCatalogCategoryAsync(long catalogId, long catalogCategoryId);
        Task<ApiResponse<List<ProductCategoryRuleGetDto>>> GetCategoryRulesAsync(long catalogId, long catalogCategoryId);
        Task<ApiResponse<List<CategoryRuleValueOptionDto>>> GetCategoryRuleValueOptionsAsync(long catalogId, long catalogCategoryId, StockAttributeType stockAttributeType, string? search);
        Task<ApiResponse<ProductCategoryRuleGetDto>> CreateCategoryRuleAsync(long catalogId, long catalogCategoryId, ProductCategoryRuleCreateDto request);
        Task<ApiResponse<ProductCategoryRuleGetDto>> UpdateCategoryRuleAsync(long catalogId, long catalogCategoryId, long ruleId, ProductCategoryRuleUpdateDto request);
        Task<ApiResponse<object>> DeleteCategoryRuleAsync(long catalogId, long catalogCategoryId, long ruleId);
        Task<ApiResponse<CategoryRulePreviewResultDto>> PreviewCategoryRulesAsync(long catalogId, long catalogCategoryId);
        Task<ApiResponse<CategoryRuleApplyResultDto>> ApplyCategoryRulesAsync(long catalogId, long catalogCategoryId);
        Task<ApiResponse<CatalogStockItemDto>> CreateStockCategoryAssignmentAsync(long catalogId, long catalogCategoryId, StockCategoryCreateDto request);
        Task<ApiResponse<object>> DeleteStockCategoryAssignmentAsync(long catalogId, long catalogCategoryId, long stockCategoryId);
        Task<ApiResponse<PagedResponse<CatalogStockItemDto>>> GetCatalogCategoryStocksAsync(long catalogId, long catalogCategoryId, PagedRequest request);
    }
}
