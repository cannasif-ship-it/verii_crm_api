namespace crm_api.Modules.Customer.Application.Dtos.Customer360
{
    public class Customer360CurrencyAmountDto
    {
        public string Currency { get; set; } = "UNKNOWN";
        public decimal DemandAmount { get; set; }
        public decimal QuotationAmount { get; set; }
        public decimal OrderAmount { get; set; }
    }
}
