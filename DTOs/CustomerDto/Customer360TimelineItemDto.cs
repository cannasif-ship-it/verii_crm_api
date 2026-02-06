namespace crm_api.DTOs.CustomerDto
{
    public class Customer360TimelineItemDto
    {
        public string Type { get; set; } = string.Empty; // Demand|Quotation|Order|Activity
        public long ItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Status { get; set; }
        public decimal? Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
