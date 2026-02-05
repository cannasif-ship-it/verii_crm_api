namespace crm_api.DTOs.PowerBi
{
    public class PowerBIReportRoleMappingGetDto
    {
        public long Id { get; set; }
        public long PowerBIReportDefinitionId { get; set; }
        public string? ReportName { get; set; }
        public long RoleId { get; set; }
        public string? RoleName { get; set; }
        public string RlsRoles { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
