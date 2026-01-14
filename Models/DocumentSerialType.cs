using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_DOCUMENT_SERIAL_TYPE")]
    public class DocumentSerialType : BaseEntity
    {
        public PricingRuleType RuleType { get; set; }

        public long? CustomerTypeId { get; set; }   // Bayi, Perakende vs
        [ForeignKey("CustomerTypeId")]
        public CustomerType? CustomerType { get; set; }

        public long? SalesRepId { get; set; }        // Plasiyer
        [ForeignKey("SalesRepId")]
        public User? SalesRep { get; set; }

    }
}

