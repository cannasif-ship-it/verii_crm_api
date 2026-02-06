namespace crm_api.DTOs
{
    public class Salesmen360CurrencyAmountDto
    {
        public string Currency { get; set; } = "UNKNOWN";
        public decimal DemandAmount { get; set; }
        public decimal QuotationAmount { get; set; }
        public decimal OrderAmount { get; set; }
    }
}
