using System.Collections.ObjectModel;

namespace crm_api.Services
{
    public static class NbaActionCatalog
    {
        public const string CustomerFollowUp = "CUSTOMER_FOLLOW_UP";
        public const string QuotationFollowUp = "QUOTATION_FOLLOW_UP";
        public const string RetentionPlan = "RETENTION_PLAN";
        public const string UpsellOffer = "UPSELL_OFFER";
        public const string PaymentReview = "PAYMENT_REVIEW";
        public const string DemandToQuotation = "DEMAND_TO_QUOTATION";

        public const string PipelineCleanup = "PIPELINE_CLEANUP";
        public const string ActivityBoost = "ACTIVITY_BOOST";
        public const string PortfolioRetention = "PORTFOLIO_RETENTION";
        public const string UpsellCampaign = "UPSELL_CAMPAIGN";
        public const string WinrateImprovement = "WINRATE_IMPROVEMENT";

        private static readonly ReadOnlyDictionary<string, NbaActionTemplate> Templates =
            new(new Dictionary<string, NbaActionTemplate>(StringComparer.OrdinalIgnoreCase)
            {
                [CustomerFollowUp] = new("Customer follow-up call", "High", 1, "Customer"),
                [QuotationFollowUp] = new("Follow up open quotations", "High", 1, "Customer"),
                [RetentionPlan] = new("Run retention plan", "High", 2, "Customer"),
                [UpsellOffer] = new("Prepare upsell offer", "Medium", 3, "Customer"),
                [PaymentReview] = new("Run payment risk review", "High", 1, "Customer"),
                [DemandToQuotation] = new("Convert open demands to quotations", "High", 2, "Customer"),

                [PipelineCleanup] = new("Clean open pipeline", "High", 1, "User"),
                [ActivityBoost] = new("Increase activity cadence", "High", 1, "User"),
                [PortfolioRetention] = new("Review risky customer portfolio", "High", 2, "User"),
                [UpsellCampaign] = new("Run upsell campaign", "Medium", 3, "User"),
                [WinrateImprovement] = new("Review lost quotation reasons", "Medium", 2, "User")
            });

        public static bool TryGet(string actionCode, out NbaActionTemplate template)
        {
            return Templates.TryGetValue(actionCode, out template!);
        }

        public sealed class NbaActionTemplate
        {
            public NbaActionTemplate(string title, string defaultPriority, int defaultDueInDays, string targetEntityType)
            {
                Title = title;
                DefaultPriority = defaultPriority;
                DefaultDueInDays = defaultDueInDays;
                TargetEntityType = targetEntityType;
            }

            public string Title { get; }
            public string DefaultPriority { get; }
            public int DefaultDueInDays { get; }
            public string TargetEntityType { get; }
        }
    }
}
