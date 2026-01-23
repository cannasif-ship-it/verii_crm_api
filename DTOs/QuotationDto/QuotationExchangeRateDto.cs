using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class QuotationExchangeRateGetDto : BaseEntityDto
    {
        public long QuotationId { get; set; }
        public string? QuotationOfferNo { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public DateTime ExchangeRateDate { get; set; }
        public bool IsOfficial { get; set; }
    }

    public class QuotationExchangeRateCreateDto
    {
        [Required]
        public long QuotationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ExchangeRate { get; set; }

        [Required]
        public DateTime ExchangeRateDate { get; set; }

        public bool IsOfficial { get; set; } = true;
    }

    public class QuotationExchangeRateUpdateDto
    {
        [Required]
        public long QuotationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ExchangeRate { get; set; }

        [Required]
        public DateTime ExchangeRateDate { get; set; }

        public bool IsOfficial { get; set; }
    }
}
