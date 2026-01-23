using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class ActivityTypeGetDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ActivityTypeCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class ActivityTypeUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
