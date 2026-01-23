using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class ApprovalUserRoleGetDto : BaseEntityDto
    {
        public long UserId { get; set; }
        public string? UserFullName { get; set; }
        public long ApprovalRoleId { get; set; }
        public string? ApprovalRoleName { get; set; }
    }

    public class ApprovalUserRoleCreateDto : BaseCreateDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long ApprovalRoleId { get; set; }
    }

    public class ApprovalUserRoleUpdateDto : BaseUpdateDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long ApprovalRoleId { get; set; }
    }
}
