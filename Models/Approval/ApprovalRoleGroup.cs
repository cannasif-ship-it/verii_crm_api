using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("RII_APPROVAL_ROLE_GROUP")]
    public class ApprovalRoleGroup : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
