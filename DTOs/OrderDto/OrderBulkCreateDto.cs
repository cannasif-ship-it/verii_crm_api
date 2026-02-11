using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class OrderBulkCreateDto
    {
        [Required]
        public CreateOrderDto Order { get; set; } = null!;

        [Required]
        [MinLength(1, ErrorMessage = "At least one line is required")]
        public List<CreateOrderLineDto> Lines { get; set; } = new List<CreateOrderLineDto>();

        public List<OrderExchangeRateCreateDto>? ExchangeRates { get; set; }

        public CreateOrderNotesDto? OrderNotes { get; set; }
    }
}
