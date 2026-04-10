namespace crm_api.Modules.PowerBI.Application.Dtos
{
    public class CreatePowerBIReportRoleMappingDto
    {
        public long PowerBIReportDefinitionId { get; set; }
        public long RoleId { get; set; }
        public string RlsRoles { get; set; } = string.Empty;
    }
}
