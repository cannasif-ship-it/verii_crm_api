using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PricingRuleLineController : ControllerBase
    {
        private readonly IPricingRuleLineService _pricingRuleLineService;

        public PricingRuleLineController(IPricingRuleLineService pricingRuleLineService)
        {
            _pricingRuleLineService = pricingRuleLineService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _pricingRuleLineService.GetAllPricingRuleLinesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _pricingRuleLineService.GetPricingRuleLineByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<IActionResult> GetByHeaderId(long headerId)
        {
            var result = await _pricingRuleLineService.GetPricingRuleLinesByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PricingRuleLineCreateDto createDto)
        {
            var result = await _pricingRuleLineService.CreatePricingRuleLineAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] PricingRuleLineUpdateDto updateDto)
        {
            var result = await _pricingRuleLineService.UpdatePricingRuleLineAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _pricingRuleLineService.DeletePricingRuleLineAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
