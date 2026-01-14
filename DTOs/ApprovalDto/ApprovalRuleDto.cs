using System.ComponentModel.DataAnnotations;
using cms_webapi.Models;

namespace cms_webapi.DTOs
{
    public class ApprovalRuleGetDto : BaseEntityDto
    {
        public long ApprovalAuthorityId { get; set; }
        public string? ApprovalAuthorityUserFullName { get; set; }
        public bool ForwardToUpperManagement { get; set; }
        public ApprovalLevel? ForwardToLevel { get; set; }
        public bool RequireFinanceApproval { get; set; }
        public bool IsActive { get; set; }
    }

    public class ApprovalRuleCreateDto
    {
        [Required]
        public long ApprovalAuthorityId { get; set; }

        public bool ForwardToUpperManagement { get; set; } = false;

        public ApprovalLevel? ForwardToLevel { get; set; }

        public bool RequireFinanceApproval { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }

    public class ApprovalRuleUpdateDto
    {
        [Required]
        public long ApprovalAuthorityId { get; set; }

        public bool ForwardToUpperManagement { get; set; }

        public ApprovalLevel? ForwardToLevel { get; set; }

        public bool RequireFinanceApproval { get; set; }

        public bool IsActive { get; set; }
    }
}
