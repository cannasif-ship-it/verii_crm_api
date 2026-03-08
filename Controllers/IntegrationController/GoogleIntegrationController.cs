using crm_api.DTOs;
using crm_api.Infrastructure;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/integrations/google")]
    [Authorize]
    public class GoogleIntegrationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleOAuthService _googleOAuthService;
        private readonly IGoogleTokenService _googleTokenService;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly IGoogleIntegrationLogService _googleIntegrationLogService;
        private readonly ITenantGoogleOAuthSettingsService _tenantGoogleOAuthSettingsService;
        private readonly IUserContextService _userContextService;
        private readonly IEncryptionService _encryptionService;
        private readonly ILocalizationService _localizationService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleIntegrationController> _logger;

        public GoogleIntegrationController(
            IUserService userService,
            IGoogleOAuthService googleOAuthService,
            IGoogleTokenService googleTokenService,
            IGoogleCalendarService googleCalendarService,
            IGoogleIntegrationLogService googleIntegrationLogService,
            ITenantGoogleOAuthSettingsService tenantGoogleOAuthSettingsService,
            IUserContextService userContextService,
            IEncryptionService encryptionService,
            ILocalizationService localizationService,
            IConfiguration configuration,
            ILogger<GoogleIntegrationController> logger)
        {
            _userService = userService;
            _googleOAuthService = googleOAuthService;
            _googleTokenService = googleTokenService;
            _googleCalendarService = googleCalendarService;
            _googleIntegrationLogService = googleIntegrationLogService;
            _tenantGoogleOAuthSettingsService = tenantGoogleOAuthSettingsService;
            _userContextService = userContextService;
            _encryptionService = encryptionService;
            _localizationService = localizationService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("logs")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GoogleIntegrationLogDto>>>> GetLogs(
            [FromQuery] GoogleIntegrationLogsQueryDto query,
            CancellationToken cancellationToken)
        {
            var currentUserIdResult = await _userService.GetCurrentUserIdAsync();
            if (!currentUserIdResult.Success)
            {
                var error = ApiResponse<PagedResponse<GoogleIntegrationLogDto>>.ErrorResult(
                    currentUserIdResult.Message,
                    currentUserIdResult.ExceptionMessage,
                    currentUserIdResult.StatusCode);

                return StatusCode(error.StatusCode, error);
            }

            var tenantId = _userContextService.GetCurrentTenantId() ?? Guid.Empty;
            if (tenantId == Guid.Empty)
            {
                var error = ApiResponse<PagedResponse<GoogleIntegrationLogDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleIntegrationLogsCouldNotBeLoaded"),
                    _localizationService.GetLocalizedString("GoogleIntegrationController.TenantContextIsMissing"),
                    StatusCodes.Status400BadRequest);

                return StatusCode(error.StatusCode, error);
            }

            try
            {
                var logs = await _googleIntegrationLogService.GetPagedAsync(
                    tenantId,
                    currentUserIdResult.Data,
                    query,
                    cancellationToken);

                var response = ApiResponse<PagedResponse<GoogleIntegrationLogDto>>.SuccessResult(
                    logs,
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleIntegrationLogsRetrieved"));

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load Google integration logs for tenant {TenantId}", tenantId);
                var error = ApiResponse<PagedResponse<GoogleIntegrationLogDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleIntegrationLogsCouldNotBeLoaded"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
                return StatusCode(error.StatusCode, error);
            }
        }

        [HttpGet("status")]
        public async Task<ActionResult<ApiResponse<GoogleIntegrationStatusDto>>> GetStatus(CancellationToken cancellationToken)
        {
            var currentUserIdResult = await _userService.GetCurrentUserIdAsync();
            if (!currentUserIdResult.Success)
            {
                var error = ApiResponse<GoogleIntegrationStatusDto>.ErrorResult(
                    currentUserIdResult.Message,
                    currentUserIdResult.ExceptionMessage,
                    currentUserIdResult.StatusCode);

                return StatusCode(error.StatusCode, error);
            }

            var account = await _googleTokenService.GetAccountAsync(currentUserIdResult.Data, cancellationToken);
            var tenantId = _userContextService.GetCurrentTenantId();
            var tenantSettings = tenantId.HasValue
                ? await _tenantGoogleOAuthSettingsService.GetRuntimeSettingsAsync(tenantId.Value, cancellationToken)
                : null;
            var dto = new GoogleIntegrationStatusDto
            {
                IsConnected = account?.IsConnected == true,
                IsOAuthConfigured = tenantSettings?.IsEnabled == true,
                GoogleEmail = account?.GoogleEmail,
                Scopes = account?.Scopes,
                ExpiresAt = account?.ExpiresAt,
            };

            var response = ApiResponse<GoogleIntegrationStatusDto>.SuccessResult(
                dto,
                _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleIntegrationStatusRetrieved"));
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("authorize-url")]
        public async Task<ActionResult<ApiResponse<GoogleAuthorizeUrlDto>>> GetAuthorizeUrl(CancellationToken cancellationToken)
        {
            var currentUserIdResult = await _userService.GetCurrentUserIdAsync();
            if (!currentUserIdResult.Success)
            {
                var error = ApiResponse<GoogleAuthorizeUrlDto>.ErrorResult(
                    currentUserIdResult.Message,
                    currentUserIdResult.ExceptionMessage,
                    currentUserIdResult.StatusCode);

                return StatusCode(error.StatusCode, error);
            }

            try
            {
                var url = await _googleOAuthService.CreateAuthorizeUrlAsync(currentUserIdResult.Data, cancellationToken);

                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = _userContextService.GetCurrentTenantId(),
                    UserId = currentUserIdResult.Data,
                    Operation = "google.oauth.authorize-url",
                    IsSuccess = true,
                    Message = "Google authorize URL created.",
                }, cancellationToken);

                var response = ApiResponse<GoogleAuthorizeUrlDto>.SuccessResult(
                    new GoogleAuthorizeUrlDto { Url = url },
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleAuthorizeUrlCreated"));
                return StatusCode(response.StatusCode, response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Google authorize URL configuration error for user {UserId}", currentUserIdResult.Data);
                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = _userContextService.GetCurrentTenantId(),
                    UserId = currentUserIdResult.Data,
                    Operation = "google.oauth.authorize-url",
                    IsSuccess = false,
                    Severity = "Warning",
                    Message = "Google authorize URL configuration error.",
                    ErrorCode = "configuration_error",
                    Metadata = ex.Message,
                }, cancellationToken);

                var error = ApiResponse<GoogleAuthorizeUrlDto>.ErrorResult(
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleAuthorizeUrlCouldNotBeGenerated"),
                    ex.Message,
                    StatusCodes.Status400BadRequest);

                return StatusCode(error.StatusCode, error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Google authorize URL for user {UserId}", currentUserIdResult.Data);
                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = _userContextService.GetCurrentTenantId(),
                    UserId = currentUserIdResult.Data,
                    Operation = "google.oauth.authorize-url",
                    IsSuccess = false,
                    Severity = "Error",
                    Message = "Google authorize URL creation failed.",
                    ErrorCode = "unexpected_error",
                    Metadata = ex.Message,
                }, cancellationToken);

                var error = ApiResponse<GoogleAuthorizeUrlDto>.ErrorResult(
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleAuthorizeUrlCouldNotBeGenerated"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);

                return StatusCode(error.StatusCode, error);
            }
        }

        [AllowAnonymous]
        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
            [FromQuery] string? code,
            [FromQuery] string? state,
            [FromQuery] string? error,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                if (_googleOAuthService.TryExtractStateContext(state ?? string.Empty, out var callbackUserIdFromError, out var callbackTenantIdFromError))
                {
                    await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                    {
                        TenantId = callbackTenantIdFromError,
                        UserId = callbackUserIdFromError,
                        Operation = "google.oauth.callback",
                        IsSuccess = false,
                        Severity = "Warning",
                        Message = "Google callback returned provider error.",
                        ErrorCode = error,
                    }, cancellationToken);
                }

                return Redirect(BuildFrontendRedirect(false, error));
            }

            if (string.IsNullOrWhiteSpace(state) || !_googleOAuthService.TryExtractStateContext(state, out var userId, out var tenantId))
            {
                return Redirect(BuildFrontendRedirect(false, "invalid_state"));
            }

            var stateValid = await _googleOAuthService.ValidateAndConsumeStateAsync(userId, state);
            if (!stateValid)
            {
                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = tenantId,
                    UserId = userId,
                    Operation = "google.oauth.callback",
                    IsSuccess = false,
                    Severity = "Warning",
                    Message = "Google callback state expired or already consumed.",
                    ErrorCode = "state_expired",
                }, cancellationToken);

                return Redirect(BuildFrontendRedirect(false, "state_expired"));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = tenantId,
                    UserId = userId,
                    Operation = "google.oauth.callback",
                    IsSuccess = false,
                    Severity = "Warning",
                    Message = "Google callback code was missing.",
                    ErrorCode = "missing_code",
                }, cancellationToken);

                return Redirect(BuildFrontendRedirect(false, "missing_code"));
            }

            try
            {
                var tokenResult = await _googleOAuthService.ExchangeCodeForTokensAsync(code, tenantId, cancellationToken);
                var googleEmail = await _googleOAuthService.GetGoogleEmailAsync(
                    tokenResult.AccessToken,
                    tokenResult.IdToken,
                    cancellationToken);

                await _googleTokenService.UpsertConnectionAsync(
                    userId,
                    tenantId,
                    tokenResult,
                    googleEmail,
                    tokenResult.Scope ?? string.Empty,
                    cancellationToken);

                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = tenantId,
                    UserId = userId,
                    Operation = "google.oauth.callback",
                    IsSuccess = true,
                    Message = "Google OAuth callback completed successfully.",
                }, cancellationToken);

                return Redirect(BuildFrontendRedirect(true));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Google callback validation/token flow failed for user {UserId}", userId);
                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = tenantId,
                    UserId = userId,
                    Operation = "google.oauth.callback",
                    IsSuccess = false,
                    Severity = "Warning",
                    Message = "Google callback validation/token flow failed.",
                    ErrorCode = "oauth_flow_error",
                    Metadata = ex.Message,
                }, cancellationToken);

                var reason = ex.Message;
                if (reason.Length > 200)
                {
                    reason = reason[..200];
                }

                return Redirect(BuildFrontendRedirect(false, reason));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google callback failed for user {UserId}", userId);
                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = tenantId,
                    UserId = userId,
                    Operation = "google.oauth.callback",
                    IsSuccess = false,
                    Severity = "Error",
                    Message = "Google callback failed.",
                    ErrorCode = "oauth_callback_failed",
                    Metadata = ex.Message,
                }, cancellationToken);

                return Redirect(BuildFrontendRedirect(false, "oauth_callback_failed"));
            }
        }

        [HttpPost("disconnect")]
        public async Task<ActionResult<ApiResponse<object>>> Disconnect(CancellationToken cancellationToken)
        {
            var currentUserIdResult = await _userService.GetCurrentUserIdAsync();
            if (!currentUserIdResult.Success)
            {
                var error = ApiResponse<object>.ErrorResult(
                    currentUserIdResult.Message,
                    currentUserIdResult.ExceptionMessage,
                    currentUserIdResult.StatusCode);

                return StatusCode(error.StatusCode, error);
            }

            try
            {
                var account = await _googleTokenService.GetAccountAsync(currentUserIdResult.Data, cancellationToken);
                if (account != null)
                {
                    var tokenToRevoke = TryDecrypt(account.RefreshTokenEncrypted)
                        ?? TryDecrypt(account.AccessTokenEncrypted);

                    if (!string.IsNullOrWhiteSpace(tokenToRevoke))
                    {
                        await _googleOAuthService.RevokeTokenAsync(tokenToRevoke, cancellationToken);
                    }
                }

                await _googleTokenService.DisconnectAsync(currentUserIdResult.Data, cancellationToken);

                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = _userContextService.GetCurrentTenantId(),
                    UserId = currentUserIdResult.Data,
                    Operation = "google.oauth.disconnect",
                    IsSuccess = true,
                    Message = "Google integration disconnected.",
                }, cancellationToken);

                var response = ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleIntegrationDisconnected"));
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google disconnect failed for user {UserId}", currentUserIdResult.Data);
                await _googleIntegrationLogService.WriteAsync(new GoogleIntegrationLogWriteDto
                {
                    TenantId = _userContextService.GetCurrentTenantId(),
                    UserId = currentUserIdResult.Data,
                    Operation = "google.oauth.disconnect",
                    IsSuccess = false,
                    Severity = "Error",
                    Message = "Google disconnect failed.",
                    ErrorCode = "disconnect_failed",
                    Metadata = ex.Message,
                }, cancellationToken);

                var error = ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleIntegrationCouldNotBeDisconnected"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);

                return StatusCode(error.StatusCode, error);
            }
        }

        [HttpPost("test-event")]
        public async Task<ActionResult<ApiResponse<GoogleTestEventDto>>> CreateTestEvent(CancellationToken cancellationToken)
        {
            var currentUserIdResult = await _userService.GetCurrentUserIdAsync();
            if (!currentUserIdResult.Success)
            {
                var error = ApiResponse<GoogleTestEventDto>.ErrorResult(
                    currentUserIdResult.Message,
                    currentUserIdResult.ExceptionMessage,
                    currentUserIdResult.StatusCode);

                return StatusCode(error.StatusCode, error);
            }

            try
            {
                var eventId = await _googleCalendarService.CreateTestEventAsync(currentUserIdResult.Data, cancellationToken);
                var response = ApiResponse<GoogleTestEventDto>.SuccessResult(
                    new GoogleTestEventDto { EventId = eventId },
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleCalendarTestEventCreated"));

                return StatusCode(response.StatusCode, response);
            }
            catch (InvalidOperationException ex)
            {
                var error = ApiResponse<GoogleTestEventDto>.ErrorResult(
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleCalendarTestEventCouldNotBeCreated"),
                    ex.Message,
                    StatusCodes.Status400BadRequest);

                return StatusCode(error.StatusCode, error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google test event creation failed for user {UserId}", currentUserIdResult.Data);
                var error = ApiResponse<GoogleTestEventDto>.ErrorResult(
                    _localizationService.GetLocalizedString("GoogleIntegrationController.GoogleCalendarTestEventCouldNotBeCreated"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);

                return StatusCode(error.StatusCode, error);
            }
        }

        private string BuildFrontendRedirect(bool connected, string? error = null)
        {
            var baseUrl = _configuration["FrontendSettings:BaseUrl"]?.TrimEnd('/');
            var callbackPath = "/settings/integrations/google";
            var redirectBase = string.IsNullOrWhiteSpace(baseUrl)
                ? callbackPath
                : $"{baseUrl}{callbackPath}";

            var query = new Dictionary<string, string?>
            {
                ["connected"] = connected ? "1" : "0",
            };

            if (!connected && !string.IsNullOrWhiteSpace(error))
            {
                query["error"] = error;
            }

            return QueryHelpers.AddQueryString(redirectBase, query!);
        }

        private string? TryDecrypt(string? encryptedValue)
        {
            if (string.IsNullOrWhiteSpace(encryptedValue))
            {
                return null;
            }

            try
            {
                return _encryptionService.Decrypt(encryptedValue);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to decrypt token during Google disconnect.");
                return null;
            }
        }
    }
}
