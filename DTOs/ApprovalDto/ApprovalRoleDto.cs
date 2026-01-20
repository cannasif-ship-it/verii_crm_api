using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ApprovalRoleGetDto : BaseEntityDto
    {
        public long ApprovalRoleGroupId { get; set; }
        public string? ApprovalRoleGroupName { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MaxAmount { get; set; }
    }

    public class ApprovalRoleCreateDto : BaseCreateDto
    {
        [Required]
        public long ApprovalRoleGroupId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public decimal MaxAmount { get; set; }
    }

    public class ApprovalRoleUpdateDto : BaseUpdateDto
    {
        [Required]
        public long ApprovalRoleGroupId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public decimal MaxAmount { get; set; }
    }
}
