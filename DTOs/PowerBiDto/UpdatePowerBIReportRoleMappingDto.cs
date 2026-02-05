namespace crm_api.DTOs.PowerBi
{
    public class UpdatePowerBIReportRoleMappingDto
    {
        public long PowerBIReportDefinitionId { get; set; }
        public long RoleId { get; set; }
        public string RlsRoles { get; set; } = string.Empty;
    }
}
