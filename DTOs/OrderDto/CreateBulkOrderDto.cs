using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class CreateBulkOrderDto
    {
        [Required]
        public CreateOrderDto Header { get; set; } = new CreateOrderDto();
        [Required]
        public List<CreateBulkOrderLineDto> Lines { get; set; } = new List<CreateBulkOrderLineDto>();
        [Required]
        public long customerTypeId { get; set; }
    }

    public class CreateBulkOrderLineDto
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
        [MaxLength(50)]
        public string? ErpProjectCode { get; set; }
    }

    public class CreateBulkOrderResultDto
    {
        public OrderDto Order { get; set; } = new OrderDto();
        public List<OrderLineDto> Lines { get; set; } = new List<OrderLineDto>();
    }
}
