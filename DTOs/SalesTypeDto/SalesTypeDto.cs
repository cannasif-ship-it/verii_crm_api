using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class SalesTypeGetDto : BaseEntityDto
    {
        public string SalesType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class SalesTypeCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string SalesType { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
    }

    public class SalesTypeUpdateDto
    {
        [Required]
        [MaxLength(20)]
        public string SalesType { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
    }
}
