using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IPermissionDefinitionService
    {
        Task<ApiResponse<PagedResponse<PermissionDefinitionDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<PermissionDefinitionDto>> GetByIdAsync(long id);
        Task<ApiResponse<PermissionDefinitionDto>> CreateAsync(CreatePermissionDefinitionDto dto);
        Task<ApiResponse<PermissionDefinitionDto>> UpdateAsync(long id, UpdatePermissionDefinitionDto dto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<PermissionDefinitionSyncResultDto>> SyncAsync(SyncPermissionDefinitionsDto dto);
    }
}
