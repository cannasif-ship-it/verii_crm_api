using System;

namespace crm_api.Models
    {
    public class Contact : BaseEntity
    {
        // Mail hitabı
        public SalutationType Salutation { get; set; } = SalutationType.None;

        // İsim
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;

        // CRM içi gösterim
        public string FullName { get; set; } = string.Empty;

        // İletişim
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Notes { get; set; }

        // Şirket
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        // 🎯 İş ünvanı / departman rolü
        public long? TitleId { get; set; }
        public virtual Title? Title { get; set; }
    }

}