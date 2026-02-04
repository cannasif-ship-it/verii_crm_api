using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
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

            var result = await _activityService.CreateActivityAsync(createActivityDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateActivityDto updateActivityDto)
        {

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