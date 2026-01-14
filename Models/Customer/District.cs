using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_DISTRICT")]
    public class District : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // IlceAdi

        [MaxLength(10)]
        public string? ERPCode { get; set; } // Netsis veya ERP eşleşmesi

        // Foreign Key
        [Required]
        public long CityId { get; set; }
        public City City { get; set; } = null!; // Navigation
    }
}
