using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using crm_api.DTOs;
using crm_api.Infrastructure;
using crm_api.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace crm_api.Services
{
    public class GoogleOAuthService : IGoogleOAuthService
    {
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string TokenEndpoint = "https://oauth2.googleapis.com/token";
        private const string UserInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo";
        private const string RevokeEndpoint = "https://oauth2.googleapis.com/revoke";

        private readonly GoogleOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<GoogleOAuthService> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public GoogleOAuthService(
            IOptions<GoogleOptions> options,
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache,
            ILogger<GoogleOAuthService> logger)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public Task<string> CreateAuthorizeUrlAsync(long userId, CancellationToken cancellationToken = default)
        {
            ValidateConfiguration();

            var randomState = WebEncoders.Base64UrlEncode(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));
            var state = $"{userId}.{randomState}";
            var stateKey = BuildStateCacheKey(userId, state);

            _memoryCache.Set(stateKey, true, TimeSpan.FromMinutes(10));

            var scopes = string.IsNullOrWhiteSpace(_options.Scopes)
                ? "https://www.googleapis.com/auth/calendar.events"
                : _options.Scopes;

            var query = new Dictionary<string, string?>
            {
                ["client_id"] = _options.ClientId,
                ["redirect_uri"] = _options.RedirectUri,
                ["response_type"] = "code",
                ["scope"] = scopes,
                ["access_type"] = "offline",
                ["prompt"] = "consent",
                ["include_granted_scopes"] = "true",
                ["state"] = state,
            };

            var url = QueryHelpers.AddQueryString(AuthorizationEndpoint, query!);
            return Task.FromResult(url);
        }

        public Task<bool> ValidateAndConsumeStateAsync(long userId, string state)
        {
            var stateKey = BuildStateCacheKey(userId, state);
            var isValid = _memoryCache.TryGetValue(stateKey, out _);
            if (isValid)
            {
                _memoryCache.Remove(stateKey);
            }

            return Task.FromResult(isValid);
        }

        public bool TryExtractUserIdFromState(string state, out long userId)
        {
            userId = 0;
            if (string.IsNullOrWhiteSpace(state))
            {
                return false;
            }

            var separatorIndex = state.IndexOf('.');
            if (separatorIndex <= 0)
            {
                return false;
            }

            var userIdPart = state[..separatorIndex];
            return long.TryParse(userIdPart, out userId) && userId > 0;
        }

        public Task<GoogleOAuthTokenResult> ExchangeCodeForTokensAsync(string code, CancellationToken cancellationToken = default)
        {
            ValidateConfiguration();

            var formData = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["redirect_uri"] = _options.RedirectUri,
            };

            return SendTokenRequestAsync(formData, cancellationToken);
        }

        public Task<GoogleOAuthTokenResult> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            ValidateConfiguration();

            var formData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
            };

            return SendTokenRequestAsync(formData, cancellationToken);
        }

        public async Task<string?> GetGoogleEmailAsync(string accessToken, string? idToken, CancellationToken cancellationToken = default)
        {
            var emailFromIdToken = TryReadEmailFromIdToken(idToken);
            if (!string.IsNullOrWhiteSpace(emailFromIdToken))
            {
                return emailFromIdToken;
            }

            var client = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, UserInfoEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Google userinfo request failed with status code {StatusCode}", response.StatusCode);
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var payload = JsonSerializer.Deserialize<GoogleUserInfoResponse>(responseBody, JsonOptions);
            return payload?.Email;
        }

        public async Task RevokeTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            var client = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, RevokeEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["token"] = token,
                }),
            };

            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Google token revoke request failed with status code {StatusCode}", response.StatusCode);
            }
        }

        private async Task<GoogleOAuthTokenResult> SendTokenRequestAsync(
            Dictionary<string, string> formData,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(formData),
            };

            using var response = await client.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                string? providerError = null;
                string? providerErrorDescription = null;
                try
                {
                    using var errorDoc = JsonDocument.Parse(responseBody);
                    if (errorDoc.RootElement.TryGetProperty("error", out var errorElement))
                    {
                        providerError = errorElement.GetString();
                    }

                    if (errorDoc.RootElement.TryGetProperty("error_description", out var descriptionElement))
                    {
                        providerErrorDescription = descriptionElement.GetString();
                    }
                }
                catch
                {
                    // ignore parse error and keep generic logging/exception below
                }

                _logger.LogWarning(
                    "Google token endpoint returned non-success status code {StatusCode}. Body: {Body}",
                    response.StatusCode,
                    responseBody);

                var providerMessage = string.IsNullOrWhiteSpace(providerError)
                    ? "unknown_error"
                    : providerError;

                if (!string.IsNullOrWhiteSpace(providerErrorDescription))
                {
                    providerMessage = $"{providerMessage}: {providerErrorDescription}";
                }

                throw new InvalidOperationException($"Google token request failed: {providerMessage}");
            }

            var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(responseBody, JsonOptions);
            if (tokenResponse == null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                throw new InvalidOperationException("Google token response is invalid.");
            }

            return new GoogleOAuthTokenResult
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresInSeconds = tokenResponse.ExpiresIn,
                Scope = tokenResponse.Scope,
                IdToken = tokenResponse.IdToken,
            };
        }

        private static string BuildStateCacheKey(long userId, string state)
        {
            return $"google_oauth_state:{userId}:{state}";
        }

        private static string? TryReadEmailFromIdToken(string? idToken)
        {
            if (string.IsNullOrWhiteSpace(idToken))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(idToken))
            {
                return null;
            }

            var jwt = handler.ReadJwtToken(idToken);
            var emailClaim = jwt.Claims.FirstOrDefault(c => string.Equals(c.Type, "email", StringComparison.OrdinalIgnoreCase));
            return emailClaim?.Value;
        }

        private void ValidateConfiguration()
        {
            var missing = new List<string>();
            if (string.IsNullOrWhiteSpace(_options.ClientId))
            {
                missing.Add("Google:ClientId");
            }

            if (string.IsNullOrWhiteSpace(_options.ClientSecret))
            {
                missing.Add("Google:ClientSecret");
            }

            if (string.IsNullOrWhiteSpace(_options.RedirectUri))
            {
                missing.Add("Google:RedirectUri");
            }

            if (missing.Count > 0)
            {
                throw new InvalidOperationException($"Google OAuth configuration is missing: {string.Join(", ", missing)}");
            }
        }

        private sealed class GoogleTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; } = string.Empty;

            [JsonPropertyName("refresh_token")]
            public string? RefreshToken { get; set; }

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonPropertyName("scope")]
            public string? Scope { get; set; }

            [JsonPropertyName("id_token")]
            public string? IdToken { get; set; }
        }

        private sealed class GoogleUserInfoResponse
        {
            public string? Email { get; set; }
        }
    }
}
