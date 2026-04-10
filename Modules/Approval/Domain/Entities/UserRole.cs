using System;

namespace crm_api.Modules.Approval.Domain.Entities
{
    public class ApprovalUserRole : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; } = null!;
        

        public long ApprovalRoleId { get; set; }
        public ApprovalRole ApprovalRole { get; set; } = null!;
    }
}