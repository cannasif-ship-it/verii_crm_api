using System.Threading.Tasks;
using crm_api.DTOs;
using crm_api.Models;

namespace crm_api.Interfaces
{
    /// <summary>
    /// PDF report document generator (report-builder discipline). Renders template + entity data to PDF bytes.
    /// </summary>
    public interface IPdfReportDocumentGeneratorService
    {
        Task<byte[]> GeneratePdfAsync(DocumentRuleType ruleType, long entityId, ReportTemplateData templateData);
    }
}
