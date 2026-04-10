using crm_api.Modules.AccessControl.Application.Dtos;

namespace crm_api.Modules.AccessControl.Application.Services
{
    public interface IPermissionAccessService
    {
        Task<ApiResponse<MyPermissionsDto>> GetMyPermissionsAsync();
    }
}
