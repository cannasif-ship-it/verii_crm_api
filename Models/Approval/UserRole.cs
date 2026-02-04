using System;
using crm_api.Models;

namespace crm_api.Models
{
    public class ApprovalUserRole : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; } = null!;
        

        public long ApprovalRoleId { get; set; }
        public ApprovalRole ApprovalRole { get; set; } = null!;
    }
}