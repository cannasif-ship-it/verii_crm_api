namespace crm_api.DTOs
{
    public class SendGoogleCustomerMailDto
    {
        public long CustomerId { get; set; }
        public long? ContactId { get; set; }

        public string? To { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }

        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;

        public string? TemplateKey { get; set; }
        public string? TemplateName { get; set; }
        public string? TemplateVersion { get; set; }
    }

    public class GoogleCustomerMailSendResultDto
    {
        public long LogId { get; set; }
        public bool IsSuccess { get; set; }
        public string? GoogleMessageId { get; set; }
        public string? GoogleThreadId { get; set; }
        public DateTimeOffset? SentAt { get; set; }
    }

    public class GoogleCustomerMailLogDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long? ContactId { get; set; }
        public string? ContactName { get; set; }
        public long SentByUserId { get; set; }
        public string? SentByUserName { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string? SenderEmail { get; set; }
        public string ToEmails { get; set; } = string.Empty;
        public string? CcEmails { get; set; }
        public string? BccEmails { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string? Body { get; set; }
        public string? BodyPreview { get; set; }
        public bool IsHtml { get; set; }
        public string? TemplateKey { get; set; }
        public string? TemplateName { get; set; }
        public string? TemplateVersion { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? GoogleMessageId { get; set; }
        public string? GoogleThreadId { get; set; }
        public DateTimeOffset? SentAt { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class GoogleCustomerMailLogQueryDto : PagedRequest
    {
        public long? CustomerId { get; set; }
        public bool ErrorsOnly { get; set; }
    }
}
