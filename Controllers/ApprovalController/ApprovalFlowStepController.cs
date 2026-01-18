using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalFlowStepController : ControllerBase
    {
        private readonly IApprovalFlowStepService _approvalFlowStepService;

        public ApprovalFlowStepController(IApprovalFlowStepService approvalFlowStepService)
        {
            _approvalFlowStepService = approvalFlowStepService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalFlowStepService.GetAllApprovalFlowStepsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalFlowStepService.GetApprovalFlowStepByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalFlowStepCreateDto approvalFlowStepCreateDto)
        {
            var result = await _approvalFlowStepService.CreateApprovalFlowStepAsync(approvalFlowStepCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalFlowStepUpdateDto approvalFlowStepUpdateDto)
        {
            var result = await _approvalFlowStepService.UpdateApprovalFlowStepAsync(id, approvalFlowStepUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalFlowStepService.DeleteApprovalFlowStepAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
