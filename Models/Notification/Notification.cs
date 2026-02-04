using System;
using crm_api.Models;

namespace crm_api.Models.Notification
{
    public class Notification : BaseEntity
    {
        public string TitleKey { get; set; } = string.Empty;
        public string? TitleArgs { get; set; } // JSON serialized arguments

        public string MessageKey { get; set; } = string.Empty;
        public string? MessageArgs { get; set; } // JSON serialized arguments

        public bool IsRead { get; set; } = false;

        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public string? RelatedEntityName { get; set; }
        public long? RelatedEntityId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}