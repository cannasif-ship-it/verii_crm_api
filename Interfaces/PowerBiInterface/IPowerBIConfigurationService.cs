using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IPowerBIConfigurationService
    {
        Task<ApiResponse<PowerBIConfigurationGetDto?>> GetAsync();
        Task<ApiResponse<PowerBIConfigurationGetDto>> CreateAsync(CreatePowerBIConfigurationDto dto);
        Task<ApiResponse<PowerBIConfigurationGetDto>> UpdateAsync(long id, UpdatePowerBIConfigurationDto dto);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
