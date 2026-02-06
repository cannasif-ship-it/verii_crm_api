namespace crm_api.DTOs.CustomerDto
{
    public class Customer360MonthlyTrendItemDto
    {
        public string Month { get; set; } = string.Empty;
        public int DemandCount { get; set; }
        public int QuotationCount { get; set; }
        public int OrderCount { get; set; }
    }
}
