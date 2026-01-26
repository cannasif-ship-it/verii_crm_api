using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models
{
    [Table("RII_DEMAND_EXCHANGE_RATE")]
    public class DemandExchangeRate : BaseEntity
    {
        public long DemandId { get; set; }
        [ForeignKey("DemandId")]
        public Demand Demand { get; set; } = null!;

        public string Currency { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,6)")]
        public decimal ExchangeRate { get; set; }

        public DateTime ExchangeRateDate { get; set; }

        public bool IsOfficial { get; set; } = true; // Resmi döviz kurları mı?

    }
}
