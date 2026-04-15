using crm_api.Modules.Definitions.Application.Dtos;
using crm_api.Modules.Definitions.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.Definitions.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesRepCodeController : ControllerBase
    {
        private readonly ISalesRepCodeService _salesRepCodeService;

        public SalesRepCodeController(ISalesRepCodeService salesRepCodeService)
        {
            _salesRepCodeService = salesRepCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _salesRepCodeService.GetAllSalesRepCodesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _salesRepCodeService.GetSalesRepCodeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalesRepCodeCreateDto dto)
        {
            var result = await _salesRepCodeService.CreateSalesRepCodeAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] SalesRepCodeUpdateDto dto)
        {
            var result = await _salesRepCodeService.UpdateSalesRepCodeAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _salesRepCodeService.DeleteSalesRepCodeAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync()
        {
            var result = await _salesRepCodeService.SyncSalesRepCodesFromErpAsync();
            return StatusCode(result.StatusCode, result);
        }
    }
}
