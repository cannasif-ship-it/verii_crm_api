using System;
namespace crm_api.Modules.Order.Domain.Entities
{
    public class OrderExchangeRate : BaseEntity
    {
        public long OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public string Currency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }

        public DateTime ExchangeRateDate { get; set; }

        public bool IsOfficial { get; set; } = true; // Resmi döviz kurları mı?

    }
}