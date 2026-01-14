using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class CityGetDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? ERPCode { get; set; }
        public long CountryId { get; set; }
        public string? CountryName { get; set; }
    }

    public class CityCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? ERPCode { get; set; }

        [Required]
        public long CountryId { get; set; }
    }

    public class CityUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? ERPCode { get; set; }

        [Required]
        public long CountryId { get; set; }
    }
}
