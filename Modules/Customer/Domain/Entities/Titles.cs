using System;
namespace crm_api.Modules.Customer.Domain.Entities
{
    public class Title : BaseEntity
    {
        public string TitleName  { get; set; } = string.Empty; // ünvan adı

        public string? Code  { get; set; } // e.g. PURMGR, GM, ITDIR

        // 🔗 Relationship: One Title can belong to many Contacts
        public ICollection<Contact>? Contacts { get; set; }

    }
}