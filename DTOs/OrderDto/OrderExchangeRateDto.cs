using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class OrderExchangeRateGetDto : BaseEntityDto
    {
        public long OrderId { get; set; }
        public string? OrderOfferNo { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public DateTime ExchangeRateDate { get; set; }
        public bool IsOfficial { get; set; }
    }

    public class OrderExchangeRateCreateDto
    {
        [Required]
        public long OrderId { get; set; }

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

    public class OrderExchangeRateUpdateDto
    {
        [Required]
        public long OrderId { get; set; }

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
