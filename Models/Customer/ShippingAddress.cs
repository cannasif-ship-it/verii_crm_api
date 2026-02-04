using System;
namespace crm_api.Models
{
    public class ShippingAddress : BaseEntity
    {

        public string Address { get; set; } = string.Empty;

        public string? PostalCode { get; set; }

        public string? ContactPerson { get; set; }  // Sevk adresinde yetkili kişi
        
        public string? Phone { get; set; } // Sevk adresinde yetkili kişinin telefon numarası

        public string? Notes { get; set; }  // Özel notlar veya teslim talimatları

        // Foreign Key
        public long CustomerId { get; set; }
        public Customer Customer { get; set; } = null!; // Navigation

         // Location Information
        public long? CountryId { get; set; } // UlkeId
        public long? CityId { get; set; } // SehirId
        public long? DistrictId { get; set; } // IlceId

        // Navigation Properties
        public Country? Countries { get; set; } // Ulke
        public City? Cities { get; set; } // Sehir
        public District? Districts { get; set; } // Ilce

    }
}