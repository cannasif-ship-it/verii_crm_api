namespace crm_api.Modules.Integrations.Application.Dtos
{
    public class TenantGoogleOAuthSettingsDto
    {
        public Guid TenantId { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecretMasked { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string Scopes { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public bool IsConfigured { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class UpdateTenantGoogleOAuthSettingsDto
    {
        public string ClientId { get; set; } = string.Empty;
        public string? ClientSecretPlain { get; set; }
        public string? RedirectUri { get; set; }
        public string? Scopes { get; set; }
        public bool IsEnabled { get; set; }
    }
}
