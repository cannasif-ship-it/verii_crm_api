using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.MailDto
{
    public class SendTestMailDto
    {
        // Optional. If empty, email is sent to the current logged-in user's email.
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? To { get; set; }
    }
}

