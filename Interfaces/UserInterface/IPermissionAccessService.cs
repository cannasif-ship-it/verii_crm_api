using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IPermissionAccessService
    {
        Task<ApiResponse<MyPermissionsDto>> GetMyPermissionsAsync();
    }
}
