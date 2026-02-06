namespace crm_api.DTOs.CustomerDto
{
    public class Customer360CurrencyAmountDto
    {
        public string Currency { get; set; } = "UNKNOWN";
        public decimal DemandAmount { get; set; }
        public decimal QuotationAmount { get; set; }
        public decimal OrderAmount { get; set; }
    }
}
