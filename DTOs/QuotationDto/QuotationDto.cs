using System;
using System.ComponentModel.DataAnnotations;
using crm_api.Models;

namespace crm_api.DTOs
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
        public long? DemandId { get; set; }
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

        public long? DemandId { get; set; }
    }

    public class UpdateQuotationDto
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

        public long? DemandId { get; set; }
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
        public long? DemandId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class StartApprovalFlowDto
    {
        [Required]
        public long EntityId { get; set; }

        [Required]
        public PricingRuleType DocumentType { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }

    public class ApproveActionDto
    {
        [Required]
        public long ApprovalActionId { get; set; }
    }

    public class RejectActionDto
    {
        [Required]
        public long ApprovalActionId { get; set; }

        [MaxLength(500)]
        public string? RejectReason { get; set; }
    }
}
