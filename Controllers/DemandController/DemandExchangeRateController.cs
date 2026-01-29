using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DemandExchangeRateController : ControllerBase
    {
        private readonly IDemandExchangeRateService _demandExchangeRateService;

        public DemandExchangeRateController(IDemandExchangeRateService demandExchangeRateService)
        {
            _demandExchangeRateService = demandExchangeRateService;
        }

        /// <summary>
        /// Tüm talep döviz kurlarını getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _demandExchangeRateService.GetAllDemandExchangeRatesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre talep döviz kurunu getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _demandExchangeRateService.GetDemandExchangeRateByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talebe göre talep döviz kurlarını getirir
        /// </summary>
        [HttpGet("demand/{demandId}")]
        public async Task<IActionResult> GetByDemandId(long demandId)
        {
            var result = await _demandExchangeRateService.GetDemandExchangeRatesByDemandIdAsync(demandId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni talep döviz kuru oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DemandExchangeRateCreateDto createDto)
        {
            var result = await _demandExchangeRateService.CreateDemandExchangeRateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talep döviz kurunu günceller
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] DemandExchangeRateUpdateDto updateDto)
        {
            var result = await _demandExchangeRateService.UpdateDemandExchangeRateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talep içindeki döviz kurlarını toplu günceller
        /// </summary>
        [HttpPut("update-exchange-rate-in-demand")]
        public async Task<IActionResult> UpdateExchangeRateInDemand([FromBody] List<DemandExchangeRateGetDto> updateDtos)
        {
            var result = await _demandExchangeRateService.UpdateExchangeRateInDemand(updateDtos);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talep döviz kurunu siler
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _demandExchangeRateService.DeleteDemandExchangeRateAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
