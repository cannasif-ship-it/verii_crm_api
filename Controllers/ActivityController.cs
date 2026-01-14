using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly ILocalizationService _localizationService;

        public ActivityController(IActivityService activityService, ILocalizationService localizationService)
        {
            _activityService = activityService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _activityService.GetAllActivitiesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _activityService.GetActivityByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateActivityDto createActivityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), "ValidationFailed", 400));
            }

            var result = await _activityService.CreateActivityAsync(createActivityDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateActivityDto updateActivityDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400,ApiResponse<object>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), ModelState?.ToString() ?? string.Empty));
            }

            var result = await _activityService.UpdateActivityAsync(id, updateActivityDto);            
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _activityService.DeleteActivityAsync(id); 
            return StatusCode(result.StatusCode, result);
        }
    }
}
