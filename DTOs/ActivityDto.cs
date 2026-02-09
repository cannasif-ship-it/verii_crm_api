using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using crm_api.Models;

namespace crm_api.DTOs
{
    public class ActivityReminderDto : BaseEntityDto
    {
        public long ActivityId { get; set; }
        public int OffsetMinutes { get; set; }
        public ReminderChannel Channel { get; set; }
        public DateTime? SentAt { get; set; }
        public ReminderStatus Status { get; set; }
    }

    public class CreateActivityReminderDto
    {
        [Range(0, 525600)]
        public int OffsetMinutes { get; set; }

        public ReminderChannel Channel { get; set; } = ReminderChannel.InApp;
    }

    public class ActivityDto : BaseEntityDto
    {
        public string Subject { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ActivityTypeId { get; set; }
        public ActivityTypeGetDto ActivityType { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool IsAllDay { get; set; }
        public ActivityStatus Status { get; set; } = ActivityStatus.Scheduled;
        public ActivityPriority Priority { get; set; } = ActivityPriority.Medium;
        public long AssignedUserId { get; set; }
        public UserDto AssignedUser { get; set; } = null!;
        public long? ContactId { get; set; }
        public ContactDto? Contact { get; set; }
        public long? PotentialCustomerId { get; set; }
        public CustomerGetDto? PotentialCustomer { get; set; }
        public string? ErpCustomerCode { get; set; }
        public List<ActivityReminderDto> Reminders { get; set; } = new List<ActivityReminderDto>();
    }

    public class CreateActivityDto
    {
        [Required]
        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public long ActivityTypeId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public bool IsAllDay { get; set; } = false;

        public ActivityStatus Status { get; set; } = ActivityStatus.Scheduled;

        public ActivityPriority Priority { get; set; } = ActivityPriority.Medium;

        [Required]
        public long AssignedUserId { get; set; }

        public long? ContactId { get; set; }

        public long? PotentialCustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public List<CreateActivityReminderDto> Reminders { get; set; } = new List<CreateActivityReminderDto>();
    }

    public class UpdateActivityDto
    {
        [Required]
        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public long ActivityTypeId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public bool IsAllDay { get; set; } = false;

        public ActivityStatus Status { get; set; } = ActivityStatus.Scheduled;

        public ActivityPriority Priority { get; set; } = ActivityPriority.Medium;

        [Required]
        public long AssignedUserId { get; set; }

        public long? ContactId { get; set; }

        public long? PotentialCustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public List<CreateActivityReminderDto> Reminders { get; set; } = new List<CreateActivityReminderDto>();
    }
}
