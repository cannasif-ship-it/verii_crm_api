using crm_api.Models;

namespace crm_api.Models
{
    public class UserAuthority : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
    }
}