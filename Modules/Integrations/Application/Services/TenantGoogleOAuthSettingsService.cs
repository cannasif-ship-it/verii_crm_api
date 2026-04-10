using crm_api.Data;
using crm_api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace crm_api.Modules.Integrations.Application.Services
{
    public class TenantGoogleOAuthSettingsService : ITenantGoogleOAuthSettingsService
    {
        private const string DefaultScope = "https://www.googleapis.com/auth/calendar.events https://www.googleapis.com/auth/gmail.send";
        private static readonly Guid GlobalTenantId = Guid.Empty;

        private readonly CmsDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly ILocalizationService _localizationService;
        private readonly GoogleOptions _googleOptions;

        public TenantGoogleOAuthSettingsService(
            CmsDbContext dbContext,
            IEncryptionService encryptionService,
            ILocalizationService localizationService,
            IOptions<GoogleOptions> googleOptions)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _localizationService = localizationService;
            _googleOptions = googleOptions.Value;
        }

        public async Task<TenantGoogleOAuthRuntimeSettings?> GetRuntimeSettingsAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            var entity = await GetPrimarySettingsEntityAsync(cancellationToken).ConfigureAwait(false);

            if (entity == null)
            {
                entity = await TryBootstrapFromLegacyGoogleOptionsAsync(cancellationToken).ConfigureAwait(false);
            }

            if (entity == null)
            {
                return null;
            }

            var secret = SafeDecrypt(entity.ClientSecretEncrypted);
            if (string.IsNullOrWhiteSpace(secret))
            {
                return null;
            }

            var redirectUri = NormalizeRedirectUri(entity.RedirectUri);
            var scopes = NormalizeScopes(entity.Scopes);

            return new TenantGoogleOAuthRuntimeSettings
            {
                TenantId = entity.TenantId,
                ClientId = entity.ClientId?.Trim() ?? string.Empty,
                ClientSecret = secret,
                RedirectUri = redirectUri,
                Scopes = scopes,
                IsEnabled = entity.IsEnabled,
            };
        }

        public async Task<ApiResponse<TenantGoogleOAuthSettingsDto>> GetCurrentTenantSettingsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await GetPrimarySettingsEntityAsync(cancellationToken).ConfigureAwait(false);

                if (entity == null)
                {
                    entity = await TryBootstrapFromLegacyGoogleOptionsAsync(cancellationToken).ConfigureAwait(false);
                }

                var responseDto = MapToDto(entity?.TenantId ?? GlobalTenantId, entity);
                return ApiResponse<TenantGoogleOAuthSettingsDto>.SuccessResult(
                    responseDto,
                    _localizationService.GetLocalizedString("TenantGoogleOAuthSettingsService.GoogleOAuthSettingsRetrieved"));
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.ValidationError"),
                    ex.Message,
                    StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TenantGoogleOAuthSettingsDto>> UpsertCurrentTenantSettingsAsync(UpdateTenantGoogleOAuthSettingsDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.ClientId))
                {
                    return ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                        _localizationService.GetLocalizedString("General.ValidationError"),
                        _localizationService.GetLocalizedString("TenantGoogleOAuthSettingsService.ClientIdRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var allSettings = await _dbContext.Set<TenantGoogleOAuthSettings>()
                    .OrderByDescending(x => x.UpdatedAt)
                    .ToListAsync(cancellationToken).ConfigureAwait(false);
                var entity = allSettings.FirstOrDefault();

                var now = DateTimeOffset.UtcNow;
                if (entity == null)
                {
                    if (string.IsNullOrWhiteSpace(dto.ClientSecretPlain))
                    {
                        return ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                            _localizationService.GetLocalizedString("General.ValidationError"),
                            _localizationService.GetLocalizedString("TenantGoogleOAuthSettingsService.ClientSecretPlainRequiredForInitialSetup"),
                            StatusCodes.Status400BadRequest);
                    }

                    entity = new TenantGoogleOAuthSettings
                    {
                        Id = Guid.NewGuid(),
                        TenantId = GlobalTenantId,
                        CreatedAt = now,
                    };

                    await _dbContext.Set<TenantGoogleOAuthSettings>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
                }

                entity.ClientId = dto.ClientId.Trim();
                entity.RedirectUri = NormalizeRedirectUri(string.IsNullOrWhiteSpace(dto.RedirectUri)
                    ? entity.RedirectUri
                    : dto.RedirectUri);
                entity.Scopes = EnsureRequiredScopes(string.IsNullOrWhiteSpace(dto.Scopes)
                    ? entity.Scopes
                    : dto.Scopes);
                entity.IsEnabled = dto.IsEnabled;

                if (!string.IsNullOrWhiteSpace(dto.ClientSecretPlain))
                {
                    entity.ClientSecretEncrypted = _encryptionService.Encrypt(dto.ClientSecretPlain.Trim());
                }

                if (string.IsNullOrWhiteSpace(entity.ClientSecretEncrypted))
                {
                    return ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                        _localizationService.GetLocalizedString("General.ValidationError"),
                        _localizationService.GetLocalizedString("TenantGoogleOAuthSettingsService.ClientSecretNotConfigured"),
                        StatusCodes.Status400BadRequest);
                }

                entity.UpdatedAt = now;

                // Keep exactly one row globally; remove stale duplicates.
                var duplicates = allSettings.Skip(1).ToList();
                if (duplicates.Count > 0)
                {
                    _dbContext.Set<TenantGoogleOAuthSettings>().RemoveRange(duplicates);
                }

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                var responseDto = MapToDto(entity.TenantId, entity);
                return ApiResponse<TenantGoogleOAuthSettingsDto>.SuccessResult(
                    responseDto,
                    _localizationService.GetLocalizedString("TenantGoogleOAuthSettingsService.GoogleOAuthSettingsUpdated"));
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.ValidationError"),
                    ex.Message,
                    StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return ApiResponse<TenantGoogleOAuthSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        private TenantGoogleOAuthSettingsDto MapToDto(Guid tenantId, TenantGoogleOAuthSettings? entity)
        {
            var redirectUri = NormalizeRedirectUri(entity?.RedirectUri);
            var scopes = NormalizeScopes(entity?.Scopes);

            if (entity == null)
            {
                return new TenantGoogleOAuthSettingsDto
                {
                    TenantId = tenantId,
                    ClientId = string.Empty,
                    ClientSecretMasked = string.Empty,
                    RedirectUri = redirectUri,
                    Scopes = scopes,
                    IsEnabled = false,
                    IsConfigured = false,
                    UpdatedAt = null,
                };
            }

            return new TenantGoogleOAuthSettingsDto
            {
                TenantId = tenantId,
                ClientId = entity.ClientId,
                ClientSecretMasked = MaskSecret(SafeDecrypt(entity.ClientSecretEncrypted)),
                RedirectUri = redirectUri,
                Scopes = scopes,
                IsEnabled = entity.IsEnabled,
                IsConfigured = !string.IsNullOrWhiteSpace(entity.ClientId)
                               && !string.IsNullOrWhiteSpace(entity.ClientSecretEncrypted),
                UpdatedAt = entity.UpdatedAt,
            };
        }

        private string NormalizeRedirectUri(string? redirectUri)
        {
            if (!string.IsNullOrWhiteSpace(redirectUri))
            {
                return redirectUri.Trim();
            }

            if (!string.IsNullOrWhiteSpace(_googleOptions.RedirectUri))
            {
                return _googleOptions.RedirectUri.Trim();
            }

            return "/api/integrations/google/callback";
        }

        private string NormalizeScopes(string? scopes)
        {
            if (!string.IsNullOrWhiteSpace(scopes))
            {
                return EnsureRequiredScopes(scopes);
            }

            if (!string.IsNullOrWhiteSpace(_googleOptions.Scopes))
            {
                return EnsureRequiredScopes(_googleOptions.Scopes);
            }

            return EnsureRequiredScopes(DefaultScope);
        }

        private static string EnsureRequiredScopes(string? scopeSource)
        {
            var scopeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(scopeSource))
            {
                foreach (var scope in scopeSource
                             .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    scopeSet.Add(scope);
                }
            }

            scopeSet.Add("https://www.googleapis.com/auth/calendar.events");
            scopeSet.Add("https://www.googleapis.com/auth/gmail.send");

            return string.Join(' ', scopeSet);
        }

        private string? SafeDecrypt(string? encryptedValue)
        {
            if (string.IsNullOrWhiteSpace(encryptedValue))
            {
                return null;
            }

            try
            {
                return _encryptionService.Decrypt(encryptedValue);
            }
            catch
            {
                return null;
            }
        }

        private static string MaskSecret(string? secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
            {
                return string.Empty;
            }

            var trimmed = secret.Trim();
            var visibleLength = Math.Min(4, trimmed.Length);
            var suffix = trimmed[^visibleLength..];
            return $"****{suffix}";
        }

        private async Task<TenantGoogleOAuthSettings?> TryBootstrapFromLegacyGoogleOptionsAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_googleOptions.ClientId) || string.IsNullOrWhiteSpace(_googleOptions.ClientSecret))
            {
                return null;
            }

            var existing = await GetPrimarySettingsEntityAsync(cancellationToken).ConfigureAwait(false);
            if (existing != null)
            {
                return existing;
            }

            var now = DateTimeOffset.UtcNow;
            var entity = new TenantGoogleOAuthSettings
            {
                Id = Guid.NewGuid(),
                TenantId = GlobalTenantId,
                ClientId = _googleOptions.ClientId.Trim(),
                ClientSecretEncrypted = _encryptionService.Encrypt(_googleOptions.ClientSecret.Trim()),
                RedirectUri = string.IsNullOrWhiteSpace(_googleOptions.RedirectUri) ? null : _googleOptions.RedirectUri.Trim(),
                Scopes = string.IsNullOrWhiteSpace(_googleOptions.Scopes) ? null : _googleOptions.Scopes.Trim(),
                IsEnabled = true,
                CreatedAt = now,
                UpdatedAt = now,
            };

            await _dbContext.Set<TenantGoogleOAuthSettings>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        private async Task<TenantGoogleOAuthSettings?> GetPrimarySettingsEntityAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TenantGoogleOAuthSettings>()
                .AsNoTracking()
                .OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
