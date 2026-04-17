using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using crm_api.Hubs;
using crm_api.Modules.System.Application.Services;
using crm_api.Shared.Common.Application;

namespace crm_api.Modules.Identity.Api
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IHubContext<AuthHub> _hubContext;
        private readonly ILocalizationService _localizationService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IPermissionAccessService _permissionAccessService;
        private readonly ISystemSettingsService _systemSettingsService;

        public AuthController(
            IHubContext<AuthHub> hubContext,
            ILocalizationService localizationService,
            IAuthService authService,
            IUserService userService,
            IPermissionAccessService permissionAccessService,
            ISystemSettingsService systemSettingsService)
        {
            _hubContext = hubContext;
            _localizationService = localizationService;
            _authService = authService;
            _userService = userService;
            _permissionAccessService = permissionAccessService;
            _systemSettingsService = systemSettingsService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginWithSessionResponseDto>>> Login([FromBody] LoginRequest request)
        {
            var loginDto = new LoginDto
            {
                Username = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe
            };

            var loginResult = await _authService.LoginWithSessionAsync(loginDto);

            if (loginResult.Success && loginResult.Data != null)
            {
                return StatusCode(loginResult.StatusCode, loginResult);
            }

            return StatusCode(loginResult.StatusCode, ApiResponse<LoginWithSessionResponseDto>.ErrorResult(
                loginResult.Message,
                loginResult.ExceptionMessage,
                loginResult.StatusCode));
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return StatusCode(401, ApiResponse<UserDto>.ErrorResult(
                    _localizationService.GetLocalizedString("AuthService.UserIdNotFound"),
                    "Unauthorized",
                    401));
            }

            var result = await _userService.GetUserProfileAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("me/permissions")]
        public async Task<ActionResult<ApiResponse<MyPermissionsDto>>> GetMyPermissions()
        {
            var result = await _permissionAccessService.GetMyPermissionsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("bootstrap")]
        public async Task<ActionResult<ApiResponse<AppBootstrapDto>>> GetBootstrap()
        {
            var permissionsResult = await _permissionAccessService.GetMyPermissionsAsync();
            if (!permissionsResult.Success || permissionsResult.Data == null)
            {
                return StatusCode(
                    permissionsResult.StatusCode,
                    ApiResponse<AppBootstrapDto>.ErrorResult(
                        permissionsResult.Message,
                        permissionsResult.ExceptionMessage,
                        permissionsResult.StatusCode));
            }

            var settingsResult = await _systemSettingsService.GetAsync();
            if (!settingsResult.Success || settingsResult.Data == null)
            {
                return StatusCode(
                    settingsResult.StatusCode,
                    ApiResponse<AppBootstrapDto>.ErrorResult(
                        settingsResult.Message,
                        settingsResult.ExceptionMessage,
                        settingsResult.StatusCode));
            }

            var firstName = User.FindFirst("firstName")?.Value?.Trim() ?? string.Empty;
            var lastName = User.FindFirst("lastName")?.Value?.Trim() ?? string.Empty;
            var fullName = $"{firstName} {lastName}".Trim();
            var fallbackName = User.FindFirst(ClaimTypes.Name)?.Value?.Trim() ?? string.Empty;
            var email = User.FindFirst(ClaimTypes.Email)?.Value?.Trim()
                ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value?.Trim()
                ?? string.Empty;
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdValue, out var userId))
            {
                return StatusCode(
                    StatusCodes.Status401Unauthorized,
                    ApiResponse<AppBootstrapDto>.ErrorResult(
                        _localizationService.GetLocalizedString("General.Unauthorized"),
                        _localizationService.GetLocalizedString("General.Unauthorized"),
                        StatusCodes.Status401Unauthorized));
            }

            var bootstrap = new AppBootstrapDto
            {
                User = new AppBootstrapUserDto
                {
                    Id = userId,
                    Email = email,
                    Name = string.IsNullOrWhiteSpace(fullName) ? fallbackName : fullName,
                },
                Permissions = permissionsResult.Data,
                SystemSettings = settingsResult.Data,
            };

            var response = ApiResponse<AppBootstrapDto>.SuccessResult(
                bootstrap,
                _localizationService.GetLocalizedString("General.OperationSuccessful"));

            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<ActionResult<ApiResponse<string>>> RequestPasswordReset([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authService.RequestPasswordResetAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<string>>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _authService.ChangePasswordAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<LoginWithSessionResponseDto>>> RefreshToken([FromBody] RefreshTokenDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return StatusCode(result.StatusCode, result);
        }

    }
}
