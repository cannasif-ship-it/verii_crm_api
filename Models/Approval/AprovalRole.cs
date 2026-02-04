using System;
using crm_api.Models;

namespace crm_api.Models
{
    public class ApprovalRole : BaseEntity
    {
        public long ApprovalRoleGroupId { get; set; }
        public ApprovalRoleGroup ApprovalRoleGroup { get; set; } = null!;
        public decimal MaxAmount { get; set; }

        /// <summary>
        /// A Bölge Satışçı, B Bölge Satışçı
        /// </summary>
        public string Name { get; set; } = null!;
    }
}