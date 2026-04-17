using crm_api.Modules.AccessControl.Application.Dtos;
using crm_api.Modules.System.Application.Dtos;

namespace crm_api.Modules.Identity.Application.Dtos
{
    public class AppBootstrapUserDto
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class AppBootstrapDto
    {
        public AppBootstrapUserDto User { get; set; } = new();
        public MyPermissionsDto Permissions { get; set; } = new();
        public SystemSettingsDto SystemSettings { get; set; } = new();
    }
}
