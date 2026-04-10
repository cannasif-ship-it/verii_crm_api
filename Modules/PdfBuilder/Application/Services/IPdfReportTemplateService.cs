using System.Collections.Generic;
using System.Threading.Tasks;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    /// <summary>
    /// PDF report template service (report-builder discipline).
    /// </summary>
    public interface IPdfReportTemplateService
    {
        Task<ApiResponse<PagedResponse<PdfReportTemplateDto>>> GetAllAsync(PdfReportTemplateListRequest request);
        Task<ApiResponse<PdfReportTemplateDto>> GetByIdAsync(long id);
        Task<ApiResponse<PdfReportTemplateDto>> CreateAsync(CreatePdfReportTemplateDto dto, long userId);
        Task<ApiResponse<PdfReportTemplateDto>> UpdateAsync(long id, UpdatePdfReportTemplateDto dto, long userId);
        Task<ApiResponse<bool>> DeleteAsync(long id);
        Task<ApiResponse<byte[]>> GeneratePdfAsync(long templateId, long entityId, long? requestingUserId);
    }
}
