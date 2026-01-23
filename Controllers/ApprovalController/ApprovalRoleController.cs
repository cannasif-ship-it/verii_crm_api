using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalRoleController : ControllerBase
    {
        private readonly IApprovalRoleService _approvalRoleService;

        public ApprovalRoleController(IApprovalRoleService approvalRoleService)
        {
            _approvalRoleService = approvalRoleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalRoleService.GetAllApprovalRolesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalRoleService.GetApprovalRoleByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalRoleCreateDto approvalRoleCreateDto)
        {
            var result = await _approvalRoleService.CreateApprovalRoleAsync(approvalRoleCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalRoleUpdateDto approvalRoleUpdateDto)
        {
            var result = await _approvalRoleService.UpdateApprovalRoleAsync(id, approvalRoleUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalRoleService.DeleteApprovalRoleAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
