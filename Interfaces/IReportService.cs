using crm_api.DTOs;
using crm_api.DTOs.ReportBuilderDto;

namespace crm_api.Interfaces
{
    public interface IReportService
    {
        Task<ApiResponse<ReportDetailDto>> GetByIdAsync(long id);
        Task<ApiResponse<PagedResponse<ReportListItemDto>>> ListAsync(string? search, int pageNumber = 1, int pageSize = 20);
        Task<ApiResponse<ReportDetailDto>> CreateAsync(ReportCreateDto dto, long userId);
        Task<ApiResponse<ReportDetailDto>> UpdateAsync(long id, ReportUpdateDto dto, long userId);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
