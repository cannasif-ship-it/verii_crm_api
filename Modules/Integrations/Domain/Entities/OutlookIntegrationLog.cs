namespace crm_api.Modules.Integrations.Domain.Entities
{
    public class OutlookIntegrationLog : BaseEntity
    {
        public Guid TenantId { get; set; }
        public long? UserId { get; set; }
        public User? User { get; set; }
        public string Operation { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string Severity { get; set; } = "Info";
        public string Provider { get; set; } = "Outlook";
        public string? Message { get; set; }
        public string? ErrorCode { get; set; }
        public string? ActivityId { get; set; }
        public string? ProviderEventId { get; set; }
        public string? MetadataJson { get; set; }
    }
}
