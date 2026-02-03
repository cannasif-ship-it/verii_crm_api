using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs.ReportBuilderDto;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IReportPreviewService _previewService;

        public ReportsController(IReportService reportService, IReportPreviewService previewService)
        {
            _reportService = reportService;
            _previewService = previewService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReportCreateDto dto)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !long.TryParse(userIdStr, out var userId))
                return Unauthorized();
            var result = await _reportService.CreateAsync(dto, userId);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? search = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _reportService.ListAsync(search, pageNumber, pageSize);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _reportService.GetByIdAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ReportUpdateDto dto)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !long.TryParse(userIdStr, out var userId))
                return Unauthorized();
            var result = await _reportService.UpdateAsync(id, dto, userId);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _reportService.SoftDeleteAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpPost("preview")]
        public async Task<IActionResult> Preview([FromBody] PreviewRequestDto request)
        {
            var result = await _previewService.PreviewAsync(request);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result.Data);
        }
    }
}
