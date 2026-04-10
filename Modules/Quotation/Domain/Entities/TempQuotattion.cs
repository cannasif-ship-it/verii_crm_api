using System;
using System.Collections.Generic;
using CustomerEntity = crm_api.Modules.Customer.Domain.Entities.Customer;
using QuotationEntity = crm_api.Modules.Quotation.Domain.Entities.Quotation;

namespace crm_api.Modules.Quotation.Domain.Entities
{
    public class TempQuotattion : BaseEntity
    {
        public long CustomerId { get; set; }
        public CustomerEntity? Customer { get; set; }
        public long? RevisionId { get; set; }
        public TempQuotattion? Revision { get; set; }
        public long? QuotationId { get; set; }
        public QuotationEntity? Quotation { get; set; }
        public string? QuotationNo { get; set; }

        public DateTime OfferDate { get; set; } = DateTime.UtcNow;
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; } = 1m;

        public decimal DiscountRate1 { get; set; } = 0m;
        public decimal DiscountRate2 { get; set; } = 0m;
        public decimal DiscountRate3 { get; set; } = 0m;

        public bool IsApproved { get; set; } = false;
        public DateTime? ApprovedDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public ICollection<TempQuotattion> Revisions { get; set; } = new List<TempQuotattion>();
        public ICollection<TempQuotattionLine> Lines { get; set; } = new List<TempQuotattionLine>();
        public ICollection<TempQuotattionExchangeLine> ExchangeLines { get; set; } = new List<TempQuotattionExchangeLine>();
    }

    public class TempQuotattionLine : BaseEntity
    {
        public long TempQuotattionId { get; set; }
        public TempQuotattion TempQuotattion { get; set; } = null!;

        public string ProductCode { get; set; } = string.Empty;
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

        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }

    public class TempQuotattionExchangeLine : BaseEntity
    {
        public long TempQuotattionId { get; set; }
        public TempQuotattion TempQuotattion { get; set; } = null!;

        public string Currency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; } = 0m;
        public DateTime ExchangeRateDate { get; set; } = DateTime.UtcNow;

        public bool IsManual { get; set; } = true;
    }
}
