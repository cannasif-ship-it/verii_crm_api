using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using crm_api.DTOs;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuotationLineController : ControllerBase
    {
        private readonly IQuotationLineService _quotationLineService;
        private readonly ILocalizationService _localizationService;

        public QuotationLineController(IQuotationLineService quotationLineService, ILocalizationService localizationService)
        {
            _quotationLineService = quotationLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm teklif satırlarını getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetQuotationLines([FromQuery] PagedRequest request)
        {
            var result = await _quotationLineService.GetAllQuotationLinesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID’ye göre teklif satırını getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuotationLine(long id)
        {
            var result = await _quotationLineService.GetQuotationLineByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni teklif satırı oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateQuotationLine([FromBody] CreateQuotationLineDto createQuotationLineDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<QuotationLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineController.InvalidModelState"),
                    _localizationService.GetLocalizedString("QuotationLineController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _quotationLineService.CreateQuotationLineAsync(createQuotationLineDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("create-multiple")]
        public async Task<IActionResult> CreateQuotationLines([FromBody] List<CreateQuotationLineDto> createQuotationLineDtos)
        {
            var result = await _quotationLineService.CreateQuotationLinesAsync(createQuotationLineDtos);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklif satırını günceller
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuotationLine(long id, [FromBody] UpdateQuotationLineDto updateQuotationLineDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<QuotationLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineController.InvalidModelState"),
                    _localizationService.GetLocalizedString("QuotationLineController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _quotationLineService.UpdateQuotationLineAsync(id, updateQuotationLineDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklif satırını siler
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotationLine(long id)
        {
            var result = await _quotationLineService.DeleteQuotationLineAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklife göre teklif satırlarını getirir
        /// </summary>
        [HttpGet("by-quotation/{quotationId}")]
        public async Task<IActionResult> GetQuotationLinesByQuotationId(long quotationId)
        {
            var result = await _quotationLineService.GetQuotationLinesByQuotationIdAsync(quotationId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
