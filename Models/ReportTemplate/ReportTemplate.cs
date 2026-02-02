using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models
{
    /// <summary>
    /// Report template entity for storing JSON-based report designs
    /// </summary>
    [Table("ReportTemplates")]
    public class ReportTemplate : BaseEntity
    {
        /// <summary>
        /// Document type: 0=Demand/Talep, 1=Quotation/Teklif, 2=Order/Sipariş
        /// </summary>
        [Required]
        public DocumentRuleType RuleType { get; set; }

        /// <summary>
        /// Template title/description (Başlık)
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// JSON template data (page config + elements array)
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string TemplateJson { get; set; } = string.Empty;

        /// <summary>
        /// Is this template active/enabled
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// User ID who created this template
        /// </summary>
        public long? CreatedByUserId { get; set; }

        /// <summary>
        /// User ID who last updated this template
        /// </summary>
        public long? UpdatedByUserId { get; set; }
    }

    /// <summary>
    /// Document type enum for report templates
    /// </summary>
    public enum DocumentRuleType
    {
        /// <summary>
        /// Demand/Talep
        /// </summary>
        Demand = 0,

        /// <summary>
        /// Quotation/Teklif
        /// </summary>
        Quotation = 1,

        /// <summary>
        /// Order/Sipariş
        /// </summary>
        Order = 2
    }
}
