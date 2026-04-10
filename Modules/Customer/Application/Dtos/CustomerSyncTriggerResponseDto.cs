namespace crm_api.Modules.Customer.Application.Dtos
{
    public class CustomerSyncTriggerResponseDto
    {
        public string JobId { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
        public DateTime EnqueuedAtUtc { get; set; }
    }
}
