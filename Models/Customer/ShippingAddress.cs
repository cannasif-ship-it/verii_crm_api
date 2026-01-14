using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_SHIPPING_ADDRESS")]
    public class ShippingAddress : BaseEntity
    {

        [Required]
        [StringLength(150)]
        public string Address { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }  // Sevk adresinde yetkili kişi
        
        [StringLength(20)]
        public string? Phone { get; set; } // Sevk adresinde yetkili kişinin telefon numarası

        [StringLength(100)]
        public string? Notes { get; set; }  // Özel notlar veya teslim talimatları

        // Foreign Key
        [Required]
        public long CustomerId { get; set; }
        public Customer Customer { get; set; } = null!; // Navigation

         // Location Information
        public long? CountryId { get; set; } // UlkeId
        public long? CityId { get; set; } // SehirId
        public long? DistrictId { get; set; } // IlceId

        // Navigation Properties
        [ForeignKey("CountryId")]
        public Country? Countries { get; set; } // Ulke
        [ForeignKey("CityId")]
        public City? Cities { get; set; } // Sehir
        [ForeignKey("DistrictId")]
        public District? Districts { get; set; } // Ilce

    }
}
