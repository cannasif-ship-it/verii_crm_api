using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cms_webapi.Models;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_ROLE_GROUP")]
    public class ApprovalRoleGroup : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
