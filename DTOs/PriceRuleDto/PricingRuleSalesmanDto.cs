using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class PricingRuleSalesmanGetDto : BaseEntityDto
    {
        public long PricingRuleHeaderId { get; set; }
        public long SalesmanId { get; set; }
        public string? SalesmanFullName { get; set; }
    }

    public class PricingRuleSalesmanCreateDto
    {
        [Required]
        public long PricingRuleHeaderId { get; set; }

        [Required]
        public long SalesmanId { get; set; }
    }

    public class PricingRuleSalesmanUpdateDto
    {
        [Required]
        public long PricingRuleHeaderId { get; set; }

        [Required]
        public long SalesmanId { get; set; }
    }
}
