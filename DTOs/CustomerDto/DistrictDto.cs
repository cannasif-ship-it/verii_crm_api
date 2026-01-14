using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class DistrictGetDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? ERPCode { get; set; }
        public long CityId { get; set; }
        public string? CityName { get; set; }
        
    }

    public class DistrictCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? ERPCode { get; set; }

        [Required]
        public long CityId { get; set; }
    }

    public class DistrictUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? ERPCode { get; set; }

        [Required]
        public long CityId { get; set; }
    }
}
