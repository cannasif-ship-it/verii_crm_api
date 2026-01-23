using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalRequestController : ControllerBase
    {
        private readonly IApprovalRequestService _approvalRequestService;

        public ApprovalRequestController(IApprovalRequestService approvalRequestService)
        {
            _approvalRequestService = approvalRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalRequestService.GetAllApprovalRequestsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalRequestService.GetApprovalRequestByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalRequestCreateDto approvalRequestCreateDto)
        {
            var result = await _approvalRequestService.CreateApprovalRequestAsync(approvalRequestCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalRequestUpdateDto approvalRequestUpdateDto)
        {
            var result = await _approvalRequestService.UpdateApprovalRequestAsync(id, approvalRequestUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalRequestService.DeleteApprovalRequestAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
