using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class TempQuotattionGetDto : BaseEntityDto
    {
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OfferDate { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class TempQuotattionCreateDto
    {
        [Required]
        public long CustomerId { get; set; }

        [Required]
        [MaxLength(10)]
        public string CurrencyCode { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal ExchangeRate { get; set; } = 1m;

        [Range(typeof(decimal), "0", "100")]
        public decimal DiscountRate1 { get; set; } = 0m;

        [Range(typeof(decimal), "0", "100")]
        public decimal DiscountRate2 { get; set; } = 0m;

        [Range(typeof(decimal), "0", "100")]
        public decimal DiscountRate3 { get; set; } = 0m;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class TempQuotattionUpdateDto
    {
        [Required]
        public long CustomerId { get; set; }

        [Required]
        [MaxLength(10)]
        public string CurrencyCode { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal ExchangeRate { get; set; } = 1m;

        [Range(typeof(decimal), "0", "100")]
        public decimal DiscountRate1 { get; set; } = 0m;

        [Range(typeof(decimal), "0", "100")]
        public decimal DiscountRate2 { get; set; } = 0m;

        [Range(typeof(decimal), "0", "100")]
        public decimal DiscountRate3 { get; set; } = 0m;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class TempQuotattionLineGetDto : BaseEntityDto
    {
        public long TempQuotattionId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
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
        public string Description { get; set; } = string.Empty;
    }

    public class TempQuotattionLineCreateDto
    {
        [Required]
        public long TempQuotattionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string ProductName { get; set; } = string.Empty;

        public decimal Quantity { get; set; } = 0m;
        public decimal UnitPrice { get; set; } = 0m;
        public decimal DiscountRate1 { get; set; } = 0m;
        public decimal DiscountAmount1 { get; set; } = 0m;
        public decimal DiscountRate2 { get; set; } = 0m;
        public decimal DiscountAmount2 { get; set; } = 0m;
        public decimal DiscountRate3 { get; set; } = 0m;
        public decimal DiscountAmount3 { get; set; } = 0m;
        public decimal VatRate { get; set; } = 0m;
        public decimal VatAmount { get; set; } = 0m;
        public decimal LineTotal { get; set; } = 0m;
        public decimal LineGrandTotal { get; set; } = 0m;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class TempQuotattionLineUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string ProductName { get; set; } = string.Empty;

        public decimal Quantity { get; set; } = 0m;
        public decimal UnitPrice { get; set; } = 0m;
        public decimal DiscountRate1 { get; set; } = 0m;
        public decimal DiscountAmount1 { get; set; } = 0m;
        public decimal DiscountRate2 { get; set; } = 0m;
        public decimal DiscountAmount2 { get; set; } = 0m;
        public decimal DiscountRate3 { get; set; } = 0m;
        public decimal DiscountAmount3 { get; set; } = 0m;
        public decimal VatRate { get; set; } = 0m;
        public decimal VatAmount { get; set; } = 0m;
        public decimal LineTotal { get; set; } = 0m;
        public decimal LineGrandTotal { get; set; } = 0m;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class TempQuotattionExchangeLineGetDto : BaseEntityDto
    {
        public long TempQuotattionId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public DateTime ExchangeRateDate { get; set; }
        public bool IsManual { get; set; }
    }

    public class TempQuotattionExchangeLineCreateDto
    {
        [Required]
        public long TempQuotattionId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal ExchangeRate { get; set; } = 0m;

        [Required]
        public DateTime ExchangeRateDate { get; set; }

        public bool IsManual { get; set; } = true;
    }

    public class TempQuotattionExchangeLineUpdateDto
    {
        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal ExchangeRate { get; set; } = 0m;

        [Required]
        public DateTime ExchangeRateDate { get; set; }

        public bool IsManual { get; set; } = true;
    }
}
