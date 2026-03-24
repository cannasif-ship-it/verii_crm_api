namespace crm_api.Models.ReportBuilder
{
    public class ReportDefinition : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ConnectionKey { get; set; } = string.Empty;

        public string DataSourceType { get; set; } = "view";

        public string DataSourceName { get; set; } = string.Empty;
        public string ConfigJson { get; set; } = string.Empty;

        public ICollection<ReportAssignment> Assignments { get; set; } = new List<ReportAssignment>();
    }
}
