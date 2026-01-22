using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs.MailDto
{
    public class BulkSendMailDto
    {
        [Required(ErrorMessage = "To email addresses are required")]
        [MinLength(1, ErrorMessage = "At least one recipient is required")]
        public List<string> To { get; set; } = new List<string>();

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
