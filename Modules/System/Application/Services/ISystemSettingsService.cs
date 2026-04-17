using crm_api.Modules.System.Application.Dtos;

namespace crm_api.Modules.System.Application.Services
{
    public interface ISystemSettingsService
    {
        Task<ApiResponse<SystemSettingsDto>> GetAsync();
        Task<ApiResponse<SystemSettingsDto>> UpdateAsync(UpdateSystemSettingsDto dto, long userId);
    }
}
