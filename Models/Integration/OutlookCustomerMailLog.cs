namespace crm_api.Models
{
    public class OutlookCustomerMailLog : BaseEntity
    {
        public Guid TenantId { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public long? ContactId { get; set; }
        public Contact? Contact { get; set; }
        public long SentByUserId { get; set; }
        public User SentByUser { get; set; } = null!;
        public string Provider { get; set; } = "OutlookGraphApi";
        public string? SenderEmail { get; set; }
        public string ToEmails { get; set; } = string.Empty;
        public string? CcEmails { get; set; }
        public string? BccEmails { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
        public string? TemplateKey { get; set; }
        public string? TemplateName { get; set; }
        public string? TemplateVersion { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? OutlookMessageId { get; set; }
        public string? OutlookConversationId { get; set; }
        public DateTimeOffset? SentAt { get; set; }
        public string? MetadataJson { get; set; }
    }
}
