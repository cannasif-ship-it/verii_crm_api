using System.Threading.Tasks;
using crm_api.DTOs;
using crm_api.Models;

namespace crm_api.Interfaces
{
    public interface IReportPdfGeneratorService
    {
        Task<byte[]> GeneratePdfAsync(DocumentRuleType ruleType, long entityId, ReportTemplateData templateData);
    }
}
