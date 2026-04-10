namespace crm_api.Modules.Analytics.Application.Dtos
{
    public class Salesmen360MonthlyTrendItemDto
    {
        public string Month { get; set; } = string.Empty;
        public int DemandCount { get; set; }
        public int QuotationCount { get; set; }
        public int OrderCount { get; set; }
    }
}
