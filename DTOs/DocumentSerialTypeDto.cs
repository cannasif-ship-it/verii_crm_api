using System;
using System.ComponentModel.DataAnnotations;
using cms_webapi.Models;

namespace cms_webapi.DTOs
{
    public class DocumentSerialTypeGetDto : BaseEntityDto
    {
        public PricingRuleType RuleType { get; set; }
        public long? CustomerTypeId { get; set; }
        public string? CustomerTypeName { get; set; }
        public long? SalesRepId { get; set; }
        public string? SalesRepFullName { get; set; }
    }

    public class DocumentSerialTypeCreateDto
    {
        [Required]
        public PricingRuleType RuleType { get; set; }

        public long? CustomerTypeId { get; set; }

        public long? SalesRepId { get; set; }
    }

    public class DocumentSerialTypeUpdateDto
    {
        [Required]
        public PricingRuleType RuleType { get; set; }

        public long? CustomerTypeId { get; set; }

        public long? SalesRepId { get; set; }
    }
}
