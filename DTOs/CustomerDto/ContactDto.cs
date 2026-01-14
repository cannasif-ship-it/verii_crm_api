using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ContactDto : BaseEntityDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Notes { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long TitleId { get; set; }
        public string? TitleName { get; set; }
        
    }

    public class CreateContactDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(20)]
        public string? Mobile { get; set; }

        [MaxLength(250)]
        public string? Notes { get; set; }

        [Required]
        public long CustomerId { get; set; }

        [Required]
        public long TitleId { get; set; }
    }

    public class UpdateContactDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(20)]
        public string? Mobile { get; set; }

        [MaxLength(250)]
        public string? Notes { get; set; }

        [Required]
        public long CustomerId { get; set; }

        [Required]
        public long TitleId { get; set; }
    }
}
