using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ApprovalFlowGetDto : BaseEntityDto
    {
        public int DocumentType { get; set; }
        public string? DocumentTypeName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class ApprovalFlowCreateDto : BaseCreateDto
    {
        [Required]
        public int DocumentType { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class ApprovalFlowUpdateDto : BaseUpdateDto
    {
        [Required]
        public int DocumentType { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
