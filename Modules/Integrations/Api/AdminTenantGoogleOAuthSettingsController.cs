using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.Integrations.Api
{
    [ApiController]
    [Route("api/admin/tenants/google-oauth/settings")]
    [Authorize]
    public class AdminTenantGoogleOAuthSettingsController : ControllerBase
    {
        private readonly ITenantGoogleOAuthSettingsService _tenantGoogleOAuthSettingsService;
        private readonly IPermissionAccessService _permissionAccessService;

        public AdminTenantGoogleOAuthSettingsController(
            ITenantGoogleOAuthSettingsService tenantGoogleOAuthSettingsService,
            IPermissionAccessService permissionAccessService)
        {
            _tenantGoogleOAuthSettingsService = tenantGoogleOAuthSettingsService;
            _permissionAccessService = permissionAccessService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<TenantGoogleOAuthSettingsDto>>> Get(CancellationToken cancellationToken)
        {
            if (!await CanManageTenantGoogleOAuthSettingsAsync())
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                        "Forbidden",
                        "Only TenantAdmin/SystemAdmin can access this endpoint.",
                        StatusCodes.Status403Forbidden));
            }

            var response = await _tenantGoogleOAuthSettingsService.GetCurrentTenantSettingsAsync(cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<TenantGoogleOAuthSettingsDto>>> Put(
            [FromBody] UpdateTenantGoogleOAuthSettingsDto dto,
            CancellationToken cancellationToken)
        {
            if (!await CanManageTenantGoogleOAuthSettingsAsync())
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                        "Forbidden",
                        "Only TenantAdmin/SystemAdmin can access this endpoint.",
                        StatusCodes.Status403Forbidden));
            }

            var response = await _tenantGoogleOAuthSettingsService.UpsertCurrentTenantSettingsAsync(dto, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        private async Task<bool> CanManageTenantGoogleOAuthSettingsAsync()
        {
            var permissionResponse = await _permissionAccessService.GetMyPermissionsAsync();
            if (!permissionResponse.Success || permissionResponse.Data == null)
            {
                return false;
            }

            if (permissionResponse.Data.IsSystemAdmin)
            {
                return true;
            }

            var role = permissionResponse.Data.RoleTitle?.Trim();
            return string.Equals(role, "TenantAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, "SystemAdmin", StringComparison.OrdinalIgnoreCase);
        }
    }
}
