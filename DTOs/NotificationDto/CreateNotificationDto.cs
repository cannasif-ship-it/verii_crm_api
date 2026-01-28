using crm_api.Models.Notification;

namespace crm_api.DTOs.NotificationDto
{
    public class CreateNotificationDto
    {
        public string TitleKey { get; set; } = string.Empty;
        public object[]? TitleArgs { get; set; } // We will serialize this to JSON string

        public string MessageKey { get; set; } = string.Empty;
        public object[]? MessageArgs { get; set; } // We will serialize this to JSON string

        public long UserId { get; set; }
        public string? RelatedEntityName { get; set; }
        public long? RelatedEntityId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
