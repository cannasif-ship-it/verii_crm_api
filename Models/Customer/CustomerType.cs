using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_CUSTOMER_TYPE")]
    public class CustomerType : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // Tip adı, örn. "Customer", "Supplier", "Potential"

        [MaxLength(255)]
        public string? Description { get; set; } // Opsiyonel açıklama

        // Navigation
        public ICollection<Customer>? Customers { get; set; }
    }
}
