namespace crm_api.Modules.Integrations.Domain.Entities
{
    public class UserOutlookAccount
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public string? OutlookEmail { get; set; }
        public string? RefreshTokenEncrypted { get; set; }
        public string? AccessTokenEncrypted { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public string? Scopes { get; set; }
        public bool IsConnected { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
