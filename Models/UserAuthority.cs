using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cms_webapi.Models;

namespace cms_webapi.Models
{
    [Table("RII_USER_AUTHORITY")]
    public class UserAuthority : BaseEntity
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;
    }
}