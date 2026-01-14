using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_RULE")]
    public class ApprovalRule : BaseEntity
    {
        [Required]
        public long ApprovalAuthorityId { get; set; }
        [ForeignKey("ApprovalAuthorityId")]
        public ApprovalAuthority ApprovalAuthority { get; set; } = null!;
        
        public bool ForwardToUpperManagement { get; set; } = false;
        // true: Yetki yetse bile üst yönetime gönder
        // false: Yetki yetiyorsa burada bitir
        
        public ApprovalLevel? ForwardToLevel { get; set; } // Hangi seviyeye gönderilecek?
        // null: Direkt GeneralManager'a
        // Finance: Önce Finance'e, sonra GeneralManager'a
        // GeneralManager: Direkt GeneralManager'a
        
        public bool RequireFinanceApproval { get; set; } = false; // Finance onayı zorunlu mu?
        // true: Finance onaycısı varsa mutlaka onaylamalı
        // false: Finance onaycısı yoksa direkt GeneralManager'a gidebilir
        
        public bool IsActive { get; set; } = true;
    }
}
