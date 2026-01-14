using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_PRICING_RULE_SALESMAN")]
    public class PricingRuleSalesman : BaseEntity
    {
        public long PricingRuleHeaderId { get; set; }

        [ForeignKey(nameof(PricingRuleHeaderId))]
        public PricingRuleHeader PricingRuleHeader { get; set; } = null!;

        public long SalesmanId { get; set; }
        [ForeignKey("SalesmanId")]
        public User Salesman { get; set; } = null!;
    
    }
}

