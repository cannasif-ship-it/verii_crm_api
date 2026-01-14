using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ActivityDto : BaseEntityDto
    {
        public string Subject { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public long? PotentialCustomerId { get; set; }
        public CustomerGetDto? PotentialCustomer { get; set; }
        public string? ErpCustomerCode { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string? Priority { get; set; }
        public long? ContactId { get; set; }
        public ContactDto? Contact { get; set; }
        public long? AssignedUserId { get; set; }
        public UserDto? AssignedUser { get; set; }
        public DateTime? ActivityDate { get; set; }
    }

    public class CreateActivityDto
    {
        [Required]
        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActivityType { get; set; } = string.Empty;

        public long? PotentialCustomerId { get; set; }

        public string? ErpCustomerCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        [MaxLength(50)]
        public string? Priority { get; set; }

        public long? ContactId { get; set; }

        public long? AssignedUserId { get; set; }

        public DateTime? ActivityDate { get; set; }
    }

    public class UpdateActivityDto
    {
        [Required]
        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActivityType { get; set; } = string.Empty;

        public long? PotentialCustomerId { get; set; }

        public string? ErpCustomerCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        [MaxLength(50)]
        public string? Priority { get; set; }

        public long? ContactId { get; set; }

        public long? AssignedUserId { get; set; }

        public DateTime? ActivityDate { get; set; }
    }
}
