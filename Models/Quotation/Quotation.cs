using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models
{
    [Table("RII_QUOTATION")]
    public class Quotation : BaseHeaderEntity
    {
        [ForeignKey("PotentialCustomer")]
        public long? PotentialCustomerId { get; set; }
        public Customer? PotentialCustomer { get; set; } // potansiyel müşteri

        public string? ErpCustomerCode { get; set; } = String.Empty;  // e.g., "CUST001"


        public long? ContactId { get; set; } // müşteri temsilcisi ID
        [ForeignKey("ContactId")]
        public Contact? Contact { get; set; } // müşteri temsilcisi

        public DateTime? ValidUntil { get; set; } // Teklif geçerlilik tarihi

        public DateTime? DeliveryDate { get; set; } // Tahmini teslim tarihi

        public long? ShippingAddressId { get; set; } // Teslimat adresi ID

        [ForeignKey("ShippingAddressId")]
        public ShippingAddress? ShippingAddress { get; set; } // Navigation Property (Teslimat adresi bilgisi)


        public long? RepresentativeId { get; set; } // Satış temsilcisi ID
        [ForeignKey("RepresentativeId")]
        public User? Representative { get; set; } // Satış temsilcisi


        public long? ActivityId { get; set; } // Bağlı olduğu activite
        [ForeignKey("ActivityId")]
        public Activity? Activity { get; set; } // Bağlı olduğu activite

        public ApprovalStatus? Status { get; set; } // Genel durum bilgisi (Workflow)

        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; } // Genel açıklama

        public long? PaymentTypeId { get; set; } // Ödeme tipi ID
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType? PaymentType { get; set; } // Navigation Property (Ödeme tipi bilgisi)


        public long DocumentSerialTypeId { get; set; }

        [ForeignKey(nameof(DocumentSerialTypeId))]
        public DocumentSerialType? DocumentSerialType { get; set; } // Navigation Property (Belge seri tipi bilgisi)

        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? OfferType { get; set; } = string.Empty; // Teklif tipi Yurtiçi yurtdışı

        public DateTime? OfferDate { get; set; } // Teklif tarihi

        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? OfferNo { get; set; } // Teklif numarası

        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string? RevisionNo { get; set; } // Revizyon numarası (varsa)

        public long? RevisionId { get; set; } // Revizyon ID (varsa)

        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Currency { get; set; } = String.Empty; // Döviz tipi

        [Column(TypeName = "bit")]
        public bool HasCustomerSpecificDiscount { get; set; } = false; // Müşteri özelindekiler için indirim var mı?

        [Column(TypeName = "decimal(18,6)")]
        public decimal Total { get; set; } = 0m; // KDV hariç toplam

        [Column(TypeName = "decimal(18,6)")]
        public decimal GrandTotal { get; set; } = 0m; // KDV dahil toplam
        
        //navigation
        public ICollection<QuotationLine> Lines { get; set; } = new List<QuotationLine>();

    }
}
