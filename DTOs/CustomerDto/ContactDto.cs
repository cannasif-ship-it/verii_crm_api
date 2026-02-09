using System;
using System.ComponentModel.DataAnnotations;
using crm_api.Models;

namespace crm_api.DTOs
{
    public class ContactDto : BaseEntityDto
    {
        public SalutationType Salutation { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Notes { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long? TitleId { get; set; }
        public string? TitleName { get; set; }
        
    }

    public class CreateContactDto
    {
        public SalutationType Salutation { get; set; } = SalutationType.None;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? FullName { get; set; }

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

        public long? TitleId { get; set; }
    }

    public class UpdateContactDto
    {
        public SalutationType Salutation { get; set; } = SalutationType.None;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? FullName { get; set; }

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

        public long? TitleId { get; set; }
    }
}
