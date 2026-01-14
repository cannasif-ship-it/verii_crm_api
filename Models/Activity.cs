using System;
using System.Collections.Generic;

namespace cms_webapi.Models
{
    public class Activity : BaseEntity
    {
        public string Subject { get; set; } = String.Empty;  // e.g., "Meeting with John Doe"

        public string? Description { get; set; }

        public string ActivityType { get; set; } = String.Empty;  // e.g., "Call", "Meeting", "Email"

        public long? PotentialCustomerId { get; set; }
        public Customer? PotentialCustomer { get; set; }

        public string? ErpCustomerCode { get; set; } = String.Empty;  // e.g., "CUST001"

        public string Status { get; set; } = String.Empty;  // e.g., "Scheduled", "Completed", "Canceled"

        public bool IsCompleted { get; set; } = false;

        public string? Priority { get; set; }  // "Low", "Medium", "High"

        public long? ContactId { get; set; }
        public Contact? Contact { get; set; }

        public long? AssignedUserId { get; set; }  // CRM Kullanıcısı (sorumlu kişi)
        public User? AssignedUser { get; set; }
        public DateTime? ActivityDate { get; set; }
    }
}

