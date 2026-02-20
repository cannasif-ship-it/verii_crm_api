using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class CreateShippingAddressDto
    {
        [MaxLength(150)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }
        
        public decimal? Latitude { get; set; }
        
        public decimal? Longitude { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public long? CountryId { get; set; }

        public long? CityId { get; set; }

        public long? DistrictId { get; set; }

        public bool IsDefault { get; set; } = false;
    }

    public class UpdateShippingAddressDto
    {
        [MaxLength(150)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }
        
        public decimal? Latitude { get; set; }
        
        public decimal? Longitude { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public long? CountryId { get; set; }

        public long? CityId { get; set; }

        public long? DistrictId { get; set; }

        public bool IsDefault { get; set; } = false;
    }

    public class ShippingAddressGetDto : BaseEntityDto
    {
        public string? Name { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Notes { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long? CountryId { get; set; }
        public string? CountryName { get; set; }
        public long? CityId { get; set; }
        public string? CityName { get; set; }
        public long? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        
    }
}
