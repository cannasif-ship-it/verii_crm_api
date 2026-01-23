using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using crm_api.Models;

namespace crm_api.Models
{
    [Table("RII_APPROVAL_FLOW")]
    public class ApprovalFlow : BaseEntity
    {
        /// <summary>
        /// Teklif / Talep / Sipariş
        /// </summary>
        [Required]
        public PricingRuleType DocumentType { get; set; }

        /// <summary>
        /// Müşteri tanımı (Örn: 100K Üstü Teklif)
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
