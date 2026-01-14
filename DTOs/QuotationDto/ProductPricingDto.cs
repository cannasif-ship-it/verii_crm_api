using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ProductPricingGetDto : BaseEntityDto
    {
        public string ErpProductCode { get; set; } = string.Empty;
        public string ErpGroupCode { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal ListPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal? Discount1 { get; set; }
        public decimal? Discount2 { get; set; }
        public decimal? Discount3 { get; set; }
    }

    public class ProductPricingCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string ErpProductCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ErpGroupCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        public decimal ListPrice { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        public decimal? Discount1 { get; set; }

        public decimal? Discount2 { get; set; }

        public decimal? Discount3 { get; set; }
    }

    public class ProductPricingUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string ErpProductCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ErpGroupCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        public decimal ListPrice { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        public decimal? Discount1 { get; set; }

        public decimal? Discount2 { get; set; }

        public decimal? Discount3 { get; set; }
    }
}
