
namespace crm_api.Modules.ReportBuilder.Domain.Entities
{
    public class ReportAssignment : BaseEntity
    {
        public long ReportDefinitionId { get; set; }
        public long UserId { get; set; }

        public ReportDefinition ReportDefinition { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
