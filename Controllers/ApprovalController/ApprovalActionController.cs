using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalActionController : ControllerBase
    {
        private readonly IApprovalActionService _approvalActionService;

        public ApprovalActionController(IApprovalActionService approvalActionService)
        {
            _approvalActionService = approvalActionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalActionService.GetAllApprovalActionsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalActionService.GetApprovalActionByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalActionCreateDto approvalActionCreateDto)
        {
            var result = await _approvalActionService.CreateApprovalActionAsync(approvalActionCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalActionUpdateDto approvalActionUpdateDto)
        {
            var result = await _approvalActionService.UpdateApprovalActionAsync(id, approvalActionUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalActionService.DeleteApprovalActionAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
