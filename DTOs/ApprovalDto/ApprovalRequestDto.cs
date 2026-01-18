using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ApprovalRequestGetDto : BaseEntityDto
    {
        public long EntityId { get; set; }
        public int DocumentType { get; set; }
        public string? DocumentTypeName { get; set; }
        public long ApprovalFlowId { get; set; }
        public string? ApprovalFlowDescription { get; set; }
        public int CurrentStep { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
    }

    public class ApprovalRequestCreateDto : BaseCreateDto
    {
        [Required]
        public long EntityId { get; set; }

        [Required]
        public int DocumentType { get; set; }

        [Required]
        public long ApprovalFlowId { get; set; }

        public int CurrentStep { get; set; } = 1;

        [Required]
        public int Status { get; set; }
    }

    public class ApprovalRequestUpdateDto : BaseUpdateDto
    {
        [Required]
        public long EntityId { get; set; }

        [Required]
        public int DocumentType { get; set; }

        [Required]
        public long ApprovalFlowId { get; set; }

        public int CurrentStep { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
