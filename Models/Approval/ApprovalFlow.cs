using System;
using crm_api.Models;

namespace crm_api.Models
{
    public class ApprovalFlow : BaseEntity
    {
        /// <summary>
        /// Teklif / Talep / Sipariş
        /// </summary>
        public PricingRuleType DocumentType { get; set; }

        /// <summary>
        /// Müşteri tanımı (Örn: 100K Üstü Teklif)
        /// </summary>
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

}