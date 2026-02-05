using crm_api.DTOs;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IPowerBIReportSyncService
    {
        Task<ApiResponse<PowerBIReportSyncResultDto>> SyncAsync(Guid? workspaceId);
    }
}
