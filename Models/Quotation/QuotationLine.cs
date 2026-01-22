using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_QUOTATION_LINE")]
    public class QuotationLine : BaseEntity
    {
        // 🔹 İlişki bilgileri
        public long QuotationId { get; set; }

        [ForeignKey("QuotationId")]
        public Quotation Quotation { get; set; } = null!; // Navigation property

        // 🔹 Ürün bilgileri
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string ProductCode { get; set; } = string.Empty;

        // 🔹 Miktar & Fiyat
        [Column(TypeName = "decimal(18,6)")]
        public decimal Quantity { get; set; } = 0m;

        [Column(TypeName = "decimal(18,6)")]
        public decimal UnitPrice { get; set; } = 0m;

        // 🔹 İndirimler
        [Column(TypeName = "decimal(18,6)")]
        public decimal DiscountRate1 { get; set; } = 0m; // % bazlı indirim 1

        [Column(TypeName = "decimal(18,6)")]
        public decimal DiscountAmount1 { get; set; } = 0m; // Tutar bazlı indirim 1

        [Column(TypeName = "decimal(18,6)")]
        public decimal DiscountRate2 { get; set; } = 0m; // % bazlı indirim 2

        [Column(TypeName = "decimal(18,6)")]
        public decimal DiscountAmount2 { get; set; } = 0m; // Tutar bazlı indirim 2

        [Column(TypeName = "decimal(18,6)")]
        public decimal DiscountRate3 { get; set; } = 0m; // % bazlı indirim 3

        [Column(TypeName = "decimal(18,6)")]
        public decimal DiscountAmount3 { get; set; } = 0m; // Tutar bazlı indirim 3

        // 🔹 KDV
        [Column(TypeName = "decimal(18,6)")]
        public decimal VatRate { get; set; } = 0m; // KDV oranı

        [Column(TypeName = "decimal(18,6)")]
        public decimal VatAmount { get; set; } = 0m; // KDV tutarı

        // 🔹 Toplamlar
        [Column(TypeName = "decimal(18,6)")]
        public decimal LineTotal { get; set; } = 0m; // KDV hariç toplam

        [Column(TypeName = "decimal(18,6)")]
        public decimal LineGrandTotal { get; set; } = 0m; // KDV dahil toplam

        // 🔹 Açıklama
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string? Description { get; set; } // Satır açıklaması (opsiyonel)

        // === Onay ===
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.HavenotStarted;

        public long? PricingRuleHeaderId { get; set; }
        [ForeignKey("PricingRuleHeaderId")]
        public PricingRuleHeader? PricingRuleHeader { get; set; }

        public long? RelatedStockId { get; set; }
        [ForeignKey("RelatedStockId")]
        public Stock? RelatedStock { get; set; }

        public string? RelatedProductKey { get; set; }

        public bool IsMainRelatedProduct { get; set; } = false;

    }
}
