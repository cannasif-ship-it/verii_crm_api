using System.Threading.Tasks;

namespace crm_api.Modules.PdfBuilder.Application.Services
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
