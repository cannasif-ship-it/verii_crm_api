namespace crm_api.Models.PowerBi
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