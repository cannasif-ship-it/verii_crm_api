using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    public interface IPdfTemplateAssetService
    {
        Task<ApiResponse<PdfTemplateAssetDto>> UploadAsync(
            IFormFile file,
            long userId,
            long? templateId = null,
            string? assetScope = null,
            string? elementId = null,
            int? pageNumber = null,
            long? tempQuotattionId = null,
            long? tempQuotattionLineId = null,
            string? productCode = null);
        Task NormalizeTemplateAssetPathsAsync(ReportTemplateData templateData, long userId, long templateId, DocumentRuleType ruleType);
        Task SyncTemplateImageUsagesAsync(ReportTemplateData templateData, long templateId, DocumentRuleType ruleType);
        Task BindQuickQuotationImageAsync(string? relativeUrl, long tempQuotattionId, long tempQuotattionLineId, string? productCode = null);
        Task ClearQuickQuotationImageBindingsForLineAsync(long tempQuotattionLineId);
    }
}
