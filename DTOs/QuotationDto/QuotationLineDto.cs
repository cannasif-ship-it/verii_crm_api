using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class QuotationLineDto : BaseEntityDto
    {
        public long QuotationId { get; set; }
        public long? ProductId { get; set; }
        public string? ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountAmount1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountAmount2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public decimal DiscountAmount3 { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineGrandTotal { get; set; }
        public string? Description { get; set; }
        public long? PricingRuleHeaderId { get; set; }
        public long? RelatedStockId { get; set; }
    }

    public class CreateQuotationLineDto
    {
        [Required]
        public long QuotationId { get; set; }

        [Required]
        public long? ProductId { get; set; }

        [MaxLength(100)]
        public string ProductCode { get; set; } = string.Empty;

        [MaxLength(250)]
        public string ProductName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountAmount1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountAmount2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public decimal DiscountAmount3 { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineGrandTotal { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
        public long? PricingRuleHeaderId { get; set; }
        public long? RelatedStockId { get; set; }
    }

    public class UpdateQuotationLineDto
    {
        [Required]
        public long ProductId { get; set; }

        [MaxLength(100)]
        public string? ProductCode { get; set; } = string.Empty;

        [MaxLength(250)]
        public string ProductName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountAmount1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountAmount2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public decimal DiscountAmount3 { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineGrandTotal { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
        public long? PricingRuleHeaderId { get; set; }
        public long? RelatedStockId { get; set; }
    }

    public class QuotationLineGetDto : BaseEntityDto
    {
        public long QuotationId { get; set; }
        public long? ProductId { get; set; }
        public string? ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountAmount1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountAmount2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public decimal DiscountAmount3 { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineGrandTotal { get; set; }
        public string? Description { get; set; }
        public long? PricingRuleHeaderId { get; set; }
        public long? RelatedStockId { get; set; }
    }
}
