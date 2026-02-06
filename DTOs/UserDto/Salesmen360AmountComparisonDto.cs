namespace crm_api.DTOs
{
    public class Salesmen360AmountComparisonDto
    {
        public string? Currency { get; set; }
        public decimal Last12MonthsOrderAmount { get; set; }
        public decimal OpenQuotationAmount { get; set; }
        public decimal OpenOrderAmount { get; set; }
    }
}
