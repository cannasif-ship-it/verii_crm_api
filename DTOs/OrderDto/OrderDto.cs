using System;
using System.ComponentModel.DataAnnotations;
using crm_api.Models;

namespace crm_api.DTOs
{
    public class OrderDto : BaseHeaderEntityDto
    {
        public long? PotentialCustomerId { get; set; }
        public string? PotentialCustomerName { get; set; }
        public string? ErpCustomerCode { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? ShippingAddressId { get; set; }
        public string? ShippingAddressText { get; set; }
        public long? RepresentativeId { get; set; }
        public string? RepresentativeName { get; set; }
        public ApprovalStatus? Status { get; set; }
        public string? Description { get; set; }
        public long? PaymentTypeId { get; set; }
        public string? PaymentTypeName { get; set; }
        public long DocumentSerialTypeId { get; set; }
        public string? DocumentSerialTypeName { get; set; }
        public string OfferType { get; set; } = string.Empty;
        public DateTime? OfferDate { get; set; }
        public string? OfferNo { get; set; }
        public string? RevisionNo { get; set; }
        public long? RevisionId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal? GeneralDiscountRate { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public long? QuotationId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CreateOrderDto
    {
        public long? PotentialCustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long? ShippingAddressId { get; set; }

        public long? RepresentativeId { get; set; }

        public ApprovalStatus? Status { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public long? PaymentTypeId { get; set; }

        public long DocumentSerialTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OfferType { get; set; } = string.Empty;

        public DateTime? OfferDate { get; set; }

        [MaxLength(50)]
        public string? OfferNo { get; set; }

        [MaxLength(50)]
        public string? RevisionNo { get; set; }

        public long? RevisionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Currency { get; set; } = string.Empty;

        public decimal? GeneralDiscountRate { get; set; }

        public decimal? GeneralDiscountAmount { get; set; }

        public long? QuotationId { get; set; }
    }

    public class UpdateOrderDto
    {
        public long? PotentialCustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long? ShippingAddressId { get; set; }

        public long? RepresentativeId { get; set; }

        public ApprovalStatus? Status { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public long? PaymentTypeId { get; set; }

        public long DocumentSerialTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OfferType { get; set; } = string.Empty;

        public DateTime? OfferDate { get; set; }

        [MaxLength(50)]
        public string? OfferNo { get; set; }

        [MaxLength(50)]
        public string? RevisionNo { get; set; }

        public long? RevisionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Currency { get; set; } = string.Empty;

        public decimal? GeneralDiscountRate { get; set; }

        public decimal? GeneralDiscountAmount { get; set; }

        public long? QuotationId { get; set; }
    }

    public class OrderGetDto : BaseHeaderEntityDto
    {
        public long? PotentialCustomerId { get; set; }
        public string? PotentialCustomerName { get; set; }
        public string? ErpCustomerCode { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? ShippingAddressId { get; set; }
        public string? ShippingAddressText { get; set; }
        public long? RepresentativeId { get; set; }
        public string? RepresentativeName { get; set; }
        public ApprovalStatus? Status { get; set; }
        public string? Description { get; set; }
        public long? PaymentTypeId { get; set; }
        public string? PaymentTypeName { get; set; }
        public long DocumentSerialTypeId { get; set; }
        public string? DocumentSerialTypeName { get; set; }
        public string OfferType { get; set; } = string.Empty;
        public DateTime? OfferDate { get; set; }
        public string? OfferNo { get; set; }
        public string? RevisionNo { get; set; }
        public long? RevisionId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal? GeneralDiscountRate { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public long? QuotationId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
