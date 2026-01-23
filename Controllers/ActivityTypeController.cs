using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ActivityTypeController : ControllerBase
    {
        private readonly IActivityTypeService _activityTypeService;
        private readonly ILocalizationService _localizationService;

        public ActivityTypeController(IActivityTypeService activityTypeService, ILocalizationService localizationService)
        {
            _activityTypeService = activityTypeService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _activityTypeService.GetAllActivityTypesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _activityTypeService.GetActivityTypeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActivityTypeCreateDto createActivityTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<ActivityTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityTypeController.InvalidModelState"),
                    _localizationService.GetLocalizedString("ActivityTypeController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _activityTypeService.CreateActivityTypeAsync(createActivityTypeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ActivityTypeUpdateDto updateActivityTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<ActivityTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityTypeController.InvalidModelState"),
                    _localizationService.GetLocalizedString("ActivityTypeController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _activityTypeService.UpdateActivityTypeAsync(id, updateActivityTypeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _activityTypeService.DeleteActivityTypeAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
