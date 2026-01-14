using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;
        private readonly ILocalizationService _localizationService;

        public DistrictController(IDistrictService districtService, ILocalizationService localizationService)
        {
            _districtService = districtService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _districtService.GetAllDistrictsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _districtService.GetDistrictByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DistrictCreateDto districtCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<DistrictGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DistrictController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DistrictController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _districtService.CreateDistrictAsync(districtCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] DistrictUpdateDto districtUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<DistrictGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DistrictController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DistrictController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _districtService.UpdateDistrictAsync(id, districtUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _districtService.DeleteDistrictAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
