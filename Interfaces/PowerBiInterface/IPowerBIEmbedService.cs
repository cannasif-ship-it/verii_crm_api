using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IPowerBIEmbedService
    {
        Task<ApiResponse<EmbedConfigDto>> GetEmbedConfigAsync(long reportDefinitionId);
    }
}
