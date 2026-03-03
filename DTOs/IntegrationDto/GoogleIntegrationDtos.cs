namespace crm_api.DTOs
{
    public class GoogleIntegrationStatusDto
    {
        public bool IsConnected { get; set; }
        public string? GoogleEmail { get; set; }
        public string? Scopes { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
    }

    public class GoogleAuthorizeUrlDto
    {
        public string Url { get; set; } = string.Empty;
    }

    public class GoogleTestEventDto
    {
        public string EventId { get; set; } = string.Empty;
    }

    public class GoogleOAuthTokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public int ExpiresInSeconds { get; set; }
        public string? Scope { get; set; }
        public string? IdToken { get; set; }
    }
}
