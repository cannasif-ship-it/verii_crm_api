using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalRoleGroupController : ControllerBase
    {
        private readonly IApprovalRoleGroupService _approvalRoleGroupService;

        public ApprovalRoleGroupController(IApprovalRoleGroupService approvalRoleGroupService)
        {
            _approvalRoleGroupService = approvalRoleGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalRoleGroupService.GetAllApprovalRoleGroupsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalRoleGroupService.GetApprovalRoleGroupByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalRoleGroupCreateDto approvalRoleGroupCreateDto)
        {
            var result = await _approvalRoleGroupService.CreateApprovalRoleGroupAsync(approvalRoleGroupCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalRoleGroupUpdateDto approvalRoleGroupUpdateDto)
        {
            var result = await _approvalRoleGroupService.UpdateApprovalRoleGroupAsync(id, approvalRoleGroupUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalRoleGroupService.DeleteApprovalRoleGroupAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
