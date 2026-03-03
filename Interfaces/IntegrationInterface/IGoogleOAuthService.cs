using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IGoogleOAuthService
    {
        Task<string> CreateAuthorizeUrlAsync(long userId, CancellationToken cancellationToken = default);
        Task<bool> ValidateAndConsumeStateAsync(long userId, string state);
        bool TryExtractUserIdFromState(string state, out long userId);
        Task<GoogleOAuthTokenResult> ExchangeCodeForTokensAsync(string code, CancellationToken cancellationToken = default);
        Task<GoogleOAuthTokenResult> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<string?> GetGoogleEmailAsync(string accessToken, string? idToken, CancellationToken cancellationToken = default);
        Task RevokeTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}
