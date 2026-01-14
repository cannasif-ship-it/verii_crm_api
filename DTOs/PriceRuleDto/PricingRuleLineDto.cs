using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class PricingRuleLineGetDto : BaseEntityDto
    {
        public long PricingRuleHeaderId { get; set; }
        public string StokCode { get; set; } = string.Empty;
        public decimal MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public decimal? FixedUnitPrice { get; set; }
        public string CurrencyCode { get; set; } = "TRY";
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountAmount1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountAmount2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public decimal DiscountAmount3 { get; set; }
    }

    public class PricingRuleLineCreateDto
    {
        [Required]
        public long PricingRuleHeaderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StokCode { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MinQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? FixedUnitPrice { get; set; }

        [MaxLength(10)]
        public string CurrencyCode { get; set; } = "TRY";

        [Range(0, 100)]
        public decimal DiscountRate1 { get; set; } = 0m;

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount1 { get; set; } = 0m;

        [Range(0, 100)]
        public decimal DiscountRate2 { get; set; } = 0m;

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount2 { get; set; } = 0m;

        [Range(0, 100)]
        public decimal DiscountRate3 { get; set; } = 0m;

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount3 { get; set; } = 0m;
    }

    public class PricingRuleLineUpdateDto
    {
        [Required]
        public long PricingRuleHeaderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StokCode { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MinQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? FixedUnitPrice { get; set; }

        [MaxLength(10)]
        public string CurrencyCode { get; set; } = "TRY";

        [Range(0, 100)]
        public decimal DiscountRate1 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount1 { get; set; }

        [Range(0, 100)]
        public decimal DiscountRate2 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount2 { get; set; }

        [Range(0, 100)]
        public decimal DiscountRate3 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount3 { get; set; }
    }
}
