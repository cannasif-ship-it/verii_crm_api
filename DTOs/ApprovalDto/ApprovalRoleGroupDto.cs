using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ApprovalRoleGroupGetDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ApprovalRoleGroupCreateDto : BaseCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }

    public class ApprovalRoleGroupUpdateDto : BaseUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
