using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalFlowController : ControllerBase
    {
        private readonly IApprovalFlowService _approvalFlowService;

        public ApprovalFlowController(IApprovalFlowService approvalFlowService)
        {
            _approvalFlowService = approvalFlowService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalFlowService.GetAllApprovalFlowsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalFlowService.GetApprovalFlowByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalFlowCreateDto approvalFlowCreateDto)
        {
            var result = await _approvalFlowService.CreateApprovalFlowAsync(approvalFlowCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalFlowUpdateDto approvalFlowUpdateDto)
        {
            var result = await _approvalFlowService.UpdateApprovalFlowAsync(id, approvalFlowUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalFlowService.DeleteApprovalFlowAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
