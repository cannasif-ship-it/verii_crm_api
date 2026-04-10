using System;
using StockEntity = crm_api.Modules.Stock.Domain.Entities.Stock;
namespace crm_api.Modules.Quotation.Domain.Entities
{
    public class QuotationLine : BaseEntity
    {
        // 🔹 İlişki bilgileri
        public long QuotationId { get; set; }
        public Quotation Quotation { get; set; } = null!; // Navigation property

        // 🔹 Ürün bilgileri
        public string ProductCode { get; set; } = string.Empty;

        // 🔹 Miktar & Fiyat
        public decimal Quantity { get; set; } = 0m;
        public decimal UnitPrice { get; set; } = 0m;

        // 🔹 İndirimler
        public decimal DiscountRate1 { get; set; } = 0m; // % bazlı indirim 1
        public decimal DiscountAmount1 { get; set; } = 0m; // Tutar bazlı indirim 1
        public decimal DiscountRate2 { get; set; } = 0m; // % bazlı indirim 2
        public decimal DiscountAmount2 { get; set; } = 0m; // Tutar bazlı indirim 2
        public decimal DiscountRate3 { get; set; } = 0m; // % bazlı indirim 3
        public decimal DiscountAmount3 { get; set; } = 0m; // Tutar bazlı indirim 3

        // 🔹 KDV
        public decimal VatRate { get; set; } = 0m; // KDV oranı
        public decimal VatAmount { get; set; } = 0m; // KDV tutarı

        // 🔹 Toplamlar
        public decimal LineTotal { get; set; } = 0m; // KDV hariç toplam
        public decimal LineGrandTotal { get; set; } = 0m; // KDV dahil toplam

        // 🔹 Açıklama
        public string? Description { get; set; } // Satır açıklaması (opsiyonel)
        public string? Description1 { get; set; } // Satır açıklaması 1 (opsiyonel)
        public string? Description2 { get; set; } // Satır açıklaması 2 (opsiyonel)
        public string? Description3 { get; set; } // Satır açıklaması 3 (opsiyonel)

        // === Onay ===
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.HavenotStarted;

        public long? PricingRuleHeaderId { get; set; }
        public PricingRuleHeader? PricingRuleHeader { get; set; }

        public long? RelatedStockId { get; set; }
        public StockEntity? RelatedStock { get; set; }

        public string? RelatedProductKey { get; set; }

        public bool IsMainRelatedProduct { get; set; } = false;

        public string? ErpProjectCode { get; set; }  // max 50, nullable
    }
}
