using QuickQuotationEntity = crm_api.Modules.Quotation.Domain.Entities.TempQuotattion;
using QuickQuotationLineEntity = crm_api.Modules.Quotation.Domain.Entities.TempQuotattionLine;

namespace crm_api.Modules.PdfBuilder.Domain.Entities
{
    /// <summary>
    /// Strong ownership record for quick quotation line images.
    /// </summary>
    public class QuickQuotationImageUsage : BaseEntity
    {
        public long QuickQuotationImageId { get; set; }
        public QuickQuotationImage QuickQuotationImage { get; set; } = null!;

        public long TempQuotattionId { get; set; }
        public QuickQuotationEntity TempQuotattion { get; set; } = null!;

        public long TempQuotattionLineId { get; set; }
        public QuickQuotationLineEntity TempQuotattionLine { get; set; } = null!;

        public string ProductCode { get; set; } = string.Empty;
    }
}
