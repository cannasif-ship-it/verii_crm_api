using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace cms_webapi.Models
{
    [Table("RII_CITY")]
    public class City : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // SehirAdi

        [MaxLength(10)]
        public string? ERPCode { get; set; } // Netsis veya ERP eşleşmesi

        // Foreign Key
        [Required]
        public long CountryId { get; set; }
        public Country Country { get; set; } = null!; // Navigation

        // Navigation
        public ICollection<District>? Districts { get; set; }
    }
}

