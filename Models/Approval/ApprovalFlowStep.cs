using System;
using crm_api.Models;

namespace crm_api.Models
{
    public class ApprovalFlowStep : BaseEntity
        {
            /// <summary>
            /// Hangi flow?
            /// </summary>
            public long ApprovalFlowId { get; set; }
            public ApprovalFlow ApprovalFlow { get; set; } = null!;

            /// <summary>
            /// 1 → 2 → 3
            /// </summary>
            public int StepOrder { get; set; }

            /// <summary>
            /// Bu adımı hangi grup onaylar?
            /// </summary>
            public long ApprovalRoleGroupId { get; set; }
            public ApprovalRoleGroup ApprovalRoleGroup { get; set; } = null!;
    }
}