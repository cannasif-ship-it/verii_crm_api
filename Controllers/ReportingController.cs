using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs.ReportBuilderDto;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reporting")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingConnectionService _connectionService;
        private readonly IReportingCatalogService _catalogService;

        public ReportingController(IReportingConnectionService connectionService, IReportingCatalogService catalogService)
        {
            _connectionService = connectionService;
            _catalogService = catalogService;
        }

        [HttpGet("connections")]
        public IActionResult GetConnections()
        {
            var result = _connectionService.GetConnections();
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result.Data);
        }

        [HttpPost("datasources/check")]
        public async Task<IActionResult> CheckDataSource([FromBody] DataSourceCheckRequestDto request)
        {
            var result = await _catalogService.CheckAndGetSchemaAsync(request.ConnectionKey, request.Type, request.Name);
            var response = new DataSourceCheckResponseDto
            {
                Exists = result.Success && result.Data != null && result.Data.Count > 0,
                Message = result.Success ? (result.Data?.Count > 0 ? "OK" : "Object not found or has no columns.") : (result.Message ?? "Error"),
                Schema = result.Data ?? new List<FieldSchemaDto>()
            };
            return Ok(response);
        }
    }
}
