using crm_api.DTOs;
using crm_api.Interfaces;

namespace crm_api.Services
{
    public class OutlookEntegrationService : IOutlookEntegrationService
    {
        private readonly ILogger<OutlookEntegrationService> _logger;
        private readonly ILocalizationService _localizationService;

        public OutlookEntegrationService(
            ILogger<OutlookEntegrationService> logger,
            ILocalizationService localizationService)
        {
            _logger = logger;
            _localizationService = localizationService;
        }

        public Task<ApiResponse<OutlookEntegrationAuthorizeUrlDto>> CreateConnectUrlAsync(long userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration CreateConnectUrl called for UserId={UserId}", userId);

            var dto = new OutlookEntegrationAuthorizeUrlDto
            {
                Url = string.Empty,
            };

            var response = ApiResponse<OutlookEntegrationAuthorizeUrlDto>.SuccessResult(
                dto,
                _localizationService.GetLocalizedString("OutlookEntegrationService.ConnectionUrlPlaceholderCreated"));

            return Task.FromResult(response);
        }

        public Task<ApiResponse<bool>> HandleOAuthCallbackAsync(long userId, string code, string state, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration HandleOAuthCallback called for UserId={UserId}", userId);

            return Task.FromResult(ApiResponse<bool>.ErrorResult(
                _localizationService.GetLocalizedString("OutlookEntegrationService.OAuthCallbackPending"),
                _localizationService.GetLocalizedString("General.NotImplementedYet"),
                StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<OutlookEntegrationStatusDto>> GetStatusAsync(long userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration GetStatus called for UserId={UserId}", userId);

            var dto = new OutlookEntegrationStatusDto
            {
                IsConnected = false,
                IsOAuthConfigured = false,
                OutlookEmail = null,
                Scopes = null,
                ExpiresAt = null,
            };

            return Task.FromResult(ApiResponse<OutlookEntegrationStatusDto>.SuccessResult(
                dto,
                _localizationService.GetLocalizedString("OutlookEntegrationService.StatusPlaceholder")));
        }

        public Task<ApiResponse<bool>> DisconnectAsync(long userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration Disconnect called for UserId={UserId}", userId);

            return Task.FromResult(ApiResponse<bool>.ErrorResult(
                _localizationService.GetLocalizedString("OutlookEntegrationService.DisconnectPending"),
                _localizationService.GetLocalizedString("General.NotImplementedYet"),
                StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<OutlookMailSendResultDto>> SendMailAsync(long userId, SendOutlookMailDto dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration SendMail called for UserId={UserId}", userId);

            return Task.FromResult(ApiResponse<OutlookMailSendResultDto>.ErrorResult(
                _localizationService.GetLocalizedString("OutlookEntegrationService.MailSendPending"),
                _localizationService.GetLocalizedString("General.NotImplementedYet"),
                StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<OutlookCalendarEventResultDto>> CreateCalendarEventAsync(long userId, CreateOutlookCalendarEventDto dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration CreateCalendarEvent called for UserId={UserId}", userId);

            return Task.FromResult(ApiResponse<OutlookCalendarEventResultDto>.ErrorResult(
                _localizationService.GetLocalizedString("OutlookEntegrationService.CalendarCreatePending"),
                _localizationService.GetLocalizedString("General.NotImplementedYet"),
                StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<OutlookCalendarEventResultDto>> UpdateCalendarEventAsync(long userId, string eventId, UpdateOutlookCalendarEventDto dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration UpdateCalendarEvent called for UserId={UserId}, EventId={EventId}", userId, eventId);

            return Task.FromResult(ApiResponse<OutlookCalendarEventResultDto>.ErrorResult(
                _localizationService.GetLocalizedString("OutlookEntegrationService.CalendarUpdatePending"),
                _localizationService.GetLocalizedString("General.NotImplementedYet"),
                StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<bool>> DeleteCalendarEventAsync(long userId, string eventId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration DeleteCalendarEvent called for UserId={UserId}, EventId={EventId}", userId, eventId);

            return Task.FromResult(ApiResponse<bool>.ErrorResult(
                _localizationService.GetLocalizedString("OutlookEntegrationService.CalendarDeletePending"),
                _localizationService.GetLocalizedString("General.NotImplementedYet"),
                StatusCodes.Status501NotImplemented));
        }

        public Task<ApiResponse<PagedResponse<OutlookEntegrationLogDto>>> GetLogsAsync(long userId, OutlookEntegrationLogsQueryDto query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("OutlookEntegration GetLogs called for UserId={UserId}", userId);

            var paged = new PagedResponse<OutlookEntegrationLogDto>
            {
                Items = new List<OutlookEntegrationLogDto>(),
                TotalCount = 0,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
            };

            return Task.FromResult(ApiResponse<PagedResponse<OutlookEntegrationLogDto>>.SuccessResult(
                paged,
                _localizationService.GetLocalizedString("OutlookEntegrationService.LogsPlaceholder")));
        }
    }
}
