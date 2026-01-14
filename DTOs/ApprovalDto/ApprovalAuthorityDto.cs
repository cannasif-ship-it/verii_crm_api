using System.ComponentModel.DataAnnotations;
using cms_webapi.Models;

namespace cms_webapi.DTOs
{
    public class ApprovalAuthorityGetDto : BaseEntityDto
    {
        public long UserId { get; set; }
        public string? UserFullName { get; set; }
        public decimal MaxApprovalAmount { get; set; }
        public ApprovalLevel ApprovalLevel { get; set; }
        public bool CanFinalize { get; set; }
        public bool RequireUpperManagement { get; set; }
        public bool IsActive { get; set; }
    }

    public class ApprovalAuthorityCreateDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "MaxApprovalAmount must be greater than or equal to 0")]
        public decimal MaxApprovalAmount { get; set; }

        [Required]
        public ApprovalLevel ApprovalLevel { get; set; }

        public bool CanFinalize { get; set; } = false;

        public bool RequireUpperManagement { get; set; } = true;

        public bool IsActive { get; set; } = true;
    }

    public class ApprovalAuthorityUpdateDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "MaxApprovalAmount must be greater than or equal to 0")]
        public decimal MaxApprovalAmount { get; set; }

        [Required]
        public ApprovalLevel ApprovalLevel { get; set; }

        public bool CanFinalize { get; set; }

        public bool RequireUpperManagement { get; set; }

        public bool IsActive { get; set; }
    }
}
