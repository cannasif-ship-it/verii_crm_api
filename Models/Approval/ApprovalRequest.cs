using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("RII_APPROVAL_REQUEST")]
    public class ApprovalRequest : BaseEntity
    {
        /// <summary>
        /// TeklifId / SiparişId
        /// </summary>
        [Required]
        public long EntityId { get; set; }

        [Required]
        public PricingRuleType DocumentType { get; set; }

        [Required]
        public long ApprovalFlowId { get; set; }

        [ForeignKey(nameof(ApprovalFlowId))]
        public ApprovalFlow ApprovalFlow { get; set; } = null!;

        public int CurrentStep { get; set; } = 1;

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;
    }
}
