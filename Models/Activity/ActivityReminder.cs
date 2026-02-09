using System;

namespace crm_api.Models
{
    public class ActivityReminder : BaseEntity
    {
        public long ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        public int OffsetMinutes { get; set; }
        public ReminderChannel Channel { get; set; } = ReminderChannel.InApp;
        public DateTime? SentAt { get; set; }
        public ReminderStatus Status { get; set; } = ReminderStatus.Pending;
    }
}
