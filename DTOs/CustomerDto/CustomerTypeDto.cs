using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class CustomerTypeGetDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CustomerTypeCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }
    }

    public class CustomerTypeUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }
    }
}
