using System.Threading.Tasks;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;

namespace crm_api.Services
{
    /// <summary>
    /// Legacy adapter: delegates to IPdfReportDocumentGeneratorService for backward compatibility.
    /// </summary>
    public class ReportPdfGeneratorService : IReportPdfGeneratorService
    {
        private readonly IPdfReportDocumentGeneratorService _pdfDocumentGenerator;

        public ReportPdfGeneratorService(IPdfReportDocumentGeneratorService pdfDocumentGenerator)
        {
            _pdfDocumentGenerator = pdfDocumentGenerator;
        }

        public Task<byte[]> GeneratePdfAsync(DocumentRuleType ruleType, long entityId, ReportTemplateData templateData)
        {
            return _pdfDocumentGenerator.GeneratePdfAsync(ruleType, entityId, templateData);
        }
    }
}
