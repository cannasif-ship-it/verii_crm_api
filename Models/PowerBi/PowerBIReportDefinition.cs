namespace crm_api.Models.PowerBi
{
    public class PowerBIReportDefinition : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        // Power BI identifiers (Power BI tarafındaki GUID'ler string olarak tutulur)
        public string WorkspaceId { get; set; } = default!;
        public string ReportId { get; set; } = default!;

        // İstersen cache olarak tut (zorunlu değil)
        public string? EmbedUrl { get; set; }

        public PowerBIContentType ContentType { get; set; } = PowerBIContentType.Report;

        public bool IsActive { get; set; } = true;

        // Default filters, page, settings vs (opsiyonel)
        public string? DefaultSettingsJson { get; set; }
    }
}
