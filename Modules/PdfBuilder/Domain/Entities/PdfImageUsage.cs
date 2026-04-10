using crm_api.Modules.PdfBuilder.Domain.Enums;

namespace crm_api.Modules.PdfBuilder.Domain.Entities
{
    /// <summary>
    /// Strong ownership record that tells where an uploaded PDF image is used.
    /// </summary>
    public class PdfImageUsage : BaseEntity
    {
        public long PdfTemplateAssetId { get; set; }
        public PdfTemplateAsset PdfTemplateAsset { get; set; } = null!;

        public long ReportTemplateId { get; set; }
        public ReportTemplate ReportTemplate { get; set; } = null!;

        public PdfImageUsageType UsageType { get; set; } = PdfImageUsageType.TemplateElement;

        public string ElementId { get; set; } = string.Empty;

        public int PageNumber { get; set; } = 1;

        public DocumentRuleType RuleType { get; set; }
    }
}
