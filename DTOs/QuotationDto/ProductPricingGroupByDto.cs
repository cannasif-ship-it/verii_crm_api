using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ProductPricingGroupByDto
    {
        public int Id { get; set; }
        public string ErpGroupCode { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal ListPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal? Discount1 { get; set; }
        public decimal? Discount2 { get; set; }
        public decimal? Discount3 { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
    }

    public class CreateProductPricingGroupByDto
    {
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

    public class UpdateProductPricingGroupByDto
    {
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
