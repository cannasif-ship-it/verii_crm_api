using crm_api.Models;

namespace crm_api.Models.UserPermissions
{
    public class PermissionDefinition : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<PermissionGroupPermission> GroupPermissions { get; set; } = new List<PermissionGroupPermission>();
    }
}
