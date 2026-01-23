using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("RII_CUSTOMER")]
    public class Customer : BaseHeaderEntity
    {
        // Basic Information
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? CustomerCode { get; set; } // CariKod

        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string CustomerName { get; set; } = string.Empty; // CariAd
        public long? CustomerTypeId { get; set; } // 'M' = Customer, 'T' = Supplier, 'P' = Potential
        [ForeignKey("CustomerTypeId")]
        public CustomerType? CustomerTypes { get; set; } // CariTipId

        // Tax Information
        public string? TaxOffice { get; set; } // VergiDaire
        public string? TaxNumber { get; set; } // VergiNo
        public string? TcknNumber { get; set; } // TCKNNo

        // Classification
        public string? SalesRepCode { get; set; } // PlasiyerKodu
        public string? GroupCode { get; set; } // GrupKodu

        // Financial Information
        [Column(TypeName = "decimal(18,6)")]
        public decimal? CreditLimit { get; set; } // RiskSiniri

        // ERP Integration
        public short BranchCode { get; set; } // SubeKodu
        public short BusinessUnitCode { get; set; } // IsletmeKodu

        // Contact Information
        [MaxLength(250)]
        public string? Notes { get; set; } // CRMNotes
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(100)]
        public string? Website { get; set; } // Web
        [MaxLength(100)]
        public string? Phone1 { get; set; } // Telefon1
        [MaxLength(100)]
        public string? Phone2 { get; set; } // Telefon2
        [MaxLength(100)]
        public string? Address { get; set; } // Adres

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
