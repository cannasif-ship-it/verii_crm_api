using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class QuotationDto : BaseHeaderEntityDto
    {
        public long? PotentialCustomerId { get; set; }
        public string? PotentialCustomerName { get; set; }
        public string? ErpCustomerCode { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? ShippingAddressId { get; set; }
        public string? ShippingAddressText { get; set; }
        public long? RepresentativeId { get; set; }
        public string? RepresentativeName { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public long? PaymentTypeId { get; set; }
        public string? PaymentTypeName { get; set; }
        public string OfferType { get; set; } = string.Empty;
        public DateTime? OfferDate { get; set; }
        public string? OfferNo { get; set; }
        public string? RevisionNo { get; set; }
        public long? RevisionId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CreateQuotationDto
    {
        public long? PotentialCustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long? ShippingAddressId { get; set; }

        public long? RepresentativeId { get; set; }

        public int? Status { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public long? PaymentTypeId { get; set; }

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
    }

    public class UpdateQuotationDto
    {
        public long? PotentialCustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long? ShippingAddressId { get; set; }

        public long? RepresentativeId { get; set; }

        public int? Status { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public long? PaymentTypeId { get; set; }

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
    }

    public class QuotationGetDto : BaseHeaderEntityDto
    {
        public long? PotentialCustomerId { get; set; }
        public string? PotentialCustomerName { get; set; }
        public string? ErpCustomerCode { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? ShippingAddressId { get; set; }
        public string? ShippingAddressText { get; set; }
        public long? RepresentativeId { get; set; }
        public string? RepresentativeName { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public long? PaymentTypeId { get; set; }
        public string? PaymentTypeName { get; set; }
        public string OfferType { get; set; } = string.Empty;
        public DateTime? OfferDate { get; set; }
        public string? OfferNo { get; set; }
        public string? RevisionNo { get; set; }
        public long? RevisionId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
