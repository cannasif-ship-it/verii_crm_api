using System;

namespace crm_api.Models
{
    public class Contact : BaseEntity
    {
        public string FullName { get; set; }  = string.Empty; // e.g. John Doe

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Mobile { get; set; }

        public string? Notes { get; set; }  // Additional remarks or relationship context

        // Foreign Key
        public long CustomerId { get; set; }
        // Navigation property
        public virtual Customer Customers { get; set; } = null!;

        // Foreign Key
        public long TitleId { get; set; }
        // Navigation property
        public virtual Title Titles { get; set; } = null!;
    }
}
