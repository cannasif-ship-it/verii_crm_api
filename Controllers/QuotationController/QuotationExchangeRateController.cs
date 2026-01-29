using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuotationExchangeRateController : ControllerBase
    {
        private readonly IQuotationExchangeRateService _quotationExchangeRateService;

        public QuotationExchangeRateController(IQuotationExchangeRateService quotationExchangeRateService)
        {
            _quotationExchangeRateService = quotationExchangeRateService;
        }

        /// <summary>
        /// Tüm teklif döviz kurlarını getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _quotationExchangeRateService.GetAllQuotationExchangeRatesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre teklif döviz kurunu getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _quotationExchangeRateService.GetQuotationExchangeRateByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklife göre teklif döviz kurlarını getirir
        /// </summary>
        [HttpGet("quotation/{quotationId}")]
        public async Task<IActionResult> GetByQuotationId(long quotationId)
        {
            var result = await _quotationExchangeRateService.GetQuotationExchangeRatesByQuotationIdAsync(quotationId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni teklif döviz kuru oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuotationExchangeRateCreateDto createDto)
        {
            var result = await _quotationExchangeRateService.CreateQuotationExchangeRateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklif döviz kurunu günceller
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] QuotationExchangeRateUpdateDto updateDto)
        {
            var result = await _quotationExchangeRateService.UpdateQuotationExchangeRateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklif içindeki döviz kurlarını toplu günceller
        /// </summary>
        [HttpPut("update-exchange-rate-in-quotation")]
        public async Task<IActionResult> UpdateExchangeRateInQuotation([FromBody] List<QuotationExchangeRateGetDto> updateDtos)
        {
            var result = await _quotationExchangeRateService.UpdateExchangeRateInQuotation(updateDtos);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklif döviz kurunu siler
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _quotationExchangeRateService.DeleteQuotationExchangeRateAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
