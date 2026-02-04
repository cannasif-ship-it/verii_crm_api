using System;
using System.Collections.Generic;
namespace crm_api.Models
{
    public class Activity : BaseEntity
    {
        public string Subject { get; set; } = String.Empty;  // e.g., "Meeting with John Doe"

        public string? Description { get; set; }

        public long? ActivityTypeId { get; set; }  // Foreign key to ActivityType
        public ActivityType? ActivityType { get; set; }

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