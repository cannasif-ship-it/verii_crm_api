using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_AUTHORITY")]
    public class ApprovalAuthority : BaseEntity
    {
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        
        [Column(TypeName = "decimal(18,6)")]
        public decimal MaxApprovalAmount { get; set; }
        
        public ApprovalLevel ApprovalLevel { get; set; }
        
        public bool CanFinalize { get; set; } = false; // Son onaycı mı?
        
        public bool RequireUpperManagement { get; set; } = true; // Üst yönetime gitmesi zorunlu mu?
        
        public bool IsActive { get; set; } = true;
    }
}
