namespace crm_api.Models
{
    public enum ActivityStatus
    {
        Scheduled = 0,
        Completed = 1,
        Cancelled = 2
    }

    public enum ActivityPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum ReminderChannel
    {
        InApp = 0,
        Email = 1,
        Sms = 2,
        Push = 3
    }

    public enum ReminderStatus
    {
        Pending = 0,
        Sent = 1,
        Failed = 2,
        Cancelled = 3
    }
}
