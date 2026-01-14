using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_PRICING_RULE_HEADER")]
    public class PricingRuleHeader : BaseEntity
    {
        public PricingRuleType RuleType { get; set; }
        public string RuleCode { get; set; } = string.Empty;

        public string RuleName { get; set; } = string.Empty;

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        // KAPSAM
        public long? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public string? ErpCustomerCode { get; set; } = String.Empty;  // e.g., "CUST001"

        // ERP uyumu
        public short? BranchCode { get; set; }

        public bool PriceIncludesVat { get; set; } = false;

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<PricingRuleLine> Lines { get; set; } = new List<PricingRuleLine>();
        public ICollection<PricingRuleSalesman> Salesmen { get; set; } = new List<PricingRuleSalesman>();
    }
}

