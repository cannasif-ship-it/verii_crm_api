using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using crm_api.Models;

namespace crm_api.DTOs
{
    public class PricingRuleHeaderGetDto : BaseEntityDto
    {
        public PricingRuleType RuleType { get; set; }
        public string RuleCode { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? ErpCustomerCode { get; set; }
        public short? BranchCode { get; set; }
        public bool PriceIncludesVat { get; set; }
        public bool IsActive { get; set; }
        public List<PricingRuleLineGetDto>? Lines { get; set; }
        public List<PricingRuleSalesmanGetDto>? Salesmen { get; set; }
    }

    public class PricingRuleHeaderCreateDto
    {
        [Required]
        public PricingRuleType RuleType { get; set; }

        [Required]
        [MaxLength(50)]
        public string RuleCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string RuleName { get; set; } = string.Empty;

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public long? CustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public short? BranchCode { get; set; }

        public bool PriceIncludesVat { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public List<PricingRuleLineCreateDto>? Lines { get; set; }
        public List<PricingRuleSalesmanCreateDto>? Salesmen { get; set; }
    }

    public class PricingRuleHeaderUpdateDto
    {
        [Required]
        public PricingRuleType RuleType { get; set; }

        [Required]
        [MaxLength(50)]
        public string RuleCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string RuleName { get; set; } = string.Empty;

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public long? CustomerId { get; set; }

        [MaxLength(50)]
        public string? ErpCustomerCode { get; set; }

        public short? BranchCode { get; set; }

        public bool PriceIncludesVat { get; set; }

        public bool IsActive { get; set; }

        public List<PricingRuleLineUpdateDto>? Lines { get; set; }
        public List<PricingRuleSalesmanUpdateDto>? Salesmen { get; set; }
    }
}
