using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_USER_DISCOUNT_LIMIT")]
    public class UserDiscountLimit : BaseEntity
    {        
        
        [Required]
        public string ErpProductGroupCode { get; set; } = string.Empty;
        
        [Required]
        public long SalespersonId { get; set;}

        [ForeignKey("SalespersonId")]
        public virtual User Salespersons { get; set; } = null!;

        // Maksimum uygulanabilir iskonto oranları
        [Column(TypeName = "decimal(18,6)")]
        public decimal MaxDiscount1 { get; set; }  // örn: %10

        [Column(TypeName = "decimal(18,6)")]
        public decimal? MaxDiscount2 { get; set; }  // opsiyonel, örn: %5

        [Column(TypeName = "decimal(18,6)")]
        public decimal? MaxDiscount3 { get; set; }  // opsiyonel, örn: %3

    }
}
