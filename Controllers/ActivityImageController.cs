using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ActivityImageController : ControllerBase
    {
        private readonly IActivityImageService _activityImageService;
        private readonly ILocalizationService _localizationService;

        public ActivityImageController(IActivityImageService activityImageService, ILocalizationService localizationService)
        {
            _activityImageService = activityImageService;
            _localizationService = localizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<CreateActivityImageDto> request)
        {
            var result = await _activityImageService.AddImagesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("upload/{activityId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImages(
            [FromRoute] long activityId,
            List<IFormFile> files,
            [FromForm] List<string>? resimAciklamalar = null)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("FileUploadService.FileRequired"),
                    "No files provided",
                    400));
            }

            var result = await _activityImageService.UploadImagesAsync(activityId, files, resimAciklamalar);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-activity/{activityId}")]
        public async Task<IActionResult> GetByActivityId(long activityId)
        {
            var result = await _activityImageService.GetByActivityIdAsync(activityId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateActivityImageDto request)
        {
            var result = await _activityImageService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _activityImageService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
