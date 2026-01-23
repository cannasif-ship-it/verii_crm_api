using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TitleController : ControllerBase
    {
        private readonly ITitleService _titleService;
        private readonly ILocalizationService _localizationService;

        public TitleController(ITitleService titleService, ILocalizationService localizationService)
        {
            _titleService = titleService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            if (request == null)
            {
                request = new PagedRequest();
            }

            if (request.Filters == null)
            {
                request.Filters = new List<Filter>();
            }

            var result = await _titleService.GetAllTitlesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _titleService.GetTitleByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTitleDto createTitleDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<TitleDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TitleController.InvalidModelState"),
                    _localizationService.GetLocalizedString("TitleController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _titleService.CreateTitleAsync(createTitleDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateTitleDto updateTitleDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<TitleDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TitleController.InvalidModelState"),
                    _localizationService.GetLocalizedString("TitleController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _titleService.UpdateTitleAsync(id, updateTitleDto);            
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _titleService.DeleteTitleAsync(id); 
            return StatusCode(result.StatusCode, result);
        }
    }
}
