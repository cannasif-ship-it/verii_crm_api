namespace crm_api.DTOs.CustomerDto
{
    public class Customer360SimpleItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Status { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}
