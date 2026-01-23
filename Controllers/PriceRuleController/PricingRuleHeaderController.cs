using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PricingRuleHeaderController : ControllerBase
    {
        private readonly IPricingRuleHeaderService _pricingRuleHeaderService;

        public PricingRuleHeaderController(IPricingRuleHeaderService pricingRuleHeaderService)
        {
            _pricingRuleHeaderService = pricingRuleHeaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _pricingRuleHeaderService.GetAllPricingRuleHeadersAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _pricingRuleHeaderService.GetPricingRuleHeaderByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PricingRuleHeaderCreateDto createDto)
        {
            var result = await _pricingRuleHeaderService.CreatePricingRuleHeaderAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] PricingRuleHeaderUpdateDto updateDto)
        {
            var result = await _pricingRuleHeaderService.UpdatePricingRuleHeaderAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _pricingRuleHeaderService.DeletePricingRuleHeaderAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
