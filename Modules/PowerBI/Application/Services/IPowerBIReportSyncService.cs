using crm_api.Modules.PowerBI.Application.Dtos;

namespace crm_api.Modules.PowerBI.Application.Services
{
    public interface IPowerBIReportSyncService
    {
        Task<ApiResponse<PowerBIReportSyncResultDto>> SyncAsync(Guid? workspaceId);
    }
}
