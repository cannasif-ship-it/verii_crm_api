using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models.ReportBuilder
{
    [Table("RII_REPORT_DEFINITIONS")]
    public class ReportDefinition : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string ConnectionKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string DataSourceType { get; set; } = "view";

        [Required]
        [MaxLength(128)]
        public string DataSourceName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string ConfigJson { get; set; } = string.Empty;
    }
}
