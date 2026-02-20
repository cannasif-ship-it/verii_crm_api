namespace crm_api.Models
{
    public class ShippingAddress : BaseEntity
    {
        // Adres etiketi (Merkez Depo / Fabrika / Şube-1 gibi)
        public string? Name { get; set; }

        public string Address { get; set; } = string.Empty;
        public string? PostalCode { get; set; }

        // Sevk adresinde yetkili kişi
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public string? Notes { get; set; }  // teslim talimatları vs.

        // Default sevk adresi flag'i (opsiyonel ama çok kullanışlı)
        public bool IsDefault { get; set; } = false;

        // Foreign Key
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        // Location Information
        public long? CountryId { get; set; } // UlkeId
        public long? CityId { get; set; } // SehirId
        public long? DistrictId { get; set; } // IlceId

        // Navigation Properties (tekil)
        public Country? Country { get; set; } // Ulke
        public City? City { get; set; } // Sehir
        public District? District { get; set; } // Ilce
    }
}
