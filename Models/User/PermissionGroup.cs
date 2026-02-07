using crm_api.Models;

namespace crm_api.Models.UserPermissions
{
    public class PermissionGroup : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSystemAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<PermissionGroupPermission> GroupPermissions { get; set; } = new List<PermissionGroupPermission>();
        public virtual ICollection<UserPermissionGroup> UserGroups { get; set; } = new List<UserPermissionGroup>();
    }
}
