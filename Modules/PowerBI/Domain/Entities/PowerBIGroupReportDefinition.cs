namespace crm_api.Modules.PowerBI.Domain.Entities
{
    /// <summary>
    /// Group -> ReportDefinition erişim eşleştirmesi
    /// </summary>
    public class PowerBIGroupReportDefinition : BaseEntity
    {
        public long GroupId { get; set; }
        public PowerBIGroup? Group { get; set; }

        public long ReportDefinitionId { get; set; }
        public PowerBIReportDefinition? ReportDefinition { get; set; }
    }
}