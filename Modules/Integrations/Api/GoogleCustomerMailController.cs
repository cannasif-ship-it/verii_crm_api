using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.Integrations.Api
{
    [ApiController]
    [Route("api/customer-mail/google")]
    [Authorize]
    public class GoogleCustomerMailController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleGmailApiService _googleGmailApiService;

        public GoogleCustomerMailController(
            IUserService userService,
            IGoogleGmailApiService googleGmailApiService)
        {
            _userService = userService;
            _googleGmailApiService = googleGmailApiService;
        }

        [HttpPost("send")]
        public async Task<ActionResult<ApiResponse<GoogleCustomerMailSendResultDto>>> Send(
            [FromBody] SendGoogleCustomerMailDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var currentUserIdResult = await _userService.GetCurrentUserIdAsync();
                if (!currentUserIdResult.Success)
                {
                    var error = ApiResponse<GoogleCustomerMailSendResultDto>.ErrorResult(
                        currentUserIdResult.Message,
                        currentUserIdResult.ExceptionMessage,
                        currentUserIdResult.StatusCode);

                    return StatusCode(error.StatusCode, error);
                }

                var result = await _googleGmailApiService.SendCustomerMailAsync(
                    currentUserIdResult.Data,
                    dto,
                    cancellationToken);

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                var error = ApiResponse<GoogleCustomerMailSendResultDto>.ErrorResult(
                    "Mail gönderimi sırasında sunucu hatası oluştu.",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);

                return StatusCode(error.StatusCode, error);
            }
        }

        [HttpGet("logs")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GoogleCustomerMailLogDto>>>> GetLogs(
            [FromQuery] GoogleCustomerMailLogQueryDto query,
            CancellationToken cancellationToken)
        {
            var currentUserIdResult = await _userService.GetCurrentUserIdAsync();
            if (!currentUserIdResult.Success)
            {
                var error = ApiResponse<PagedResponse<GoogleCustomerMailLogDto>>.ErrorResult(
                    currentUserIdResult.Message,
                    currentUserIdResult.ExceptionMessage,
                    currentUserIdResult.StatusCode);

                return StatusCode(error.StatusCode, error);
            }

            var result = await _googleGmailApiService.GetCustomerMailLogsAsync(
                currentUserIdResult.Data,
                query,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }
    }
}
