using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.ReportBuilderDto
{
    public class PreviewRequestDto
    {
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
        public string ConfigJson { get; set; } = string.Empty;
    }

    public class PreviewResponseDto
    {
        public List<FieldSchemaDto> Columns { get; set; } = new();
        public List<Dictionary<string, object?>> Rows { get; set; } = new();
    }
}
