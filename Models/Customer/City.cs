using System;
using System.Collections.Generic;
namespace crm_api.Models
{
    public class City : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // SehirAdi

        public string? ERPCode { get; set; } // Netsis veya ERP eşleşmesi

        // Foreign Key
        public long CountryId { get; set; }
        public Country Country { get; set; } = null!; // Navigation

        // Navigation
        public ICollection<District>? Districts { get; set; }
    }
}