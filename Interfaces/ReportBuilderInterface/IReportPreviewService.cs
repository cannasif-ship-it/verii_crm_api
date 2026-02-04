using crm_api.DTOs;
using crm_api.DTOs.ReportBuilderDto;

namespace crm_api.Interfaces
{
    public interface IReportPreviewService
    {
        Task<ApiResponse<PreviewResponseDto>> PreviewAsync(PreviewRequestDto request);
    }
}
