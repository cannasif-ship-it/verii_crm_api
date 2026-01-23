using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly ILocalizationService _localizationService;

        public CityController(ICityService cityService, ILocalizationService localizationService)
        {
            _cityService = cityService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<CityGetDto>>>> GetAllCities([FromQuery] PagedRequest request)
        {
            var result = await _cityService.GetAllCitiesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CityGetDto>>> GetCityById(long id)
        {
            var result = await _cityService.GetCityByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CityGetDto>>> CreateCity([FromBody] CityCreateDto cityCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<CityGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CityController.InvalidModelState"),
                    _localizationService.GetLocalizedString("CityController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _cityService.CreateCityAsync(cityCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CityGetDto>>> UpdateCity(long id, [FromBody] CityUpdateDto cityUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<CityGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CityController.InvalidModelState"),
                    _localizationService.GetLocalizedString("CityController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _cityService.UpdateCityAsync(id, cityUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCity(long id)
        {
            var result = await _cityService.DeleteCityAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
