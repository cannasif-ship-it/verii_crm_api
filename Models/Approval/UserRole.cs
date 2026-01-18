using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cms_webapi.Models;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_USER_ROLE")]
    public class ApprovalUserRole : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        

        [Required]
        public long ApprovalRoleId { get; set; }

        [ForeignKey(nameof(ApprovalRoleId))]
        public ApprovalRole ApprovalRole { get; set; } = null!;
    }
}
