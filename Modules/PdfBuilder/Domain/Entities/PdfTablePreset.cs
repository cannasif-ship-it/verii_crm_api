namespace crm_api.Modules.PdfBuilder.Domain.Entities
{
    /// <summary>
    /// Reusable server-side table preset definition for PDF/report designer tables.
    /// </summary>
    public class PdfTablePreset : BaseEntity
    {
        public DocumentRuleType RuleType { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string ColumnsJson { get; set; } = string.Empty;

        public string? TableOptionsJson { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
