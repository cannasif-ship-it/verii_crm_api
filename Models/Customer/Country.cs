using System;
using System.Collections.Generic;

namespace cms_webapi.Models
{
    public class Country : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // UlkeAdi

        public string Code { get; set; } = string.Empty; // UlkeKodu (ISO)

        public string? ERPCode { get; set; } // Netsis veya ERP eşleşmesi

        // Navigation
        public ICollection<City>? Cities { get; set; }
        // Navigation
        public ICollection<Contact>? Contacts { get; set; }
    }
}
