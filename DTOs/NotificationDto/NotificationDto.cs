using crm_api.Models.Notification;

namespace crm_api.DTOs.NotificationDto
{
    public class NotificationDto : BaseEntityDto
    {
        public string TitleKey { get; set; } = string.Empty;
        public string? TitleArgs { get; set; }
        public string Title { get; set; } = string.Empty; // Localized Title

        public string MessageKey { get; set; } = string.Empty;
        public string? MessageArgs { get; set; }
        public string Message { get; set; } = string.Empty; // Localized Message

        public bool IsRead { get; set; }
        public long UserId { get; set; }
        public string? RelatedEntityName { get; set; }
        public long? RelatedEntityId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
