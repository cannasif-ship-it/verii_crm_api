using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    public interface IPdfTemplateAssetService
    {
        Task<ApiResponse<PdfTemplateAssetDto>> UploadAsync(IFormFile file, long userId, long? templateId = null, string? assetScope = null);
        Task NormalizeTemplateAssetPathsAsync(ReportTemplateData templateData, long userId, long templateId, DocumentRuleType ruleType);
    }
}
