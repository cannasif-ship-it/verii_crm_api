
namespace crm_api.Modules.AccessControl.Domain.Entities
{
    public class PermissionGroupPermission : BaseEntity
    {
        public long PermissionGroupId { get; set; }
        public PermissionGroup PermissionGroup { get; set; } = null!;

        public long PermissionDefinitionId { get; set; }
        public PermissionDefinition PermissionDefinition { get; set; } = null!;
    }
}
