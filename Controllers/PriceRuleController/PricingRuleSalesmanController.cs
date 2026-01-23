using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PricingRuleSalesmanController : ControllerBase
    {
        private readonly IPricingRuleSalesmanService _pricingRuleSalesmanService;

        public PricingRuleSalesmanController(IPricingRuleSalesmanService pricingRuleSalesmanService)
        {
            _pricingRuleSalesmanService = pricingRuleSalesmanService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _pricingRuleSalesmanService.GetAllPricingRuleSalesmenAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _pricingRuleSalesmanService.GetPricingRuleSalesmanByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<IActionResult> GetByHeaderId(long headerId)
        {
            var result = await _pricingRuleSalesmanService.GetPricingRuleSalesmenByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PricingRuleSalesmanCreateDto createDto)
        {
            var result = await _pricingRuleSalesmanService.CreatePricingRuleSalesmanAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] PricingRuleSalesmanUpdateDto updateDto)
        {
            var result = await _pricingRuleSalesmanService.UpdatePricingRuleSalesmanAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _pricingRuleSalesmanService.DeletePricingRuleSalesmanAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
