using System;
namespace crm_api.Modules.Customer.Domain.Entities
{
    public class District : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // IlceAdi

        public string? ERPCode { get; set; } // Netsis veya ERP eşleşmesi

        // Foreign Key
        public long CityId { get; set; }
        public City City { get; set; } = null!; // Navigation
    }
}