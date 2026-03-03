using crm_api.DTOs;
using crm_api.Infrastructure;

namespace crm_api.Interfaces
{
    public interface ITenantGoogleOAuthSettingsService
    {
        Task<TenantGoogleOAuthRuntimeSettings?> GetRuntimeSettingsAsync(Guid tenantId, CancellationToken cancellationToken = default);
        Task<ApiResponse<TenantGoogleOAuthSettingsDto>> GetCurrentTenantSettingsAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<TenantGoogleOAuthSettingsDto>> UpsertCurrentTenantSettingsAsync(UpdateTenantGoogleOAuthSettingsDto dto, CancellationToken cancellationToken = default);
    }
}
