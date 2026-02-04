using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs.PowerBi;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PowerBIReportDefinitionController : ControllerBase
    {
        private readonly IPowerBIReportDefinitionService _service;
        private readonly ILocalizationService _localizationService;

        public PowerBIReportDefinitionController(
            IPowerBIReportDefinitionService service,
            ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var result = await _service.GetAllAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePowerBIReportDefinitionDto dto)
        {

            var result = await _service.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePowerBIReportDefinitionDto dto)
        {

            var result = await _service.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}