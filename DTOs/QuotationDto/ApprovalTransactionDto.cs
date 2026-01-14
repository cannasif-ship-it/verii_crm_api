using System;
using System.ComponentModel.DataAnnotations;
using cms_webapi.Models;

namespace cms_webapi.DTOs
{
    public class ApprovalTransactionGetDto : BaseEntityDto
    {
        public long DocumentId { get; set; }
        public string? QuotationOfferNo { get; set; }
        public long? LineId { get; set; }
        public string? LineProductCode { get; set; }
        public ApprovalLevel ApprovalLevel { get; set; }
        public ApprovalStatus Status { get; set; }
        public long? ApprovedByUserId { get; set; }
        public string? ApprovedByUserFullName { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Note { get; set; }
    }

    public class ApprovalTransactionCreateDto
    {
        [Required]
        public long DocumentId { get; set; }

        public long? LineId { get; set; }

        [Required]
        public ApprovalLevel ApprovalLevel { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;

        public long? ApprovedByUserId { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ActionDate { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }

    public class ApprovalTransactionUpdateDto
    {
        [Required]
        public long DocumentId { get; set; }

        public long? LineId { get; set; }

        [Required]
        public ApprovalLevel ApprovalLevel { get; set; }

        [Required]
        public ApprovalStatus Status { get; set; }

        public long? ApprovedByUserId { get; set; }

        public DateTime RequestedAt { get; set; }

        public DateTime? ActionDate { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
