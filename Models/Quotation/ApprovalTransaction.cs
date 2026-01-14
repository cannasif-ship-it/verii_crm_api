using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_TRANSACTION")]
    public class ApprovalTransaction : BaseEntity
    {

        public long DocumentId { get; set; } // QuotationId
        [ForeignKey("DocumentId")]
        public Quotation? Quotation { get; set; } // Navigation property

        public long? LineId { get; set; } // Satır bazlıysa
        [ForeignKey("LineId")]
        public QuotationLine? QuotationLine { get; set; } // Navigation property

        public ApprovalLevel ApprovalLevel { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;

        public long? ApprovedByUserId { get; set; }
        [ForeignKey("ApprovedByUserId")]
        public User? ApprovedByUser { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ActionDate { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
