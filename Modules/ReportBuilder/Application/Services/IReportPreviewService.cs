using crm_api.Modules.ReportBuilder.Application.Dtos;

namespace crm_api.Modules.ReportBuilder.Application.Services
{
    public interface IReportPreviewService
    {
        Task<ApiResponse<PreviewResponseDto>> PreviewAsync(PreviewRequestDto request, long? currentUserId = null, string? currentUserEmail = null);
    }
}
