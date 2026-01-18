using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ApprovalFlowStepGetDto : BaseEntityDto
    {
        public long ApprovalFlowId { get; set; }
        public string? ApprovalFlowDescription { get; set; }
        public int StepOrder { get; set; }
        public long ApprovalRoleGroupId { get; set; }
        public string? ApprovalRoleGroupName { get; set; }
    }

    public class ApprovalFlowStepCreateDto : BaseCreateDto
    {
        [Required]
        public long ApprovalFlowId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        [Required]
        public long ApprovalRoleGroupId { get; set; }
    }

    public class ApprovalFlowStepUpdateDto : BaseUpdateDto
    {
        [Required]
        public long ApprovalFlowId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        [Required]
        public long ApprovalRoleGroupId { get; set; }
    }
}
