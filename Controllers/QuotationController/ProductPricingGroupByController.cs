using Microsoft.AspNetCore.Mvc;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductPricingGroupByController : ControllerBase
    {
        private readonly IProductPricingGroupByService _productPricingGroupByService;
        private readonly ILocalizationService _localizationService;

        public ProductPricingGroupByController(IProductPricingGroupByService productPricingGroupByService, ILocalizationService localizationService)
        {
            _productPricingGroupByService = productPricingGroupByService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<ProductPricingGroupByDto>>>> GetAllProductPricingGroupBys([FromQuery] PagedRequest request)
        {
            var result = await _productPricingGroupByService.GetAllProductPricingGroupBysAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductPricingGroupByDto>>> GetProductPricingGroupBy(int id)
        {
            var result = await _productPricingGroupByService.GetProductPricingGroupByByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductPricingGroupByDto>>> CreateProductPricingGroupBy(CreateProductPricingGroupByDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), "ValidationFailed", 400));
            }

            var result = await _productPricingGroupByService.CreateProductPricingGroupByAsync(createDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetProductPricingGroupBy), new { id = result.Data.Id }, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductPricingGroupByDto>>> UpdateProductPricingGroupBy(int id, UpdateProductPricingGroupByDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), "ValidationFailed", 400));
            }

            var result = await _productPricingGroupByService.UpdateProductPricingGroupByAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProductPricingGroupBy(int id)
        {
            var result = await _productPricingGroupByService.DeleteProductPricingGroupByAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
