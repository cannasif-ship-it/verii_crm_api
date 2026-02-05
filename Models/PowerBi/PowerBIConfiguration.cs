namespace crm_api.Models.PowerBi
{
    /// <summary>
    /// Power BI / Azure AD configuration (single record per env).
    /// ClientSecret is not stored; read from env/secret store only.
    /// </summary>
    public class PowerBIConfiguration : BaseEntity
    {
        public string TenantId { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public Guid WorkspaceId { get; set; }
        public string? ApiBaseUrl { get; set; }
        public string? Scope { get; set; }
    }
}
