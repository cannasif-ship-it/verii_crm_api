using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("RII_APPROVAL_ROLE")]
    public class ApprovalRole : BaseEntity
    {
        [Required]
        public long ApprovalRoleGroupId { get; set; }

        [ForeignKey(nameof(ApprovalRoleGroupId))]
        public ApprovalRoleGroup ApprovalRoleGroup { get; set; } = null!;


        [Column(TypeName = "decimal(18,6)")]
        public decimal MaxAmount { get; set; }

        /// <summary>
        /// A Bölge Satışçı, B Bölge Satışçı
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
