using System;
using System.Collections.Generic;
namespace crm_api.Models
{
    public class DocumentSerialType : BaseEntity
    {
        public PricingRuleType RuleType { get; set; }

        public long? CustomerTypeId { get; set; }   // Bayi, Perakende vs
        public CustomerType? CustomerType { get; set; }

        public long? SalesRepId { get; set; }        // Plasiyer
        public User? SalesRep { get; set; }
        public string? SerialPrefix { get; set; }
        public int? SerialLength { get; set; }  // 4 basamaklı, 5 basamaklı, vs
        public int? SerialStart { get; set; }  // 1000
        public int? SerialCurrent { get; set; }  // 1000
        public int? SerialIncrement { get; set; }  // 1
    }
}