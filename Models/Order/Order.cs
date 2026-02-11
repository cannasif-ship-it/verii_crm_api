using System;
namespace crm_api.Models
{
    public class Order : BaseHeaderEntity
    {
        public long? PotentialCustomerId { get; set; }
        public Customer? PotentialCustomer { get; set; } // potansiyel müşteri

        public string? ErpCustomerCode { get; set; } = String.Empty;  // e.g., "CUST001"


        public long? ContactId { get; set; } // müşteri temsilcisi ID
        public Contact? Contact { get; set; } // müşteri temsilcisi

        public DateTime? ValidUntil { get; set; } // Sipariş geçerlilik tarihi

        public DateTime? DeliveryDate { get; set; } // Tahmini teslim tarihi

        public long? ShippingAddressId { get; set; } // Teslimat adresi ID
        public ShippingAddress? ShippingAddress { get; set; } // Navigation Property (Teslimat adresi bilgisi)


        public long? RepresentativeId { get; set; } // Satış temsilcisi ID
        public User? Representative { get; set; } // Satış temsilcisi


        public long? ActivityId { get; set; } // Bağlı olduğu activite
        public Activity? Activity { get; set; } // Bağlı olduğu activite

        public ApprovalStatus? Status { get; set; } // Genel durum bilgisi (Workflow)
        public string? Description { get; set; } // Genel açıklama

        public long? PaymentTypeId { get; set; } // Ödeme tipi ID
        public virtual PaymentType? PaymentType { get; set; } // Navigation Property (Ödeme tipi bilgisi)


        public long DocumentSerialTypeId { get; set; }
        public DocumentSerialType? DocumentSerialType { get; set; } // Navigation Property (Belge seri tipi bilgisi)
        public string? OfferType { get; set; } = string.Empty; // Sipariş tipi Yurtiçi yurtdışı

        public DateTime? OfferDate { get; set; } // Sipariş tarihi
        public string? OfferNo { get; set; } // Sipariş numarası
        public string? RevisionNo { get; set; } // Revizyon numarası (varsa)

        public long? RevisionId { get; set; } // Revizyon ID (varsa)
        public string Currency { get; set; } = String.Empty; // Döviz tipi

        public bool HasCustomerSpecificDiscount { get; set; } = false; // Müşteri özelindekiler için indirim var mı?
        public decimal? GeneralDiscountRate { get; set; } // Genel iskonto oranı
        public decimal? GeneralDiscountAmount { get; set; } // Genel iskonto tutarı
        public decimal Total { get; set; } = 0m; // KDV hariç toplam
        public decimal GrandTotal { get; set; } = 0m; // KDV dahil toplam

        public long? SalesTypeDefinitionId { get; set; }
        public SalesTypeDefinition? SalesTypeDefinition { get; set; }

        public string? ErpProjectCode { get; set; }  // max 50, nullable

        public long? QuotationId { get; set; }
        public Quotation? Quotation { get; set; }
        
        //navigation
        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
        public OrderNotes? OrderNotes { get; set; }

    }
}