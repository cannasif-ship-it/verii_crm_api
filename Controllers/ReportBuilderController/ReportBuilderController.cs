using System.Security.Claims;
using crm_api.DTOs.ReportBuilderDto;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportBuilderController : ControllerBase
    {
        private readonly IReportingConnectionService _connectionService;
        private readonly IReportingCatalogService _catalogService;
        private readonly IReportService _reportService;
        private readonly IReportPreviewService _previewService;
        private readonly ILocalizationService _localizationService;

        public ReportBuilderController(
            IReportingConnectionService connectionService,
            IReportingCatalogService catalogService,
            IReportService reportService,
            IReportPreviewService previewService,
            ILocalizationService localizationService)
        {
            _connectionService = connectionService;
            _catalogService = catalogService;
            _reportService = reportService;
            _previewService = previewService;
            _localizationService = localizationService;
        }

        private bool TryGetCurrentUser(out long userId, out string? email)
        {
            userId = 0;
            email = User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value;

            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("UserId")?.Value;

            return !string.IsNullOrEmpty(userIdStr) && long.TryParse(userIdStr, out userId);
        }

        // GET /api/reportbuilder/connections
        [HttpGet("connections")]
        public IActionResult GetConnections()
        {
            var result = _connectionService.GetConnections();
            return StatusCode(result.StatusCode, result);
        }

        // POST /api/reportbuilder/datasources/check
        // Response: { exists, message, schema } (DEĞİŞMEDİ)
        [HttpPost("datasources/check")]
        public async Task<IActionResult> CheckDataSource([FromBody] DataSourceCheckRequestDto request)
        {
            var result = await _catalogService.CheckAndGetSchemaAsync(request.ConnectionKey, request.Type, request.Name);

            var response = new DataSourceCheckResponseDto
            {
                Exists = result.Success && result.Data != null && result.Data.Count > 0,
                Message = result.Success
                    ? (result.Data?.Count > 0 ? _localizationService.GetLocalizedString("ReportBuilderController.Ok") : _localizationService.GetLocalizedString("ReportBuilderController.ObjectNotFoundOrNoColumns"))
                    : (result.Message ?? _localizationService.GetLocalizedString("ReportBuilderController.Error")),
                Schema = result.Data ?? new List<FieldSchemaDto>()
            };

            return Ok(response);
        }

        // GET /api/reportbuilder/datasources?connectionKey=...&type=view&search=...
        [HttpGet("datasources")]
        public async Task<IActionResult> ListDataSources([FromQuery] string connectionKey, [FromQuery] string type, [FromQuery] string? search = null)
        {
            var result = await _catalogService.ListDataSourcesAsync(connectionKey, type, search);
            return StatusCode(result.StatusCode, result);
        }

        // POST /api/reportbuilder   (Create)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReportCreateDto dto)
        {
            if (!TryGetCurrentUser(out var userId, out _))
                return Unauthorized();

            var result = await _reportService.CreateAsync(dto, userId);
            return StatusCode(result.StatusCode, result);
        }

        // GET /api/reportbuilder?search=&pageNumber=1&pageSize=20   (List)
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? search = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (!TryGetCurrentUser(out var userId, out var email))
                return Unauthorized();

            var result = await _reportService.ListAsync(search, userId, email, pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        // GET /api/reportbuilder/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            if (!TryGetCurrentUser(out var userId, out var email))
                return Unauthorized();

            var result = await _reportService.GetByIdAsync(id, userId, email);
            return StatusCode(result.StatusCode, result);
        }

        // PUT /api/reportbuilder/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] ReportUpdateDto dto)
        {
            if (!TryGetCurrentUser(out var userId, out var email))
                return Unauthorized();

            var result = await _reportService.UpdateAsync(id, dto, userId, email);
            return StatusCode(result.StatusCode, result);
        }

        // DELETE /api/reportbuilder/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!TryGetCurrentUser(out var userId, out var email))
                return Unauthorized();

            var result = await _reportService.SoftDeleteAsync(id, userId, email);
            return StatusCode(result.StatusCode, result);
        }

        // POST /api/reportbuilder/preview
        [HttpPost("preview")]
        public async Task<IActionResult> Preview([FromBody] PreviewRequestDto request)
        {
            var result = await _previewService.PreviewAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
