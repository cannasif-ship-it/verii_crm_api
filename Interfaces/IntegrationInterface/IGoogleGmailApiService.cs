using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IGoogleGmailApiService
    {
        Task<ApiResponse<GoogleCustomerMailSendResultDto>> SendCustomerMailAsync(
            long userId,
            SendGoogleCustomerMailDto dto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<PagedResponse<GoogleCustomerMailLogDto>>> GetCustomerMailLogsAsync(
            long userId,
            GoogleCustomerMailLogQueryDto query,
            CancellationToken cancellationToken = default);
    }
}

