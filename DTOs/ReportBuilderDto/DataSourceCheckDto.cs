using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.ReportBuilderDto
{
    public class DataSourceCheckRequestDto
    {
        [Required]
        [MaxLength(20)]
        public string ConnectionKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
    }

    public class DataSourceCheckResponseDto
    {
        public bool Exists { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<FieldSchemaDto> Schema { get; set; } = new();
    }
}
