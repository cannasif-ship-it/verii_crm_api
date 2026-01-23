using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.MailDto
{
    public class SendMailDto
    {
        [Required(ErrorMessage = "To email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = string.Empty;

        public bool IsHtml { get; set; } = true;

        [EmailAddress(ErrorMessage = "Invalid CC email address")]
        public string? Cc { get; set; }

        [EmailAddress(ErrorMessage = "Invalid BCC email address")]
        public string? Bcc { get; set; }

        public string? FromEmail { get; set; }

        public string? FromName { get; set; }

        public List<string>? Attachments { get; set; }
    }
}
