using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("APPROVAL_FLOW_STEP")]
    public class ApprovalFlowStep : BaseEntity
        {
            /// <summary>
            /// Hangi flow?
            /// </summary>
            [Required]
            public long ApprovalFlowId { get; set; }

            [ForeignKey(nameof(ApprovalFlowId))]
            public ApprovalFlow ApprovalFlow { get; set; } = null!;

            /// <summary>
            /// 1 → 2 → 3
            /// </summary>
            [Required]
            public int StepOrder { get; set; }

            /// <summary>
            /// Bu adımı hangi grup onaylar?
            /// </summary>
            [Required]
            public long ApprovalRoleGroupId { get; set; }

            [ForeignKey(nameof(ApprovalRoleGroupId))]
            public ApprovalRoleGroup ApprovalRoleGroup { get; set; } = null!;
    }
}
