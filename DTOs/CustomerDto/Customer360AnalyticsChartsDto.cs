namespace crm_api.DTOs.CustomerDto
{
    public class Customer360AnalyticsChartsDto
    {
        public List<Customer360MonthlyTrendItemDto> MonthlyTrend { get; set; } = new();
        public Customer360DistributionDto Distribution { get; set; } = new();
        public Customer360AmountComparisonDto AmountComparison { get; set; } = new();
    }
}
