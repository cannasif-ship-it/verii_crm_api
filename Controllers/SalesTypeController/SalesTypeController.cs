using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesTypeController : ControllerBase
    {
        private readonly ISalesTypeService _salesTypeService;

        public SalesTypeController(ISalesTypeService salesTypeService)
        {
            _salesTypeService = salesTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _salesTypeService.GetAllSalesTypesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _salesTypeService.GetSalesTypeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalesTypeCreateDto dto)
        {
            var result = await _salesTypeService.CreateSalesTypeAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] SalesTypeUpdateDto dto)
        {
            var result = await _salesTypeService.UpdateSalesTypeAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _salesTypeService.DeleteSalesTypeAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
