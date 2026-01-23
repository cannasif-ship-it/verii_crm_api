using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;

        public CountryController(ICountryService countryService, ILocalizationService localizationService)
        {
            _countryService = countryService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<CountryGetDto>>>> GetAllCountries([FromQuery] PagedRequest request)
        {
            var result = await _countryService.GetAllCountriesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CountryGetDto>>> GetCountryById(long id)
        {
            var result = await _countryService.GetCountryByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CountryGetDto>>> CreateCountry([FromBody] CountryCreateDto countryCreateDto)
        {
            var result = await _countryService.CreateCountryAsync(countryCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CountryGetDto>>> UpdateCountry(long id, [FromBody] CountryUpdateDto countryUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<CountryGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CountryController.InvalidModelState"),
                    _localizationService.GetLocalizedString("CountryController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _countryService.UpdateCountryAsync(id, countryUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCountry(long id)
        {
            var result = await _countryService.DeleteCountryAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
