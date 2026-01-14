using System.ComponentModel.DataAnnotations;
using cms_webapi.Models;

namespace cms_webapi.DTOs
{
    public class ApprovalQueueGetDto : BaseEntityDto
    {
        public long QuotationId { get; set; }
        public string? QuotationOfferNo { get; set; }
        public long? QuotationLineId { get; set; }
        public string? QuotationLineProductCode { get; set; }
        public long AssignedToUserId { get; set; }
        public string? AssignedToUserFullName { get; set; }
        public ApprovalLevel ApprovalLevel { get; set; }
        public ApprovalStatus Status { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int SequenceOrder { get; set; }
        public bool IsCurrent { get; set; }
        public string? Note { get; set; }
    }

    public class ApprovalQueueCreateDto
    {
        [Required]
        public long QuotationId { get; set; }

        public long? QuotationLineId { get; set; }

        [Required]
        public long AssignedToUserId { get; set; }

        [Required]
        public ApprovalLevel ApprovalLevel { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public int SequenceOrder { get; set; } = 1;

        public bool IsCurrent { get; set; } = true;

        [MaxLength(500)]
        public string? Note { get; set; }
    }

    public class ApprovalQueueUpdateDto
    {
        [Required]
        public long QuotationId { get; set; }

        public long? QuotationLineId { get; set; }

        [Required]
        public long AssignedToUserId { get; set; }

        [Required]
        public ApprovalLevel ApprovalLevel { get; set; }

        [Required]
        public ApprovalStatus Status { get; set; }

        public DateTime AssignedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        [Required]
        public int SequenceOrder { get; set; }

        public bool IsCurrent { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }

    public class ApprovalNoteDto
    {
        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
