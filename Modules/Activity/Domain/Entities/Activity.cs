using System;
using System.Collections.Generic;
using CustomerEntity = crm_api.Modules.Customer.Domain.Entities.Customer;

namespace crm_api.Modules.Activity.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public string Subject { get; set; } = string.Empty;
        public string? Description { get; set; }

        public long ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; } = null!;

        public long? PaymentTypeId { get; set; }
        public PaymentType? PaymentType { get; set; }

        public long? ActivityMeetingTypeId { get; set; }
        public ActivityMeetingType? ActivityMeetingType { get; set; }

        public long? ActivityTopicPurposeId { get; set; }
        public ActivityTopicPurpose? ActivityTopicPurpose { get; set; }

        public long? ActivityShippingId { get; set; }
        public ActivityShipping? ActivityShipping { get; set; }

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
        public CustomerEntity? PotentialCustomer { get; set; }

        public string? ErpCustomerCode { get; set; }
        public string? GoogleCalendarEventId { get; set; }

        public ICollection<ActivityReminder> Reminders { get; set; } = new List<ActivityReminder>();
        public ICollection<ActivityImage> Images { get; set; } = new List<ActivityImage>();
    }
}
