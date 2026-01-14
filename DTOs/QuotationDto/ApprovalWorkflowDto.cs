using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class ApprovalWorkflowDto : BaseEntityDto
    {
        public long CustomerTypeId { get; set; }
        public string? CustomerTypeName { get; set; }
        public long? UserId { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
    }

    public class CreateApprovalWorkflowDto
    {
        [Required]
        public long CustomerTypeId { get; set; }
        public long? UserId { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
    }

    public class UpdateApprovalWorkflowDto
    {
        [Required]
        public long CustomerTypeId { get; set; }
        public long? UserId { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
    }
}
