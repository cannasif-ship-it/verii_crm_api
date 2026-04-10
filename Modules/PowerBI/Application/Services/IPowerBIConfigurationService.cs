using crm_api.Helpers;
using crm_api.Modules.PowerBI.Application.Dtos;

namespace crm_api.Modules.PowerBI.Application.Services
{
    public interface IPowerBIConfigurationService
    {
        Task<ApiResponse<PowerBIConfigurationGetDto?>> GetAsync();
        Task<ApiResponse<PowerBIConfigurationGetDto>> CreateAsync(CreatePowerBIConfigurationDto dto);
        Task<ApiResponse<PowerBIConfigurationGetDto>> UpdateAsync(long id, UpdatePowerBIConfigurationDto dto);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
