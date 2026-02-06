namespace crm_api.DTOs.CustomerDto
{
    public class Customer360AmountComparisonDto
    {
        public decimal Last12MonthsOrderAmount { get; set; }
        public decimal OpenQuotationAmount { get; set; }
        public decimal OpenOrderAmount { get; set; }
    }
}
