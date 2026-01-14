using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class CreateShippingAddressDto
    {
        [Required]
        [MaxLength(150)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public long? CountryId { get; set; }

        public long? CityId { get; set; }

        public long? DistrictId { get; set; }
    }

    public class UpdateShippingAddressDto
    {
        [Required]
        [MaxLength(150)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public long? CountryId { get; set; }

        public long? CityId { get; set; }

        public long? DistrictId { get; set; }
    }

    public class ShippingAddressGetDto : BaseEntityDto
    {
        public string Address { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Notes { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long? CountryId { get; set; }
        public string? CountryName { get; set; }
        public long? CityId { get; set; }
        public string? CityName { get; set; }
        public long? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public bool IsActive { get; set; }
        
    }
}
