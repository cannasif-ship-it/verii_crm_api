using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class QuotationBulkCreateDto
    {
        [Required]
        public CreateQuotationDto Quotation { get; set; } = null!;

        [Required]
        [MinLength(1, ErrorMessage = "At least one line is required")]
        public List<CreateQuotationLineDto> Lines { get; set; } = new List<CreateQuotationLineDto>();

        public List<QuotationExchangeRateCreateDto>? ExchangeRates { get; set; }
    }
}
