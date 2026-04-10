using System.Threading.Tasks;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    public interface IReportPdfGeneratorService
    {
        Task<byte[]> GeneratePdfAsync(DocumentRuleType ruleType, long entityId, ReportTemplateData templateData);
    }
}
