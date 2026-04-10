namespace crm_api.Modules.Customer.Application.Dtos.Customer360
{
    public class Customer360MonthlyTrendItemDto
    {
        public string Month { get; set; } = string.Empty;
        public int DemandCount { get; set; }
        public int QuotationCount { get; set; }
        public int OrderCount { get; set; }
    }
}
