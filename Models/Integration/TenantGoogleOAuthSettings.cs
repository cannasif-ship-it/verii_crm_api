namespace crm_api.Models
{
    public class TenantGoogleOAuthSettings
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }

        public string ClientId { get; set; } = string.Empty;
        public string ClientSecretEncrypted { get; set; } = string.Empty;
        public string? RedirectUri { get; set; }
        public string? Scopes { get; set; }
        public bool IsEnabled { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
