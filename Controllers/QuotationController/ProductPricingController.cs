using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductPricingController : ControllerBase
    {
        private readonly IProductPricingService _productPricingService;
        private readonly ILocalizationService _localizationService;

        public ProductPricingController(IProductPricingService productPricingService, ILocalizationService localizationService)
        {
            _productPricingService = productPricingService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _productPricingService.GetAllProductPricingsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _productPricingService.GetProductPricingByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductPricingCreateDto createProductPricingDto)
        {

            var result = await _productPricingService.CreateProductPricingAsync(createProductPricingDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ProductPricingUpdateDto updateProductPricingDto)
        {

            var result = await _productPricingService.UpdateProductPricingAsync(id, updateProductPricingDto);            
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _productPricingService.DeleteProductPricingAsync(id); 
            return StatusCode(result.StatusCode, result);
        }
    }
}