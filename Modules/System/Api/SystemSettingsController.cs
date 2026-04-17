using crm_api.Shared.Common.Application;
using crm_api.Modules.Identity.Application.Services;
using crm_api.Modules.System.Application.Dtos;
using crm_api.Modules.System.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.System.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IUserService _userService;

        public SystemSettingsController(
            ISystemSettingsService systemSettingsService,
            IUserService userService)
        {
            _systemSettingsService = systemSettingsService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SystemSettingsDto>>> Get()
        {
            var response = await _systemSettingsService.GetAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<SystemSettingsDto>>> Update([FromBody] UpdateSystemSettingsDto dto)
        {
            var currentUserResponse = await _userService.GetCurrentUserIdAsync();
            if (!currentUserResponse.Success)
            {
                var unauth = ApiResponse<SystemSettingsDto>.ErrorResult(
                    currentUserResponse.Message,
                    currentUserResponse.Message,
                    StatusCodes.Status401Unauthorized);

                return StatusCode(unauth.StatusCode, unauth);
            }

            var response = await _systemSettingsService.UpdateAsync(dto, currentUserResponse.Data);
            return StatusCode(response.StatusCode, response);
        }
    }
}
