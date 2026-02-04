using System;
using crm_api.Models;

namespace crm_api.Models
{
    public class ApprovalAction : BaseEntity
    {
        public long ApprovalRequestId { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; } = null!;

        public int StepOrder { get; set; }

        public long ApprovedByUserId { get; set; }
        public User ApprovedByUser { get; set; } = null!;

        public DateTime ActionDate { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;
    }

}