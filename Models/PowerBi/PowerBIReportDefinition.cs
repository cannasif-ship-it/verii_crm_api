namespace crm_api.Models.PowerBi
{
    public class PowerBIReportDefinition : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public Guid WorkspaceId { get; set; }
        public Guid ReportId { get; set; }
        public Guid? DatasetId { get; set; }

        public string? EmbedUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public string? RlsRoles { get; set; }
        public string? AllowedUserIds { get; set; }
        public string? AllowedRoleIds { get; set; }
    }
}
