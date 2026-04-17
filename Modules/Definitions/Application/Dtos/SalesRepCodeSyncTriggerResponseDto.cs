namespace crm_api.Modules.Definitions.Application.Dtos
{
    public class SalesRepCodeSyncTriggerResponseDto
    {
        public string JobId { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
        public DateTime EnqueuedAtUtc { get; set; }
    }
}
