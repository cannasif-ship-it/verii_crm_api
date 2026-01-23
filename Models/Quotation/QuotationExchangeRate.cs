using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models
{
    [Table("RII_QUOTATION_EXCHANGE_RATE")]
    public class QuotationExchangeRate : BaseEntity
    {
        public long QuotationId { get; set; }
        [ForeignKey("QuotationId")]
        public Quotation Quotation { get; set; } = null!;

        public string Currency { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,6)")]
        public decimal ExchangeRate { get; set; }

        public DateTime ExchangeRateDate { get; set; }

        public bool IsOfficial { get; set; } = true; // Resmi döviz kurları mı?

    }
}
