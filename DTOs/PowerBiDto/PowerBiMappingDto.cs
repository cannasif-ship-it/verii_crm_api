using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.PowerBi
{
    public class MapPowerBIGroupToReportDto
    {
        [Required]
        public long GroupId { get; set; }

        [Required]
        public long ReportDefinitionId { get; set; }
    }

    public class MapUserToPowerBIGroupDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long GroupId { get; set; }
    }
}
