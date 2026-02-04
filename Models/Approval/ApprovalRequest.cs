using System;
using crm_api.Models;

namespace crm_api.Models
{
    public class ApprovalRequest : BaseEntity
    {
        /// <summary>
        /// TeklifId / SiparişId
        /// </summary>
        public long EntityId { get; set; }

        public PricingRuleType DocumentType { get; set; }

        public long ApprovalFlowId { get; set; }
        public ApprovalFlow ApprovalFlow { get; set; } = null!;

        public int CurrentStep { get; set; } = 1;

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;
    }
}