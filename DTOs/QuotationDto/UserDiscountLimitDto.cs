using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class UserDiscountLimitDto : BaseEntityDto
    {
        public string ErpProductGroupCode { get; set; } = string.Empty;
        public long SalespersonId { get; set; }
        public string SalespersonName { get; set; } = string.Empty;
        public decimal MaxDiscount1 { get; set; }
        public decimal? MaxDiscount2 { get; set; }
        public decimal? MaxDiscount3 { get; set; }
        
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public long? DeletedBy { get; set; }
    }

    public class CreateUserDiscountLimitDto
    {
        [Required]
        [MaxLength(50)]
        public string ErpProductGroupCode { get; set; } = string.Empty;

        [Required]
        public long SalespersonId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal MaxDiscount1 { get; set; }

        [Range(0, 100)]
        public decimal? MaxDiscount2 { get; set; }

        [Range(0, 100)]
        public decimal? MaxDiscount3 { get; set; }
    }

    public class UpdateUserDiscountLimitDto
    {
        [Required]
        [MaxLength(50)]
        public string ErpProductGroupCode { get; set; } = string.Empty;

        [Required]
        public long SalespersonId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal MaxDiscount1 { get; set; }

        [Range(0, 100)]
        public decimal? MaxDiscount2 { get; set; }

        [Range(0, 100)]
        public decimal? MaxDiscount3 { get; set; }
    }
}
