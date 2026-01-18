using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cms_webapi.Models;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_ROLE")]
    public class ApprovalRole : BaseEntity
    {
        [Required]
        public long ApprovalRoleGroupId { get; set; }

        [ForeignKey(nameof(ApprovalRoleGroupId))]
        public ApprovalRoleGroup ApprovalRoleGroup { get; set; } = null!;

        /// <summary>
        /// A Bölge Satışçı, B Bölge Satışçı
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
