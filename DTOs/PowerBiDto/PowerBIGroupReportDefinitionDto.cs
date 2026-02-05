using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.PowerBi
{
    public class PowerBIGroupReportDefinitionGetDto : BaseEntityDto
    {
        public long GroupId { get; set; }
        public string? GroupName { get; set; }

        public long ReportDefinitionId { get; set; }
        public string? ReportDefinitionName { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CreatePowerBIGroupReportDefinitionDto
    {
        [Required]
        public long GroupId { get; set; }

        [Required]
        public long ReportDefinitionId { get; set; }
    }

    public class UpdatePowerBIGroupReportDefinitionDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long GroupId { get; set; }

        [Required]
        public long ReportDefinitionId { get; set; }
    }
}
