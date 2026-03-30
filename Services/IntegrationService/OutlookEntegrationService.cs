using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Infrastructure;
using crm_api.Interfaces;
using crm_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace crm_api.Services
{
    public class OutlookEntegrationService : IOutlookEntegrationService
    {
        private const string ProviderName = "OutlookGraphApi";
        private const string AuthorizationEndpointTemplate = "https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize";
        private const string TokenEndpointTemplate = "https://login.microsoftonline.com/{0}/oauth2/v2.0/token";
        private const string GraphMeEndpoint = "https://graph.microsoft.com/v1.0/me?$select=mail,userPrincipalName,displayName";
        private const string GraphSendMailEndpoint = "https://graph.microsoft.com/v1.0/me/sendMail";
        private const string MailSendScope = "Mail.Send";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        private static readonly IReadOnlyDictionary<string, string> OperationalLogColumnMapping =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = "Id",
                ["userId"] = "UserId",
                ["operation"] = "Operation",
                ["isSuccess"] = "IsSuccess",
                ["severity"] = "Severity",
                ["provider"] = "Provider",
                ["message"] = "Message",
                ["errorCode"] = "ErrorCode",
                ["activityId"] = "ActivityId",
                ["providerEventId"] = "ProviderEventId",
                ["createdDate"] = "CreatedDate",
            };

        private static readonly IReadOnlyDictionary<string, string> CustomerMailLogColumnMapping =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = "Id",
                ["customerId"] = "CustomerId",
                ["contactId"] = "ContactId",
                ["sentByUserId"] = "SentByUserId",
                ["provider"] = "Provider",
                ["toEmails"] = "ToEmails",
                ["subject"] = "Subject",
                ["isSuccess"] = "IsSuccess",
                ["errorCode"] = "ErrorCode",
                ["outlookMessageId"] = "OutlookMessageId",
                ["outlookConversationId"] = "OutlookConversationId",
                ["sentAt"] = "SentAt",
                ["createdDate"] = "CreatedDate",
            };

        private readonly CmsDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserContextService _userContextService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly AzureAdSettings _azureAdSettings;
        private readonly ILogger<OutlookEntegrationService> _logger;
        private readonly ILocalizationService _localizationService;

        public OutlookEntegrationService(
            CmsDbContext dbContext,
            IEncryptionService encryptionService,
            IUserContextService userContextService,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache,
            IOptions<AzureAdSettings> azureAdOptions,
            ILogger<OutlookEntegrationService> logger,
            ILocalizationService localizationService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _userContextService = userContextService;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _azureAdSettings = azureAdOptions.Value;
            _logger = logger;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<OutlookEntegrationAuthorizeUrlDto>> CreateConnectUrlAsync(long userId, CancellationToken cancellationToken = default)
        {
            if (!TryGetSettings(out var settingsError))
            {
                return ApiResponse<OutlookEntegrationAuthorizeUrlDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OutlookEntegrationService.OAuthNotConfigured"),
                    settingsError,
                    StatusCodes.Status400BadRequest);
            }

            var tenantId = _userContextService.GetCurrentTenantId() ?? Guid.Empty;
            if (tenantId == Guid.Empty)
            {
                var tenantMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.TenantContextMissing");
                return ApiResponse<OutlookEntegrationAuthorizeUrlDto>.ErrorResult(tenantMsg, tenantMsg, StatusCodes.Status400BadRequest);
            }

            var state = BuildState(userId, tenantId);
            _memoryCache.Set(BuildStateCacheKey(userId, state), true, TimeSpan.FromMinutes(10));

            var query = new Dictionary<string, string?>
            {
                ["client_id"] = _azureAdSettings.ClientId,
                ["response_type"] = "code",
                ["redirect_uri"] = ResolveRedirectUri(),
                ["response_mode"] = "query",
                ["scope"] = NormalizeScopes(_azureAdSettings.Scopes),
                ["state"] = state,
                ["prompt"] = "select_account",
            };

            var url = QueryHelpers.AddQueryString(
                string.Format(AuthorizationEndpointTemplate, _azureAdSettings.TenantId.Trim()),
                query!);

            await WriteOperationalLogAsync(
                tenantId,
                userId,
                true,
                "Info",
                "outlook.oauth.authorize-url",
                "Outlook authorize URL created.",
                null,
                cancellationToken).ConfigureAwait(false);

            return ApiResponse<OutlookEntegrationAuthorizeUrlDto>.SuccessResult(
                new OutlookEntegrationAuthorizeUrlDto { Url = url },
                _localizationService.GetLocalizedString("OutlookEntegrationService.AuthorizeUrlCreated"));
        }

        public async Task<ApiResponse<bool>> HandleOAuthCallbackAsync(string? code, string? state, string? error, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("OutlookEntegrationService.ConnectionFailed"),
                    error,
                    StatusCodes.Status400BadRequest);
            }

            if (!TryExtractStateContext(state, out var userId, out var tenantId))
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("OutlookEntegrationService.ConnectionFailed"),
                    "invalid_state",
                    StatusCodes.Status400BadRequest);
            }

            if (!_memoryCache.TryGetValue(BuildStateCacheKey(userId, state!), out _))
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("OutlookEntegrationService.ConnectionFailed"),
                    "state_expired",
                    StatusCodes.Status400BadRequest);
            }

            _memoryCache.Remove(BuildStateCacheKey(userId, state!));

            if (string.IsNullOrWhiteSpace(code))
            {
                await WriteOperationalLogAsync(tenantId, userId, false, "Warning", "outlook.oauth.callback", "Outlook callback code was missing.", "missing_code", cancellationToken).ConfigureAwait(false);
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("OutlookEntegrationService.ConnectionFailed"), "missing_code", StatusCodes.Status400BadRequest);
            }

            try
            {
                var token = await ExchangeCodeForTokensAsync(code, cancellationToken).ConfigureAwait(false);
                var me = await GetOutlookProfileAsync(token.AccessToken, cancellationToken).ConfigureAwait(false);
                await UpsertConnectionAsync(userId, tenantId, me?.Email, token.AccessToken, token.RefreshToken, token.Scope, token.ExpiresInSeconds, cancellationToken).ConfigureAwait(false);
                await WriteOperationalLogAsync(tenantId, userId, true, "Info", "outlook.oauth.callback", "Outlook OAuth callback completed successfully.", null, cancellationToken).ConfigureAwait(false);
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OutlookEntegrationService.AccountConnected"));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Outlook OAuth callback failed for user {UserId}", userId);
                await WriteOperationalLogAsync(tenantId, userId, false, "Error", "outlook.oauth.callback", "Outlook OAuth callback failed.", "oauth_callback_failed", cancellationToken, ex.Message).ConfigureAwait(false);
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("OutlookEntegrationService.ConnectionFailed"), ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<ApiResponse<OutlookEntegrationStatusDto>> GetStatusAsync(long userId, CancellationToken cancellationToken = default)
        {
            var account = await _dbContext.UserOutlookAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);
            var dto = new OutlookEntegrationStatusDto
            {
                IsConnected = account?.IsConnected == true,
                IsOAuthConfigured = TryGetSettings(out _),
                OutlookEmail = account?.OutlookEmail,
                Scopes = account?.Scopes,
                ExpiresAt = account?.ExpiresAt,
            };

            return ApiResponse<OutlookEntegrationStatusDto>.SuccessResult(dto, _localizationService.GetLocalizedString("OutlookEntegrationService.StatusRetrieved"));
        }

        public async Task<ApiResponse<bool>> DisconnectAsync(long userId, CancellationToken cancellationToken = default)
        {
            var account = await _dbContext.UserOutlookAccounts.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);
            if (account == null)
            {
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OutlookEntegrationService.AlreadyDisconnected"));
            }

            account.IsConnected = false;
            account.AccessTokenEncrypted = null;
            account.RefreshTokenEncrypted = null;
            account.ExpiresAt = null;
            account.Scopes = null;
            account.UpdatedAt = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await WriteOperationalLogAsync(account.TenantId, userId, true, "Info", "outlook.oauth.disconnect", "Outlook connection removed.", null, cancellationToken).ConfigureAwait(false);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OutlookEntegrationService.ConnectionRemoved"));
        }

        public async Task<ApiResponse<OutlookMailSendResultDto>> SendMailAsync(long userId, SendOutlookMailDto dto, CancellationToken cancellationToken = default)
        {
            var validationError = ValidateSendRequest(dto);
            if (validationError != null)
            {
                return validationError;
            }

            var tenantId = _userContextService.GetCurrentTenantId() ?? Guid.Empty;
            if (tenantId == Guid.Empty)
            {
                var tenantMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.TenantContextMissing");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(tenantMsg, tenantMsg, StatusCodes.Status400BadRequest);
            }

            var customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.CustomerId!.Value && !x.IsDeleted, cancellationToken).ConfigureAwait(false);
            if (customer == null)
            {
                var custMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.CustomerNotFound");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(custMsg, custMsg, StatusCodes.Status404NotFound);
            }

            Contact? contact = null;
            if (dto.ContactId.HasValue)
            {
                contact = await _dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.ContactId.Value && !x.IsDeleted, cancellationToken).ConfigureAwait(false);
                if (contact == null)
                {
                    var contMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.ContactNotFound");
                    return ApiResponse<OutlookMailSendResultDto>.ErrorResult(contMsg, contMsg, StatusCodes.Status404NotFound);
                }

                if (contact.CustomerId != customer.Id)
                {
                    var belongMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.ContactNotBelongToCustomer");
                    return ApiResponse<OutlookMailSendResultDto>.ErrorResult(belongMsg, belongMsg, StatusCodes.Status400BadRequest);
                }
            }

            var toRecipients = ParseEmails(dto.To);
            if (toRecipients.Valid.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(contact?.Email))
                {
                    toRecipients.Valid.Add(contact.Email.Trim());
                }
                else if (!string.IsNullOrWhiteSpace(customer.Email))
                {
                    toRecipients.Valid.Add(customer.Email.Trim());
                }
            }

            if (toRecipients.Valid.Count == 0)
            {
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OutlookEntegrationService.RecipientEmailRequired"),
                    _localizationService.GetLocalizedString("OutlookEntegrationService.NoRecipientEmailFound"),
                    StatusCodes.Status400BadRequest);
            }

            var ccRecipients = ParseEmails(dto.Cc);
            var bccRecipients = ParseEmails(dto.Bcc);
            var invalidRecipients = toRecipients.Invalid
                .Concat(ccRecipients.Invalid)
                .Concat(bccRecipients.Invalid)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (invalidRecipients.Count > 0)
            {
                var invalidMessage = $"Geçersiz e-posta adresleri: {string.Join(", ", invalidRecipients)}";
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(
                    invalidMessage,
                    invalidMessage,
                    StatusCodes.Status400BadRequest);
            }
            var account = await _dbContext.UserOutlookAccounts.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);

            if (account == null || !account.IsConnected)
            {
                await WriteOperationalLogAsync(tenantId, userId, false, "Warning", "outlook.mail.send", "Outlook account is not connected.", "account_not_connected", cancellationToken).ConfigureAwait(false);
                var notConnMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.AccountNotConnected");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(notConnMsg, notConnMsg, StatusCodes.Status400BadRequest);
            }

            if (!ScopeContains(NormalizeScopes(account.Scopes), MailSendScope))
            {
                var scopeMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.MailSendScopeMissing");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(scopeMsg, scopeMsg, StatusCodes.Status400BadRequest);
            }

            var accessToken = await GetValidAccessTokenAsync(account, cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                var tokenMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.TokenInvalidOrExpired");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(tokenMsg, tokenMsg, StatusCodes.Status400BadRequest);
            }

            var response = await SendViaGraphAsync(accessToken, dto, toRecipients.Valid, ccRecipients.Valid, bccRecipients.Valid, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                var failedLog = await WriteCustomerMailLogAsync(tenantId, userId, customer, contact, account.OutlookEmail, toRecipients.Valid, ccRecipients.Valid, bccRecipients.Valid, dto, false, response.ErrorCode, response.ErrorMessage, response.MessageId, response.ConversationId, null, cancellationToken).ConfigureAwait(false);
                await WriteOperationalLogAsync(tenantId, userId, false, "Error", "outlook.mail.send", "Outlook mail send failed.", response.ErrorCode, cancellationToken, new { customerId = dto.CustomerId, logId = failedLog.Id }).ConfigureAwait(false);
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OutlookEntegrationService.MailSendFailed"),
                    response.ErrorMessage ?? _localizationService.GetLocalizedString("OutlookEntegrationService.MailSendFailed"),
                    StatusCodes.Status400BadRequest);
            }

            var sentAt = DateTimeOffset.UtcNow;
            var successLog = await WriteCustomerMailLogAsync(tenantId, userId, customer, contact, account.OutlookEmail, toRecipients.Valid, ccRecipients.Valid, bccRecipients.Valid, dto, true, null, null, response.MessageId, response.ConversationId, sentAt, cancellationToken).ConfigureAwait(false);
            long? activityId = null;
            if (dto.CreateActivityLog)
            {
                activityId = await CreateCustomerMailActivityAsync(
                    userId,
                    customer,
                    contact,
                    dto,
                    toRecipients.Valid,
                    ccRecipients.Valid,
                    bccRecipients.Valid,
                    successLog.Id,
                    sentAt,
                    cancellationToken).ConfigureAwait(false);
            }
            await WriteOperationalLogAsync(tenantId, userId, true, "Info", "outlook.mail.send", "Outlook mail sent.", null, cancellationToken, new { customerId = dto.CustomerId, logId = successLog.Id }).ConfigureAwait(false);

            return ApiResponse<OutlookMailSendResultDto>.SuccessResult(
                new OutlookMailSendResultDto
                {
                    IsSuccess = true,
                    MessageId = response.MessageId,
                    ConversationId = response.ConversationId,
                    SentAt = sentAt,
                    LogId = successLog.Id,
                    ActivityId = activityId
                },
                _localizationService.GetLocalizedString("OutlookEntegrationService.MailSentSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<OutlookCustomerMailLogDto>>> GetCustomerMailLogsAsync(long userId, OutlookCustomerMailLogQueryDto query, CancellationToken cancellationToken = default)
        {
            var tenantId = _userContextService.GetCurrentTenantId() ?? Guid.Empty;
            if (tenantId == Guid.Empty)
            {
                var tenantMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.TenantContextMissing");
                return ApiResponse<PagedResponse<OutlookCustomerMailLogDto>>.ErrorResult(tenantMsg, tenantMsg, StatusCodes.Status400BadRequest);
            }

            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = Math.Clamp(query.PageSize, 1, 100);

            IQueryable<OutlookCustomerMailLog> mailLogQuery = _dbContext.OutlookCustomerMailLogs
                .Where(x => x.TenantId == tenantId)
                .Include(x => x.Customer)
                .Include(x => x.Contact)
                .Include(x => x.SentByUser);

            if (query.CustomerId.HasValue && query.CustomerId.Value > 0)
            {
                mailLogQuery = mailLogQuery.Where(x => x.CustomerId == query.CustomerId.Value);
            }

            if (query.ErrorsOnly)
            {
                mailLogQuery = mailLogQuery.Where(x => !x.IsSuccess);
            }

            mailLogQuery = mailLogQuery.ApplyFilters(query.Filters, query.FilterLogic, CustomerMailLogColumnMapping);
            var sortBy = string.IsNullOrWhiteSpace(query.SortBy) ? "createdDate" : query.SortBy;
            var sortDirection = string.IsNullOrWhiteSpace(query.SortDirection) ? "desc" : query.SortDirection;
            mailLogQuery = mailLogQuery.ApplySorting(sortBy, sortDirection, CustomerMailLogColumnMapping);

            var totalCount = await mailLogQuery.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await mailLogQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new OutlookCustomerMailLogDto
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                CustomerName = x.Customer.CustomerName,
                ContactId = x.ContactId,
                ContactName = x.Contact != null ? x.Contact.FullName : null,
                SentByUserId = x.SentByUserId,
                SentByUserName = x.SentByUser != null ? x.SentByUser.FullName : null,
                Provider = x.Provider,
                SenderEmail = x.SenderEmail,
                ToEmails = x.ToEmails,
                CcEmails = x.CcEmails,
                BccEmails = x.BccEmails,
                Subject = x.Subject,
                Body = x.Body,
                BodyPreview = BuildPreview(x.Body, 300),
                IsHtml = x.IsHtml,
                TemplateKey = x.TemplateKey,
                TemplateName = x.TemplateName,
                TemplateVersion = x.TemplateVersion,
                IsSuccess = x.IsSuccess,
                ErrorCode = x.ErrorCode,
                ErrorMessage = x.ErrorMessage,
                OutlookMessageId = x.OutlookMessageId,
                OutlookConversationId = x.OutlookConversationId,
                SentAt = x.SentAt,
                CreatedDate = x.CreatedDate
            }).ToListAsync(cancellationToken).ConfigureAwait(false);

            return ApiResponse<PagedResponse<OutlookCustomerMailLogDto>>.SuccessResult(
                new PagedResponse<OutlookCustomerMailLogDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                _localizationService.GetLocalizedString("OutlookEntegrationService.OutlookMailLogsRetrieved"));
        }

        public Task<ApiResponse<OutlookCalendarEventResultDto>> CreateCalendarEventAsync(long userId, CreateOutlookCalendarEventDto dto, CancellationToken cancellationToken = default)
        {
            var msg = _localizationService.GetLocalizedString("OutlookEntegrationService.CalendarSyncNotImplemented");
            return Task.FromResult(ApiResponse<OutlookCalendarEventResultDto>.ErrorResult(msg, msg, StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<OutlookCalendarEventResultDto>> UpdateCalendarEventAsync(long userId, string eventId, UpdateOutlookCalendarEventDto dto, CancellationToken cancellationToken = default)
        {
            var msg = _localizationService.GetLocalizedString("OutlookEntegrationService.CalendarSyncNotImplemented");
            return Task.FromResult(ApiResponse<OutlookCalendarEventResultDto>.ErrorResult(msg, msg, StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<bool>> DeleteCalendarEventAsync(long userId, string eventId, CancellationToken cancellationToken = default)
        {
            var msg = _localizationService.GetLocalizedString("OutlookEntegrationService.CalendarSyncNotImplemented");
            return Task.FromResult(ApiResponse<bool>.ErrorResult(msg, msg, StatusCodes.Status501NotImplemented));
        }

        public async Task<ApiResponse<PagedResponse<OutlookEntegrationLogDto>>> GetLogsAsync(long userId, OutlookEntegrationLogsQueryDto query, CancellationToken cancellationToken = default)
        {
            var tenantId = _userContextService.GetCurrentTenantId() ?? Guid.Empty;
            if (tenantId == Guid.Empty)
            {
                var tenantMsg = _localizationService.GetLocalizedString("OutlookEntegrationService.TenantContextMissing");
                return ApiResponse<PagedResponse<OutlookEntegrationLogDto>>.ErrorResult(tenantMsg, tenantMsg, StatusCodes.Status400BadRequest);
            }

            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = Math.Clamp(query.PageSize, 1, 100);

            IQueryable<OutlookIntegrationLog> logQuery = _dbContext.OutlookIntegrationLogs.Where(x => x.TenantId == tenantId);
            if (query.ErrorsOnly)
            {
                logQuery = logQuery.Where(x => !x.IsSuccess);
            }

            logQuery = logQuery.ApplyFilters(query.Filters, query.FilterLogic, OperationalLogColumnMapping);
            var sortBy = string.IsNullOrWhiteSpace(query.SortBy) ? "createdDate" : query.SortBy;
            var sortDirection = string.IsNullOrWhiteSpace(query.SortDirection) ? "desc" : query.SortDirection;
            logQuery = logQuery.ApplySorting(sortBy, sortDirection, OperationalLogColumnMapping);

            var totalCount = await logQuery.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await logQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new OutlookEntegrationLogDto
            {
                Id = x.Id,
                UserId = x.UserId ?? 0,
                Operation = x.Operation,
                IsSuccess = x.IsSuccess,
                Severity = x.Severity,
                Provider = x.Provider,
                Message = x.Message,
                ErrorCode = x.ErrorCode,
                ActivityId = x.ActivityId,
                ProviderEventId = x.ProviderEventId,
                CreatedDate = x.CreatedDate
            }).ToListAsync(cancellationToken).ConfigureAwait(false);

            return ApiResponse<PagedResponse<OutlookEntegrationLogDto>>.SuccessResult(
                new PagedResponse<OutlookEntegrationLogDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                _localizationService.GetLocalizedString("OutlookEntegrationService.IntegrationLogsRetrieved"));
        }

        private ApiResponse<OutlookMailSendResultDto>? ValidateSendRequest(SendOutlookMailDto dto)
        {
            if (!dto.CustomerId.HasValue || dto.CustomerId.Value <= 0)
            {
                var msg = _localizationService.GetLocalizedString("OutlookEntegrationService.CustomerIdRequired");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(msg, msg, StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.Subject))
            {
                var msg = _localizationService.GetLocalizedString("OutlookEntegrationService.MailSubjectRequired");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(msg, msg, StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.Body))
            {
                var msg = _localizationService.GetLocalizedString("OutlookEntegrationService.MailBodyRequired");
                return ApiResponse<OutlookMailSendResultDto>.ErrorResult(msg, msg, StatusCodes.Status400BadRequest);
            }

            return null;
        }

        private async Task<OutlookGraphTokenResult> ExchangeCodeForTokensAsync(string code, CancellationToken cancellationToken)
        {
            return await SendTokenRequestAsync(new Dictionary<string, string>
            {
                ["client_id"] = _azureAdSettings.ClientId.Trim(),
                ["client_secret"] = _azureAdSettings.ClientSecret.Trim(),
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = ResolveRedirectUri(),
                ["scope"] = NormalizeScopes(_azureAdSettings.Scopes),
            }, cancellationToken).ConfigureAwait(false);
        }

        private async Task<OutlookGraphTokenResult> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return await SendTokenRequestAsync(new Dictionary<string, string>
            {
                ["client_id"] = _azureAdSettings.ClientId.Trim(),
                ["client_secret"] = _azureAdSettings.ClientSecret.Trim(),
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["redirect_uri"] = ResolveRedirectUri(),
                ["scope"] = NormalizeScopes(_azureAdSettings.Scopes),
            }, cancellationToken).ConfigureAwait(false);
        }

        private async Task<OutlookGraphTokenResult> SendTokenRequestAsync(Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, string.Format(TokenEndpointTemplate, _azureAdSettings.TenantId.Trim()))
            {
                Content = new FormUrlEncodedContent(formData)
            };

            using var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? _localizationService.GetLocalizedString("OutlookEntegrationService.TokenRequestFailed") : body);
            }

            var payload = JsonSerializer.Deserialize<OutlookTokenResponse>(body, JsonOptions);
            if (payload == null || string.IsNullOrWhiteSpace(payload.AccessToken))
            {
                throw new InvalidOperationException(_localizationService.GetLocalizedString("OutlookEntegrationService.TokenResponseInvalid"));
            }

            return new OutlookGraphTokenResult
            {
                AccessToken = payload.AccessToken,
                RefreshToken = payload.RefreshToken,
                Scope = payload.Scope,
                ExpiresInSeconds = payload.ExpiresIn
            };
        }

        private async Task<OutlookProfileResponse?> GetOutlookProfileAsync(string accessToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, GraphMeEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<OutlookProfileResponse>(body, JsonOptions);
        }

        private async Task UpsertConnectionAsync(long userId, Guid tenantId, string? outlookEmail, string accessToken, string? refreshToken, string? scopes, int expiresInSeconds, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;
            var account = await _dbContext.UserOutlookAccounts.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);
            if (account == null)
            {
                account = new UserOutlookAccount
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TenantId = tenantId,
                    CreatedAt = now
                };

                await _dbContext.UserOutlookAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
            }

            account.TenantId = tenantId;
            account.OutlookEmail = outlookEmail;
            account.AccessTokenEncrypted = _encryptionService.Encrypt(accessToken);
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                account.RefreshTokenEncrypted = _encryptionService.Encrypt(refreshToken);
            }

            account.ExpiresAt = now.AddSeconds(Math.Max(expiresInSeconds, 60));
            account.Scopes = NormalizeScopes(scopes);
            account.IsConnected = true;
            account.UpdatedAt = now;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task<string?> GetValidAccessTokenAsync(UserOutlookAccount account, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;
            var accessToken = SafeDecrypt(account.AccessTokenEncrypted);
            var shouldRefresh = string.IsNullOrWhiteSpace(accessToken) || !account.ExpiresAt.HasValue || account.ExpiresAt.Value <= now.AddMinutes(1);
            if (!shouldRefresh)
            {
                return accessToken;
            }

            var refreshToken = SafeDecrypt(account.RefreshTokenEncrypted);
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                await DisconnectAsync(account.UserId, cancellationToken).ConfigureAwait(false);
                return null;
            }

            try
            {
                var refreshed = await RefreshAccessTokenAsync(refreshToken, cancellationToken).ConfigureAwait(false);
                account.AccessTokenEncrypted = _encryptionService.Encrypt(refreshed.AccessToken);
                if (!string.IsNullOrWhiteSpace(refreshed.RefreshToken))
                {
                    account.RefreshTokenEncrypted = _encryptionService.Encrypt(refreshed.RefreshToken);
                }

                if (!string.IsNullOrWhiteSpace(refreshed.Scope))
                {
                    account.Scopes = NormalizeScopes(refreshed.Scope);
                }

                account.ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(Math.Max(refreshed.ExpiresInSeconds, 60));
                account.IsConnected = true;
                account.UpdatedAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return refreshed.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Outlook access token refresh failed for user {UserId}", account.UserId);
                await DisconnectAsync(account.UserId, cancellationToken).ConfigureAwait(false);
                return null;
            }
        }

        private async Task<GraphSendMailResult> SendViaGraphAsync(string accessToken, SendOutlookMailDto dto, IReadOnlyList<string> toRecipients, IReadOnlyList<string> ccRecipients, IReadOnlyList<string> bccRecipients, CancellationToken cancellationToken)
        {
            var validAttachments = (dto.Attachments ?? [])
                .Where(x => !string.IsNullOrWhiteSpace(x.FileName) && !string.IsNullOrWhiteSpace(x.Base64Content))
                .Select(CreateAttachment)
                .ToArray();

            var payload = new
            {
                message = new
                {
                    subject = dto.Subject.Trim(),
                    body = new
                    {
                        contentType = dto.IsHtml ? "HTML" : "Text",
                        content = dto.Body
                    },
                    toRecipients = toRecipients.Select(CreateRecipient).ToArray(),
                    ccRecipients = ccRecipients.Select(CreateRecipient).ToArray(),
                    bccRecipients = bccRecipients.Select(CreateRecipient).ToArray(),
                    attachments = validAttachments
                },
                saveToSentItems = true
            };

            var client = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, GraphSendMailEndpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Accepted || response.IsSuccessStatusCode)
            {
                return new GraphSendMailResult
                {
                    IsSuccess = true,
                    MessageId = TryGetHeaderValue(response, "request-id"),
                    ConversationId = TryGetHeaderValue(response, "client-request-id")
                };
            }

            var (errorCode, errorMessage) = ParseGraphError(body, response.StatusCode);
            return new GraphSendMailResult
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }

        private async Task<OutlookCustomerMailLog> WriteCustomerMailLogAsync(Guid tenantId, long userId, Customer customer, Contact? contact, string? senderEmail, IReadOnlyList<string> toRecipients, IReadOnlyList<string> ccRecipients, IReadOnlyList<string> bccRecipients, SendOutlookMailDto dto, bool isSuccess, string? errorCode, string? errorMessage, string? outlookMessageId, string? outlookConversationId, DateTimeOffset? sentAt, CancellationToken cancellationToken)
        {
            var entity = new OutlookCustomerMailLog
            {
                TenantId = tenantId,
                CustomerId = customer.Id,
                ContactId = contact?.Id,
                SentByUserId = userId,
                Provider = ProviderName,
                SenderEmail = TrimOrNull(senderEmail, 320),
                ToEmails = TrimOrNull(string.Join("; ", toRecipients), 4000) ?? string.Empty,
                CcEmails = TrimOrNull(string.Join("; ", ccRecipients), 4000),
                BccEmails = TrimOrNull(string.Join("; ", bccRecipients), 4000),
                Subject = TrimOrNull(dto.Subject, 512) ?? string.Empty,
                Body = dto.Body,
                IsHtml = dto.IsHtml,
                TemplateKey = TrimOrNull(dto.TemplateKey, 128),
                TemplateName = TrimOrNull(dto.TemplateName, 256),
                TemplateVersion = TrimOrNull(dto.TemplateVersion, 64),
                IsSuccess = isSuccess,
                ErrorCode = TrimOrNull(errorCode, 128),
                ErrorMessage = TrimOrNull(errorMessage, 2000),
                OutlookMessageId = TrimOrNull(outlookMessageId, 512),
                OutlookConversationId = TrimOrNull(outlookConversationId, 512),
                SentAt = sentAt,
                MetadataJson = JsonSerializer.Serialize(new
                {
                    customerName = customer.CustomerName,
                    contactName = contact?.FullName,
                    moduleKey = dto.ModuleKey,
                    recordId = dto.RecordId,
                    recordNo = dto.RecordNo,
                    revisionNo = dto.RevisionNo,
                    customerCode = dto.CustomerCode,
                    totalAmountDisplay = dto.TotalAmountDisplay,
                    validUntil = dto.ValidUntil,
                    recordOwnerName = dto.RecordOwnerName,
                    contextTitle = dto.ContextTitle,
                    attachments = dto.Attachments
                        .Where(x => !string.IsNullOrWhiteSpace(x.FileName))
                        .Select(x => new { x.FileName, x.ContentType })
                })
            };

            await _dbContext.OutlookCustomerMailLogs.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        private async Task WriteOperationalLogAsync(Guid tenantId, long? userId, bool isSuccess, string severity, string operation, string message, string? errorCode, CancellationToken cancellationToken, object? metadata = null)
        {
            var entity = new OutlookIntegrationLog
            {
                TenantId = tenantId,
                UserId = userId,
                IsSuccess = isSuccess,
                Severity = severity,
                Operation = operation,
                Provider = ProviderName,
                Message = message,
                ErrorCode = errorCode,
                MetadataJson = metadata == null ? null : JsonSerializer.Serialize(metadata)
            };

            await _dbContext.OutlookIntegrationLogs.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private bool TryGetSettings(out string? error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(_azureAdSettings.TenantId))
            {
                error = "AzureAd:TenantId is missing.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_azureAdSettings.ClientId))
            {
                error = "AzureAd:ClientId is missing.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_azureAdSettings.ClientSecret))
            {
                error = "AzureAd:ClientSecret is missing.";
                return false;
            }

            return true;
        }

        private string ResolveRedirectUri()
        {
            if (!string.IsNullOrWhiteSpace(_azureAdSettings.RedirectUri))
            {
                return _azureAdSettings.RedirectUri.Trim();
            }

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                return $"{request.Scheme}://{request.Host}{request.PathBase}/api/integrations/outlook/callback";
            }

            throw new InvalidOperationException(_localizationService.GetLocalizedString("OutlookEntegrationService.RedirectUriNotResolved"));
        }

        private static string BuildState(long userId, Guid tenantId)
        {
            var randomState = WebEncoders.Base64UrlEncode(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));
            return $"{userId}.{tenantId:D}.{randomState}";
        }

        private static string BuildStateCacheKey(long userId, string state) => $"outlook-oauth-state:{userId}:{state}";

        private static bool TryExtractStateContext(string? state, out long userId, out Guid tenantId)
        {
            userId = 0;
            tenantId = Guid.Empty;

            if (string.IsNullOrWhiteSpace(state))
            {
                return false;
            }

            var parts = state.Split('.', 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3)
            {
                return false;
            }

            return long.TryParse(parts[0], out userId)
                && userId > 0
                && Guid.TryParse(parts[1], out tenantId)
                && tenantId != Guid.Empty;
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
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to decrypt an Outlook token.");
                return null;
            }
        }

        private static bool ScopeContains(string scopeList, string requiredScope)
        {
            if (string.IsNullOrWhiteSpace(scopeList) || string.IsNullOrWhiteSpace(requiredScope))
            {
                return false;
            }

            return scopeList.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Any(x => string.Equals(x, requiredScope, StringComparison.OrdinalIgnoreCase));
        }

        private static string NormalizeScopes(string? scopes)
        {
            return string.IsNullOrWhiteSpace(scopes)
                ? "openid profile email offline_access User.Read Mail.Send Calendars.ReadWrite"
                : string.Join(' ', scopes.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Distinct(StringComparer.OrdinalIgnoreCase));
        }

        private async Task<long?> CreateCustomerMailActivityAsync(
            long userId,
            Customer customer,
            Contact? contact,
            SendOutlookMailDto dto,
            IReadOnlyList<string> toRecipients,
            IReadOnlyList<string> ccRecipients,
            IReadOnlyList<string> bccRecipients,
            long mailLogId,
            DateTimeOffset sentAt,
            CancellationToken cancellationToken)
        {
            var activityTypeId = await ResolveMailActivityTypeIdAsync(cancellationToken).ConfigureAwait(false);
            if (!activityTypeId.HasValue)
            {
                return null;
            }

            var moduleLabel = dto.ModuleKey?.Trim().ToLowerInvariant() switch
            {
                "quotation" => "Teklif",
                "demand" => "Talep",
                "order" => "Sipariş",
                "activity" => "Aktivite",
                _ => "Kayıt"
            };

            var subjectSuffix = !string.IsNullOrWhiteSpace(dto.RecordNo)
                ? dto.RecordNo!.Trim()
                : dto.RecordId.HasValue && dto.RecordId.Value > 0
                    ? $"#{dto.RecordId.Value}"
                    : customer.CustomerName;

            var activity = new Activity
            {
                Subject = $"{moduleLabel} e-postası gönderildi - {subjectSuffix}",
                Description = BuildActivityDescription(customer, contact, dto, toRecipients, ccRecipients, bccRecipients, mailLogId, sentAt),
                ActivityTypeId = activityTypeId.Value,
                StartDateTime = sentAt.LocalDateTime,
                EndDateTime = sentAt.LocalDateTime,
                IsAllDay = false,
                Status = ActivityStatus.Completed,
                Priority = ActivityPriority.Medium,
                AssignedUserId = userId,
                ContactId = contact?.Id,
                PotentialCustomerId = customer.Id,
                ErpCustomerCode = TrimOrNull(dto.CustomerCode, 50) ?? TrimOrNull(customer.CustomerCode, 50),
            };

            await _dbContext.Activities.AddAsync(activity, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return activity.Id;
        }

        private async Task<long?> ResolveMailActivityTypeIdAsync(CancellationToken cancellationToken)
        {
            var activityType = await _dbContext.ActivityTypes.AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x =>
                    x.Name.Contains("mail") ||
                    x.Name.Contains("Mail") ||
                    x.Name.Contains("email") ||
                    x.Name.Contains("Email") ||
                    x.Name.Contains("posta") ||
                    x.Name.Contains("Posta"))
                .ThenBy(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return activityType?.Id;
        }

        private static string BuildActivityDescription(
            Customer customer,
            Contact? contact,
            SendOutlookMailDto dto,
            IReadOnlyList<string> toRecipients,
            IReadOnlyList<string> ccRecipients,
            IReadOnlyList<string> bccRecipients,
            long mailLogId,
            DateTimeOffset sentAt)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Outlook e-posta gönderim kaydı");
            sb.AppendLine($"Gönderim zamanı: {sentAt.LocalDateTime:dd.MM.yyyy HH:mm}");
            sb.AppendLine($"Müşteri: {customer.CustomerName}");

            if (!string.IsNullOrWhiteSpace(dto.CustomerCode) || !string.IsNullOrWhiteSpace(customer.CustomerCode))
            {
                sb.AppendLine($"Cari kodu: {dto.CustomerCode ?? customer.CustomerCode}");
            }

            if (contact != null)
            {
                sb.AppendLine($"Kontak: {contact.FullName}");
            }

            if (!string.IsNullOrWhiteSpace(dto.RecordNo))
            {
                sb.AppendLine($"Belge no: {dto.RecordNo}");
            }

            if (!string.IsNullOrWhiteSpace(dto.RevisionNo))
            {
                sb.AppendLine($"Revize no: {dto.RevisionNo}");
            }

            if (!string.IsNullOrWhiteSpace(dto.TotalAmountDisplay))
            {
                sb.AppendLine($"Toplam: {dto.TotalAmountDisplay}");
            }

            if (!string.IsNullOrWhiteSpace(dto.ValidUntil))
            {
                sb.AppendLine($"Geçerlilik: {dto.ValidUntil}");
            }

            sb.AppendLine($"Konu: {dto.Subject}");
            sb.AppendLine($"Alıcılar: {string.Join("; ", toRecipients)}");

            if (ccRecipients.Count > 0)
            {
                sb.AppendLine($"CC: {string.Join("; ", ccRecipients)}");
            }

            if (bccRecipients.Count > 0)
            {
                sb.AppendLine($"BCC: {string.Join("; ", bccRecipients)}");
            }

            if (dto.Attachments.Count > 0)
            {
                sb.AppendLine($"Ekler: {string.Join(", ", dto.Attachments.Where(x => !string.IsNullOrWhiteSpace(x.FileName)).Select(x => x.FileName))}");
            }

            sb.AppendLine($"Mail log id: {mailLogId}");
            return sb.ToString().Trim();
        }

        private static EmailParseResult ParseEmails(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return new EmailParseResult();
            }

            var pieces = raw.Split(new[] { ';', ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var result = new EmailParseResult();
            foreach (var piece in pieces)
            {
                try
                {
                    var mail = new MailAddress(piece);
                    result.Valid.Add(mail.Address);
                }
                catch
                {
                    result.Invalid.Add(piece);
                }
            }

            return result;
        }

        private static string? BuildPreview(string? value, int maxLen)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var normalized = value.Trim();
            return normalized.Length <= maxLen ? normalized : normalized[..maxLen].TrimEnd() + "...";
        }

        private static string? TrimOrNull(string? value, int maxLen)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var trimmed = value.Trim();
            return trimmed.Length <= maxLen ? trimmed : trimmed[..maxLen];
        }

        private static object CreateRecipient(string email) => new
        {
            emailAddress = new
            {
                address = email
            }
        };

        private static Dictionary<string, object?> CreateAttachment(OutlookCustomerMailAttachmentDto attachment) => new()
        {
            ["@odata.type"] = "#microsoft.graph.fileAttachment",
            ["name"] = attachment.FileName,
            ["contentType"] = string.IsNullOrWhiteSpace(attachment.ContentType) ? "application/octet-stream" : attachment.ContentType,
            ["contentBytes"] = attachment.Base64Content
        };

        private static (string ErrorCode, string ErrorMessage) ParseGraphError(string? body, HttpStatusCode statusCode)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return (((int)statusCode).ToString(), "Outlook Graph request failed.");
            }

            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("error", out var errorElement))
                {
                    var code = errorElement.TryGetProperty("code", out var codeEl) ? codeEl.GetString() : ((int)statusCode).ToString();
                    var message = errorElement.TryGetProperty("message", out var messageEl) ? messageEl.GetString() : body;
                    return (code ?? ((int)statusCode).ToString(), message ?? body);
                }
            }
            catch
            {
            }

            return (((int)statusCode).ToString(), body);
        }

        private static string? TryGetHeaderValue(HttpResponseMessage response, string key)
        {
            return response.Headers.TryGetValues(key, out var values) ? values.FirstOrDefault() : null;
        }

        private sealed class OutlookTokenResponse
        {
            public string AccessToken { get; set; } = string.Empty;
            public string? RefreshToken { get; set; }
            public string? Scope { get; set; }
            public int ExpiresIn { get; set; }
        }

        private sealed class OutlookProfileResponse
        {
            public string? Mail { get; set; }
            public string? UserPrincipalName { get; set; }
            public string? Email => string.IsNullOrWhiteSpace(Mail) ? UserPrincipalName : Mail;
        }

        private sealed class OutlookGraphTokenResult
        {
            public string AccessToken { get; set; } = string.Empty;
            public string? RefreshToken { get; set; }
            public string? Scope { get; set; }
            public int ExpiresInSeconds { get; set; }
        }

        private sealed class EmailParseResult
        {
            public List<string> Valid { get; } = new();
            public List<string> Invalid { get; } = new();
        }

        private sealed class GraphSendMailResult
        {
            public bool IsSuccess { get; set; }
            public string? MessageId { get; set; }
            public string? ConversationId { get; set; }
            public string? ErrorCode { get; set; }
            public string? ErrorMessage { get; set; }
        }
    }
}
