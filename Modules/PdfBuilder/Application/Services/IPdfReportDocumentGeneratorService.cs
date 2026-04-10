using System.Threading.Tasks;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    /// <summary>
    /// PDF report document generator (report-builder discipline). Renders template + entity data to PDF bytes.
    /// </summary>
    public interface IPdfReportDocumentGeneratorService
    {
        Task<byte[]> GeneratePdfAsync(DocumentRuleType ruleType, long entityId, ReportTemplateData templateData);
    }
}
