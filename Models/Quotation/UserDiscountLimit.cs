using System;
namespace crm_api.Models
{
    public class UserDiscountLimit : BaseEntity
    {        
        
        public string ErpProductGroupCode { get; set; } = string.Empty;
        
        public long SalespersonId { get; set;}
        public virtual User Salespersons { get; set; } = null!;

        // Maksimum uygulanabilir iskonto oranları
        public decimal MaxDiscount1 { get; set; }  // örn: %10
        public decimal? MaxDiscount2 { get; set; }  // opsiyonel, örn: %5
        public decimal? MaxDiscount3 { get; set; }  // opsiyonel, örn: %3

    }
}