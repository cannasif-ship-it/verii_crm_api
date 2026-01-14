using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_PRODUCT_PRICING_GROUP_BY")]
    public class ProductPricingGroupBy : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string ErpGroupCode { get; set; } = string.Empty; // ERP Product Group Code

        [Required]
        [MaxLength(50)]
        public string Currency { get; set; } = string.Empty; // Para birimi (örn: TL, USD, EUR)

        [Column(TypeName = "decimal(18,6)")]
        public decimal ListPrice { get; set; } // Liste fiyatı (satış fiyatı)

        [Column(TypeName = "decimal(18,6)")]
        public decimal CostPrice { get; set; } // Maliyet fiyatı

        [Column(TypeName = "decimal(18,6)")]
        public decimal? Discount1 { get; set; } // 1. iskonto (%)

        [Column(TypeName = "decimal(18,6)")]
        public decimal? Discount2 { get; set; } // 2. iskonto (%)

        [Column(TypeName = "decimal(18,6)")]
        public decimal? Discount3 { get; set; } // 3. iskonto (%)
    }
}
