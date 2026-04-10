using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.PdfBuilder.Api
{
    [Authorize]
    [ApiController]
    [Route("api/pdf-table-presets")]
    public class PdfTablePresetController : ControllerBase
    {
        private readonly IPdfTablePresetService _pdfTablePresetService;
        private readonly ILocalizationService _localizationService;

        public PdfTablePresetController(
            IPdfTablePresetService pdfTablePresetService,
            ILocalizationService localizationService)
        {
            _pdfTablePresetService = pdfTablePresetService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] DocumentRuleType? ruleType = null,
            [FromQuery] bool? isActive = null)
        {
            var result = await _pdfTablePresetService.GetAllAsync(new PdfTablePresetListRequest
            {
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize,
                RuleType = ruleType,
                IsActive = isActive,
            });
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _pdfTablePresetService.GetByIdAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePdfTablePresetDto dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) || userId <= 0)
                return Unauthorized(ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ReportTemplateService.UnauthorizedGenerate"),
                    "Invalid or missing user claim",
                    401));

            var result = await _pdfTablePresetService.CreateAsync(dto, userId);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePdfTablePresetDto dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) || userId <= 0)
                return Unauthorized(ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ReportTemplateService.UnauthorizedGenerate"),
                    "Invalid or missing user claim",
                    401));

            var result = await _pdfTablePresetService.UpdateAsync(id, dto, userId);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _pdfTablePresetService.DeleteAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }
    }
}
