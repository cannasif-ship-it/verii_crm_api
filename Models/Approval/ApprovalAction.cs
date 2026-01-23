using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("RII_APPROVAL_ACTION")]
    public class ApprovalAction : BaseEntity
    {
        [Required]
        public long ApprovalRequestId { get; set; }

        [ForeignKey(nameof(ApprovalRequestId))]
        public ApprovalRequest ApprovalRequest { get; set; } = null!;

        [Required]
        public int StepOrder { get; set; }

        [Required]
        public long ApprovedByUserId { get; set; }

        [ForeignKey(nameof(ApprovedByUserId))]
        public User ApprovedByUser { get; set; } = null!;

        public DateTime ActionDate { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;
    }

}
