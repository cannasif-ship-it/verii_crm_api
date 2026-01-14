using cms_webapi.DTOs;
using cms_webapi.Models;

namespace cms_webapi.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserDto>> GetUserByUsernameAsync(string username);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(long id);
        Task<ApiResponse<UserDto>> RegisterUserAsync(RegisterDto registerDto);
        Task<ApiResponse<string>> LoginAsync(LoginRequest request);
        Task<ApiResponse<UserDto>> GetUserByEmailOrUsernameAsync(string emailOrUsername);
        Task<ApiResponse<LoginWithSessionResponseDto>> LoginWithSessionAsync(LoginDto loginDto);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<IEnumerable<UserDto>>> GetActiveUsersAsync();
        Task<ApiResponse<string>> RequestPasswordResetAsync(ForgotPasswordRequest request);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponse<bool>> ChangePasswordAsync(long userId, ChangePasswordRequest request);
    }

    public class LoginWithSessionResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public long UserId { get; set; }
        public Guid SessionId { get; set; }
    }
}