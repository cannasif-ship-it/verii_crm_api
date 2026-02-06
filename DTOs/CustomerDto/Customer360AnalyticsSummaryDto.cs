namespace crm_api.DTOs.CustomerDto
{
    public class Customer360AnalyticsSummaryDto
    {
        public string? Currency { get; set; }
        public decimal Last12MonthsOrderAmount { get; set; }
        public decimal OpenQuotationAmount { get; set; }
        public decimal OpenOrderAmount { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public int ActivityCount { get; set; }
        public List<Customer360CurrencyAmountDto> TotalsByCurrency { get; set; } = new();
    }
}
