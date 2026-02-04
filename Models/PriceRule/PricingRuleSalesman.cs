using System;
using System.Collections.Generic;
namespace crm_api.Models
{
    public class PricingRuleSalesman : BaseEntity
    {
        public long PricingRuleHeaderId { get; set; }
        public PricingRuleHeader PricingRuleHeader { get; set; } = null!;

        public long SalesmanId { get; set; }
        public User Salesman { get; set; } = null!;
    
    }
}