using System;
using System.Collections.Generic;

namespace crm_api.Models
{
    public class Activity : BaseEntity
    {
        public string Subject { get; set; } = string.Empty;
        public string? Description { get; set; }

        public long ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; } = null!;

        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool IsAllDay { get; set; } = false;

        public ActivityStatus Status { get; set; } = ActivityStatus.Scheduled;
        public ActivityPriority Priority { get; set; } = ActivityPriority.Medium;

        public long AssignedUserId { get; set; }
        public User AssignedUser { get; set; } = null!;

        public long? ContactId { get; set; }
        public Contact? Contact { get; set; }

        public long? PotentialCustomerId { get; set; }
        public Customer? PotentialCustomer { get; set; }

        public string? ErpCustomerCode { get; set; }

        public ICollection<ActivityReminder> Reminders { get; set; } = new List<ActivityReminder>();
    }
}
