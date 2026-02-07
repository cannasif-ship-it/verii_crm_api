using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IUserPermissionGroupService
    {
        Task<ApiResponse<UserPermissionGroupDto>> GetByUserIdAsync(long userId);
        Task<ApiResponse<UserPermissionGroupDto>> SetUserGroupsAsync(long userId, SetUserPermissionGroupsDto dto);
    }
}
