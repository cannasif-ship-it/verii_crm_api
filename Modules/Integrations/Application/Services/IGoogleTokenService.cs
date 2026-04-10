
namespace crm_api.Modules.Integrations.Application.Services
{
    public interface IGoogleTokenService
    {
        Task<UserGoogleAccount?> GetAccountAsync(long userId, CancellationToken cancellationToken = default);
        Task<UserGoogleAccount> UpsertConnectionAsync(
            long userId,
            Guid tenantId,
            GoogleOAuthTokenResult tokenResult,
            string? googleEmail,
            string configuredScopes,
            CancellationToken cancellationToken = default);
        Task<string?> GetValidAccessTokenAsync(long userId, bool forceRefresh = false, CancellationToken cancellationToken = default);
        Task<bool> DisconnectAsync(long userId, CancellationToken cancellationToken = default);
    }
}
