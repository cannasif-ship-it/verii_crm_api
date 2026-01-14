using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cms_webapi.Models;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_WORKFLOW")]
    public class ApprovalWorkflow : BaseEntity
    {

        [Required]
        public long CustomerTypeId { get; set; } 
        [ForeignKey("CustomerTypeId")]
        public CustomerType CustomerType { get; set; } = null!; // Navigation property
        
        
        
        public long? UserId { get; set; } // Kimin onaylaması gerekiyor?
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal MinAmount { get; set; } // Limit tabanlı onay
        [Column(TypeName = "decimal(18,6)")]
        public decimal MaxAmount { get; set; } // Limit tabanlı onay

    }
}
