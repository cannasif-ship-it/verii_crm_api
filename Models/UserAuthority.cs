using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("RII_USER_AUTHORITY")]
    public class UserAuthority : BaseEntity
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;
    }
}
