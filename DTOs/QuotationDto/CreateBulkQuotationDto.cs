using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{
    public class CreateBulkQuotationDto
    {
        [Required]
        public CreateQuotationDto Header { get; set; } = new CreateQuotationDto();
        [Required]
        public List<CreateBulkQuotationLineDto> Lines { get; set; } = new List<CreateBulkQuotationLineDto>();
        [Required]
        public long customerTypeId { get; set; }
    }

    public class CreateBulkQuotationLineDto
    {
        [MaxLength(100)]
        public string ProductCode { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountAmount1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountAmount2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public decimal DiscountAmount3 { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineGrandTotal { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
    }

    public class CreateBulkQuotationResultDto
    {
        public QuotationDto Quotation { get; set; } = new QuotationDto();
        public List<QuotationLineDto> Lines { get; set; } = new List<QuotationLineDto>();
    }
}