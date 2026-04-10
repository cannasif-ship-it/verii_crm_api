using System.Collections.Generic;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    /// <summary>
    /// Validates PDF report template data (Create/Update/Generate payloads).
    /// </summary>
    public interface IPdfReportTemplateValidator
    {
        /// <summary>
        /// Validates template data. Returns empty list if valid; otherwise list of error messages.
        /// </summary>
        IReadOnlyList<string> ValidateTemplateData(ReportTemplateData? data, DocumentRuleType ruleType);

        /// <summary>
        /// Returns optional overlap/placement warnings (non-blocking).
        /// </summary>
        IReadOnlyList<string> GetPlacementWarnings(ReportTemplateData? data);

        /// <summary>
        /// Validates for generate-document (template existence and data shape).
        /// </summary>
        IReadOnlyList<string> ValidateForGenerate(ReportTemplateData? data);
    }
}
