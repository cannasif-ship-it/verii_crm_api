using crm_api.Helpers;
using crm_api.Modules.PowerBI.Application.Dtos;

namespace crm_api.Modules.PowerBI.Application.Services
{
    public interface IPowerBIEmbedService
    {
        Task<ApiResponse<EmbedConfigDto>> GetEmbedConfigAsync(long reportDefinitionId);
    }
}
