using System.Collections.Generic;
using System.Threading.Tasks;
using crm_api.DTOs;
using crm_api.Models;

namespace crm_api.Interfaces
{
    public interface IReportTemplateService
    {
        Task<ApiResponse<PagedResponse<ReportTemplateDto>>> GetAllAsync(PagedRequest request, DocumentRuleType? ruleType = null, bool? isActive = null);
        Task<ApiResponse<ReportTemplateDto>> GetByIdAsync(long id);
        Task<ApiResponse<ReportTemplateDto>> CreateAsync(CreateReportTemplateDto dto, long userId);
        Task<ApiResponse<ReportTemplateDto>> UpdateAsync(long id, UpdateReportTemplateDto dto, long userId);
        Task<ApiResponse<bool>> DeleteAsync(long id);
        Task<ApiResponse<byte[]>> GeneratePdfAsync(long templateId, long entityId);
    }
}
