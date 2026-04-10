using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.Modules.Approval.Application.Dtos
{
    public class ApprovalActionGetDto : BaseEntityDto
    {
        public long ApprovalRequestId { get; set; }
        public long EntityId { get; set; }
        public string? ApprovalRequestDescription { get; set; }
        public string? QuotationOfferNo { get; set; }
        public string? QuotationRevisionNo { get; set; }
        public string? QuotationCustomerName { get; set; }
        public string? QuotationCustomerCode { get; set; }
        public string? QuotationOwnerName { get; set; }
        public decimal? QuotationGrandTotal { get; set; }
        public string? QuotationGrandTotalDisplay { get; set; }
        public int StepOrder { get; set; }
        public long ApprovedByUserId { get; set; }
        public string? ApprovedByUserFullName { get; set; }
        public DateTime ActionDate { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
    }

    public class ApprovalActionCreateDto : BaseCreateDto
    {
        [Required]
        public long ApprovalRequestId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        [Required]
        public long ApprovedByUserId { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int Status { get; set; }
    }

    public class ApprovalActionUpdateDto : BaseUpdateDto
    {
        [Required]
        public long ApprovalRequestId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        [Required]
        public long ApprovedByUserId { get; set; }

        public DateTime ActionDate { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
