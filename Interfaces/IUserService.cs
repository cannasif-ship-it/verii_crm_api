using cms_webapi.DTOs;
using System.Security.Claims;

namespace cms_webapi.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<PagedResponse<UserDto>>> GetAllUsersAsync(PagedRequest request);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(long id);
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<ApiResponse<UserDto>> UpdateUserAsync(long id, UpdateUserDto dto);
        Task<ApiResponse<object>> DeleteUserAsync(long id);
        Task<ApiResponse<UserDto>> GetUserProfileAsync(string userId);
    }
}