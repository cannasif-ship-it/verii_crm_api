using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_APPROVAL_QUEUE")]
    public class ApprovalQueue : BaseEntity
    {
        [Required]
        public long QuotationId { get; set; }
        [ForeignKey("QuotationId")]
        public Quotation Quotation { get; set; } = null!;
        
        public long? QuotationLineId { get; set; }
        [ForeignKey("QuotationLineId")]
        public QuotationLine? QuotationLine { get; set; }
        
        [Required]
        public long AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public User AssignedToUser { get; set; } = null!;
        
        public ApprovalLevel ApprovalLevel { get; set; }
        
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;
        
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedAt { get; set; }
        
        public int SequenceOrder { get; set; } = 1; // Onay sırası
        
        public bool IsCurrent { get; set; } = true; // Şu anki onay adımı mı?
        
        [MaxLength(500)]
        public string? Note { get; set; } // Onay/Red notu
    }
}
