namespace crm_api.DTOs.PowerBi
{
    public class CreatePowerBIReportRoleMappingDto
    {
        public long PowerBIReportDefinitionId { get; set; }
        public long RoleId { get; set; }
        public string RlsRoles { get; set; } = string.Empty;
    }
}
