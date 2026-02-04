using System;
namespace crm_api.Models
{
    public class QuotationExchangeRate : BaseEntity
    {
        public long QuotationId { get; set; }
        public Quotation Quotation { get; set; } = null!;

        public string Currency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }

        public DateTime ExchangeRateDate { get; set; }

        public bool IsOfficial { get; set; } = true; // Resmi döviz kurları mı?

    }
}