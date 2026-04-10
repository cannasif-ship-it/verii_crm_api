using System;
using System.Collections.Generic;
namespace crm_api.Modules.Pricing.Domain.Entities
{
    public class PricingRuleLine : BaseEntity
    {
       public long PricingRuleHeaderId { get; set; }
        public PricingRuleHeader PricingRuleHeader { get; set; } = null!;

        public string StokCode { get; set; } = string.Empty;
        public decimal MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public decimal? FixedUnitPrice { get; set; }

        // Fiyat bilgisi
        public string CurrencyCode { get; set; } = string.Empty;

        // 🔹 İndirimler
        public decimal DiscountRate1 { get; set; } = 0m; // % bazlı indirim 1
        public decimal DiscountAmount1 { get; set; } = 0m; // Tutar bazlı indirim 1
        public decimal DiscountRate2 { get; set; } = 0m; // % bazlı indirim 2
        public decimal DiscountAmount2 { get; set; } = 0m; // Tutar bazlı indirim 2
        public decimal DiscountRate3 { get; set; } = 0m; // % bazlı indirim 3
        public decimal DiscountAmount3 { get; set; } = 0m; // Tutar bazlı indirim 3
    }
}