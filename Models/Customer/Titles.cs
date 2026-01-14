using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_TITLE")]
    public class Title : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleName  { get; set; } = string.Empty; // ünvan adı

        [MaxLength(10)]
        public string? Code  { get; set; } // e.g. PURMGR, GM, ITDIR

        // 🔗 Relationship: One Title can belong to many Contacts
        public ICollection<Contact>? Contacts { get; set; }

    }
}
