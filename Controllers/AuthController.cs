using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using crm_api.Hubs;
using crm_api.Interfaces;
using crm_api.DTOs;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IHubContext<AuthHub> _hubContext;
        private readonly ILocalizationService _localizationService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IHubContext<AuthHub> hubContext, ILocalizationService localizationService, IAuthService authService, IUserService userService)
        {
            _hubContext = hubContext;
            _localizationService = localizationService;
            _authService = authService;
            _userService = userService;
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
                await AuthHub.ForceLogoutUser(_hubContext, loginResult.Data.UserId.ToString());
                return StatusCode(loginResult.StatusCode, loginResult);
            }

            return StatusCode(loginResult.StatusCode, ApiResponse<LoginWithSessionResponseDto>.ErrorResult(
                loginResult.Message,
                loginResult.ExceptionMessage,
                loginResult.StatusCode));
        }

        [AllowAnonymous]
        [HttpPost("admin-login")]
        public async Task<ActionResult<ApiResponse<string>>> AdminLogin()
        {
            var loginDto = new LoginRequest
            {
                Email = "admin@v3rii.com",
                Password = "Veriipass123!"
            };

            var loginDtoInternal = new LoginDto
            {
                Username = loginDto.Email,
                Password = loginDto.Password
            };

            var loginResult = await _authService.LoginWithSessionAsync(loginDtoInternal);

            // SignalR ile eski kullanıcıyı çıkış yaptır (eğer varsa)
            if (loginResult.Success && loginResult.Data != null)
            {
                await AuthHub.ForceLogoutUser(_hubContext, loginResult.Data.UserId.ToString());

                return StatusCode(loginResult.StatusCode, ApiResponse<string>.SuccessResult(
                    loginResult.Data.Token,
                    loginResult.Message));
            }

            return StatusCode(loginResult.StatusCode, ApiResponse<string>.ErrorResult(
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

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _authService.ChangePasswordAsync(request);
            return StatusCode(result.StatusCode, result);
        }

    }
}
