using System;
namespace crm_api.Models
{
    public class PaymentType : BaseEntity
    {
        public string Name { get; set; } = null!; // Ödeme tipi adı (örnek: Peşin, Vadeli, Kredi Kartı)
        public string? Description { get; set; } // Açıklama (örnek: 30 gün vadeli, 3 taksit vb.)


    }
}