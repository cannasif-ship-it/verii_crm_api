namespace crm_api.DTOs
{
    public class Salesmen360AnalyticsChartsDto
    {
        public List<Salesmen360MonthlyTrendItemDto> MonthlyTrend { get; set; } = new();
        public Salesmen360DistributionDto Distribution { get; set; } = new();
        public Salesmen360AmountComparisonDto AmountComparison { get; set; } = new();
        public List<Salesmen360AmountComparisonDto> AmountComparisonByCurrency { get; set; } = new();
    }
}
