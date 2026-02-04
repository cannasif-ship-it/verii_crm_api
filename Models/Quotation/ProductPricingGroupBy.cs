using System;
using System.Collections.Generic;
namespace crm_api.Models
{
    public class ProductPricingGroupBy : BaseEntity
    {
        public string ErpGroupCode { get; set; } = string.Empty; // ERP Product Group Code

        public string Currency { get; set; } = string.Empty; // Para birimi (örn: TL, USD, EUR)
        public decimal ListPrice { get; set; } // Liste fiyatı (satış fiyatı)
        public decimal CostPrice { get; set; } // Maliyet fiyatı
        public decimal? Discount1 { get; set; } // 1. iskonto (%)
        public decimal? Discount2 { get; set; } // 2. iskonto (%)
        public decimal? Discount3 { get; set; } // 3. iskonto (%)
    }
}