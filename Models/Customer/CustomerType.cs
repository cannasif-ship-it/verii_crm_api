using System;
using System.Collections.Generic;
namespace crm_api.Models
{
    public class CustomerType : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // Tip adı, örn. "Customer", "Supplier", "Potential"

        public string? Description { get; set; } // Opsiyonel açıklama

        // Navigation
        public ICollection<Customer>? Customers { get; set; }
    }
}