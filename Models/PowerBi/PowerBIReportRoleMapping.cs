namespace crm_api.Models.PowerBi
{
    public class PowerBIReportRoleMapping : BaseEntity
    {
        public long PowerBIReportDefinitionId { get; set; }
        public PowerBIReportDefinition ReportDefinition { get; set; } = null!;

        public long RoleId { get; set; }
        public UserAuthority Role { get; set; } = null!;

        public string RlsRoles { get; set; } = string.Empty;
    }
}
