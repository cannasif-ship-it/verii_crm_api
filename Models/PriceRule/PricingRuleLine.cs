using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models
{
    [Table("RII_PRICING_RULE_LINE")]
    public class PricingRuleLine : BaseEntity
    {
       public long PricingRuleHeaderId { get; set; }

        [ForeignKey(nameof(PricingRuleHeaderId))]
        public PricingRuleHeader PricingRuleHeader { get; set; } = null!;

        public string StokCode { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,6)")]
        public decimal MinQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? MaxQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? FixedUnitPrice { get; set; }

        // Fiyat bilgisi
        public string CurrencyCode { get; set; } = string.Empty;

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
    }
}

