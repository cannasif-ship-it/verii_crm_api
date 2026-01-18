using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalUserRoleController : ControllerBase
    {
        private readonly IApprovalUserRoleService _approvalUserRoleService;

        public ApprovalUserRoleController(IApprovalUserRoleService approvalUserRoleService)
        {
            _approvalUserRoleService = approvalUserRoleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalUserRoleService.GetAllApprovalUserRolesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalUserRoleService.GetApprovalUserRoleByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalUserRoleCreateDto approvalUserRoleCreateDto)
        {
            var result = await _approvalUserRoleService.CreateApprovalUserRoleAsync(approvalUserRoleCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalUserRoleUpdateDto approvalUserRoleUpdateDto)
        {
            var result = await _approvalUserRoleService.UpdateApprovalUserRoleAsync(id, approvalUserRoleUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalUserRoleService.DeleteApprovalUserRoleAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
