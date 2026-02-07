using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class UserPermissionGroupDto
    {
        public long UserId { get; set; }
        public List<long> PermissionGroupIds { get; set; } = new List<long>();
        public List<string> PermissionGroupNames { get; set; } = new List<string>();
    }

    public class SetUserPermissionGroupsDto
    {
        [Required]
        public List<long> PermissionGroupIds { get; set; } = new List<long>();
    }
}
