using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalWorkflowController : ControllerBase
    {
        private readonly IApprovalWorkflowService _service;
        private readonly ILocalizationService _localizationService;

        public ApprovalWorkflowController(IApprovalWorkflowService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _service.GetAllApprovalWorkflowsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetApprovalWorkflowByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateApprovalWorkflowDto dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<ApprovalWorkflowDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalWorkflowController.InvalidModelState"),
                    _localizationService.GetLocalizedString("ApprovalWorkflowController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }
            var result = await _service.CreateApprovalWorkflowAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateApprovalWorkflowDto dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<ApprovalWorkflowDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalWorkflowController.InvalidModelState"),
                    _localizationService.GetLocalizedString("ApprovalWorkflowController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }
            var result = await _service.UpdateApprovalWorkflowAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteApprovalWorkflowAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}