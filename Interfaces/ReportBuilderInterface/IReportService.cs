using crm_api.DTOs;
using crm_api.DTOs.ReportBuilderDto;

namespace crm_api.Interfaces
{
    public interface IReportService
    {
        Task<ApiResponse<ReportDetailDto>> GetByIdAsync(long id, long userId, string? email);
        Task<ApiResponse<PagedResponse<ReportListItemDto>>> ListAsync(string? search, long userId, string? email, int pageNumber = 1, int pageSize = 20);
        Task<ApiResponse<ReportDetailDto>> CreateAsync(ReportCreateDto dto, long userId);
        Task<ApiResponse<ReportDetailDto>> UpdateAsync(long id, ReportUpdateDto dto, long userId, string? email);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, long userId, string? email);
    }
}
