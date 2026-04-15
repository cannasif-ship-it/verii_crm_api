using crm_api.Modules.Definitions.Application.Dtos;
using crm_api.Modules.Definitions.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.Definitions.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesRepCodeUserMatchController : ControllerBase
    {
        private readonly ISalesRepCodeUserMatchService _salesRepCodeUserMatchService;

        public SalesRepCodeUserMatchController(ISalesRepCodeUserMatchService salesRepCodeUserMatchService)
        {
            _salesRepCodeUserMatchService = salesRepCodeUserMatchService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _salesRepCodeUserMatchService.GetAllSalesRepCodeUserMatchesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _salesRepCodeUserMatchService.GetSalesRepCodeUserMatchByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalesRepCodeUserMatchCreateDto dto)
        {
            var result = await _salesRepCodeUserMatchService.CreateSalesRepCodeUserMatchAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] SalesRepCodeUserMatchUpdateDto dto)
        {
            var result = await _salesRepCodeUserMatchService.UpdateSalesRepCodeUserMatchAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _salesRepCodeUserMatchService.DeleteSalesRepCodeUserMatchAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
