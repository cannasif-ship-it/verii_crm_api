using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using crm_api.DTOs;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DemandLineController : ControllerBase
    {
        private readonly IDemandLineService _demandLineService;
        private readonly ILocalizationService _localizationService;

        public DemandLineController(IDemandLineService demandLineService, ILocalizationService localizationService)
        {
            _demandLineService = demandLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm talep satırlarını getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDemandLines([FromQuery] PagedRequest request)
        {
            var result = await _demandLineService.GetAllDemandLinesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID’ye göre talep satırını getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDemandLine(long id)
        {
            var result = await _demandLineService.GetDemandLineByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni talep satırı oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateDemandLine([FromBody] CreateDemandLineDto createDemandLineDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<DemandLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DemandLineController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _demandLineService.CreateDemandLineAsync(createDemandLineDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Birden fazla talep satırı oluşturur
        /// </summary>
        [HttpPost("create-multiple")]
        public async Task<IActionResult> CreateDemandLines([FromBody] List<CreateDemandLineDto> createDemandLineDtos)
        {
            var result = await _demandLineService.CreateDemandLinesAsync(createDemandLineDtos);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talep satırını günceller
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDemandLine(long id, [FromBody] UpdateDemandLineDto updateDemandLineDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<DemandLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DemandLineController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _demandLineService.UpdateDemandLineAsync(id, updateDemandLineDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talep satırını siler
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDemandLine(long id)
        {
            var result = await _demandLineService.DeleteDemandLineAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talebe göre talep satırlarını getirir
        /// </summary>
        [HttpGet("by-demand/{demandId}")]
        public async Task<IActionResult> GetDemandLinesByDemandId(long demandId)
        {
            var result = await _demandLineService.GetDemandLinesByDemandIdAsync(demandId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
