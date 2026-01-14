using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_PAYMENT_TYPE")]
    public class PaymentType : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!; // Ödeme tipi adı (örnek: Peşin, Vadeli, Kredi Kartı)

        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; } // Açıklama (örnek: 30 gün vadeli, 3 taksit vb.)


    }
}
