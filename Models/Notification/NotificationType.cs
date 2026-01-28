using System.Text.Json.Serialization;

namespace crm_api.Models.Notification
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
