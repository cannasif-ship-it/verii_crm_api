using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class DemandBulkCreateDto
    {
        [Required]
        public CreateDemandDto Demand { get; set; } = null!;

        [Required]
        [MinLength(1, ErrorMessage = "At least one line is required")]
        public List<CreateDemandLineDto> Lines { get; set; } = new List<CreateDemandLineDto>();

        public List<DemandExchangeRateCreateDto>? ExchangeRates { get; set; }
    }
}
