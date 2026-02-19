using System.Collections.Generic;

namespace crm_api.Models
{
    public class Customer : BaseHeaderEntity
    {
        // Basic Information
        public string? CustomerCode { get; set; } // CariKod
        public string CustomerName { get; set; } = string.Empty; // CariAd

        public long? CustomerTypeId { get; set; } // 'M' = Customer, 'T' = Supplier, 'P' = Potential
        public CustomerType? CustomerType { get; set; } // Navigation (tekil)

        // Tax Information
        public string? TaxOffice { get; set; } // VergiDaire
        public string? TaxNumber { get; set; } // VergiNo
        public string? TcknNumber { get; set; } // TCKNNo

        // Classification
        public string? SalesRepCode { get; set; } // PlasiyerKodu
        public string? GroupCode { get; set; } // GrupKodu

        // Financial Information
        public decimal? CreditLimit { get; set; } // RiskSiniri

        // ERP Integration
        public short BranchCode { get; set; } // SubeKodu
        public short BusinessUnitCode { get; set; } // IsletmeKodu

        // Contact Information
        public string? Notes { get; set; } // CRMNotes
        public string? Email { get; set; }
        public string? Website { get; set; } // Web
        public string? Phone1 { get; set; } // Telefon1
        public string? Phone2 { get; set; } // Telefon2

        // (Opsiyonel) Merkez/Özet adres gibi kullanacaksan kalsın.
        // Sevk adresleri zaten ShippingAddress tablosunda.
        public string? Address { get; set; } // Adres (opsiyonel / legacy)

        // Location Information
        public long? CountryId { get; set; } // UlkeId
        public long? CityId { get; set; } // SehirId
        public long? DistrictId { get; set; } // IlceId

        // Navigation Properties (tekil)
        public Country? Country { get; set; } // Ulke
        public City? City { get; set; } // Sehir
        public District? District { get; set; } // Ilce

        // ✅ Relationships
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
        public virtual ICollection<CustomerImage> CustomerImages { get; set; } = new List<CustomerImage>();

        // ✅ Default Shipping Address (çok işe yarar)
        public long? DefaultShippingAddressId { get; set; }
        public virtual ShippingAddress? DefaultShippingAddress { get; set; }
    }
}
