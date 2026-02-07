namespace crm_api.DTOs
{
    public class Salesmen360KpiDto
    {
        public string? Currency { get; set; }
        public int TotalDemands { get; set; }
        public int TotalQuotations { get; set; }
        public int TotalOrders { get; set; }
        public int TotalActivities { get; set; }
        public decimal TotalDemandAmount { get; set; }
        public decimal TotalQuotationAmount { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public List<Salesmen360CurrencyAmountDto> TotalsByCurrency { get; set; } = new();
    }

    public class Salesmen360OverviewDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Salesmen360KpiDto Kpis { get; set; } = new();
        public RevenueQualityDto RevenueQuality { get; set; } = new();
        public List<RecommendedActionDto> RecommendedActions { get; set; } = new();
    }
}
