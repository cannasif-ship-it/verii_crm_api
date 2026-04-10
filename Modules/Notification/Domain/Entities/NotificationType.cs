using System.Text.Json.Serialization;

namespace crm_api.Modules.Notification.Domain.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NotificationType
    {
        DemandDetail,
        DemandApproval,
        QuotationDetail,
        QuotationApproval,
        OrderDetail,
        OrderApproval
    }
}
