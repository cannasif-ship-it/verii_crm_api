using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.Catalog.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CatalogController : ControllerBase
    {
        private readonly IProductCatalogService _productCatalogService;

        public CatalogController(IProductCatalogService productCatalogService)
        {
            _productCatalogService = productCatalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogs()
        {
            var result = await _productCatalogService.GetCatalogsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("category-image/upload")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UploadCategoryImage([FromForm] IFormFile file)
        {
            var result = await _productCatalogService.UploadCategoryImageAsync(file);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCatalogById(long id)
        {
            var result = await _productCatalogService.GetCatalogByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCatalog([FromBody] ProductCatalogCreateDto request)
        {
            var result = await _productCatalogService.CreateCatalogAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCatalog(long id, [FromBody] ProductCatalogUpdateDto request)
        {
            var result = await _productCatalogService.UpdateCatalogAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCatalog(long id)
        {
            var result = await _productCatalogService.DeleteCatalogAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{catalogId:long}/categories")]
        public async Task<IActionResult> GetCatalogCategories(long catalogId, [FromQuery] long? parentCatalogCategoryId)
        {
            var result = await _productCatalogService.GetCatalogCategoriesAsync(catalogId, parentCatalogCategoryId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{catalogId:long}/categories")]
        public async Task<IActionResult> CreateCatalogCategory(long catalogId, [FromBody] CatalogCategoryCreateDto request)
        {
            var result = await _productCatalogService.CreateCatalogCategoryAsync(catalogId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{catalogId:long}/categories/{catalogCategoryId:long}")]
        public async Task<IActionResult> UpdateCatalogCategory(long catalogId, long catalogCategoryId, [FromBody] CatalogCategoryUpdateDto request)
        {
            var result = await _productCatalogService.UpdateCatalogCategoryAsync(catalogId, catalogCategoryId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{catalogId:long}/categories/reorder")]
        public async Task<IActionResult> ReorderCatalogCategories(long catalogId, [FromBody] CatalogCategoryReorderDto request)
        {
            var result = await _productCatalogService.ReorderCatalogCategoriesAsync(catalogId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{catalogId:long}/categories/{catalogCategoryId:long}")]
        public async Task<IActionResult> DeleteCatalogCategory(long catalogId, long catalogCategoryId)
        {
            var result = await _productCatalogService.DeleteCatalogCategoryAsync(catalogId, catalogCategoryId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{catalogId:long}/categories/{catalogCategoryId:long}/rules")]
        public async Task<IActionResult> GetCategoryRules(long catalogId, long catalogCategoryId)
        {
            var result = await _productCatalogService.GetCategoryRulesAsync(catalogId, catalogCategoryId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{catalogId:long}/categories/{catalogCategoryId:long}/rule-value-options")]
        public async Task<IActionResult> GetCategoryRuleValueOptions(long catalogId, long catalogCategoryId, [FromQuery] StockAttributeType stockAttributeType, [FromQuery] string? search)
        {
            var result = await _productCatalogService.GetCategoryRuleValueOptionsAsync(catalogId, catalogCategoryId, stockAttributeType, search);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{catalogId:long}/categories/{catalogCategoryId:long}/rules")]
        public async Task<IActionResult> CreateCategoryRule(long catalogId, long catalogCategoryId, [FromBody] ProductCategoryRuleCreateDto request)
        {
            var result = await _productCatalogService.CreateCategoryRuleAsync(catalogId, catalogCategoryId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{catalogId:long}/categories/{catalogCategoryId:long}/rules/{ruleId:long}")]
        public async Task<IActionResult> UpdateCategoryRule(long catalogId, long catalogCategoryId, long ruleId, [FromBody] ProductCategoryRuleUpdateDto request)
        {
            var result = await _productCatalogService.UpdateCategoryRuleAsync(catalogId, catalogCategoryId, ruleId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{catalogId:long}/categories/{catalogCategoryId:long}/rules/{ruleId:long}")]
        public async Task<IActionResult> DeleteCategoryRule(long catalogId, long catalogCategoryId, long ruleId)
        {
            var result = await _productCatalogService.DeleteCategoryRuleAsync(catalogId, catalogCategoryId, ruleId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{catalogId:long}/categories/{catalogCategoryId:long}/rules/preview")]
        public async Task<IActionResult> PreviewCategoryRules(long catalogId, long catalogCategoryId)
        {
            var result = await _productCatalogService.PreviewCategoryRulesAsync(catalogId, catalogCategoryId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{catalogId:long}/categories/{catalogCategoryId:long}/rules/apply")]
        public async Task<IActionResult> ApplyCategoryRules(long catalogId, long catalogCategoryId)
        {
            var result = await _productCatalogService.ApplyCategoryRulesAsync(catalogId, catalogCategoryId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{catalogId:long}/categories/{catalogCategoryId:long}/stocks")]
        public async Task<IActionResult> CreateStockCategoryAssignment(long catalogId, long catalogCategoryId, [FromBody] StockCategoryCreateDto request)
        {
            var result = await _productCatalogService.CreateStockCategoryAssignmentAsync(catalogId, catalogCategoryId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{catalogId:long}/categories/{catalogCategoryId:long}/stocks/{stockCategoryId:long}")]
        public async Task<IActionResult> DeleteStockCategoryAssignment(long catalogId, long catalogCategoryId, long stockCategoryId)
        {
            var result = await _productCatalogService.DeleteStockCategoryAssignmentAsync(catalogId, catalogCategoryId, stockCategoryId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{catalogId:long}/categories/{catalogCategoryId:long}/stocks")]
        public async Task<IActionResult> GetCatalogCategoryStocks(long catalogId, long catalogCategoryId, [FromQuery] bool includeDescendants = false, [FromQuery] PagedRequest request = null!)
        {
            var result = await _productCatalogService.GetCatalogCategoryStocksAsync(catalogId, catalogCategoryId, includeDescendants, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
