using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class ApprovalActionGetDto : BaseEntityDto
    {
        public long ApprovalRequestId { get; set; }
        public string? ApprovalRequestDescription { get; set; }
        public int StepOrder { get; set; }
        public long ApprovedByUserId { get; set; }
        public string? ApprovedByUserFullName { get; set; }
        public DateTime ActionDate { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
    }

    public class ApprovalActionCreateDto : BaseCreateDto
    {
        [Required]
        public long ApprovalRequestId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        [Required]
        public long ApprovedByUserId { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int Status { get; set; }
    }

    public class ApprovalActionUpdateDto : BaseUpdateDto
    {
        [Required]
        public long ApprovalRequestId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        [Required]
        public long ApprovedByUserId { get; set; }

        public DateTime ActionDate { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
