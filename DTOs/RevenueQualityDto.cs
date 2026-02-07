namespace crm_api.DTOs
{
    public class RevenueQualityDto
    {
        public string? CohortKey { get; set; }
        public decimal? RetentionRate { get; set; }
        public string? RfmSegment { get; set; }
        public decimal? Ltv { get; set; }
        public decimal? ChurnRiskScore { get; set; }
        public decimal? UpsellPropensityScore { get; set; }
        public decimal? PaymentBehaviorScore { get; set; }
        public string? DataQualityNote { get; set; }
    }
}
