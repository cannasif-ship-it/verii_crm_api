using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class PaymentTypeGetDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class PaymentTypeCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class PaymentTypeUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
