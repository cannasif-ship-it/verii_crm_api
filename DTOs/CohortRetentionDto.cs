namespace crm_api.DTOs
{
    public class CohortRetentionDto
    {
        public string CohortKey { get; set; } = string.Empty;
        public int CohortSize { get; set; }
        public List<CohortRetentionPointDto> Points { get; set; } = new();
    }

    public class CohortRetentionPointDto
    {
        public int PeriodIndex { get; set; }
        public string PeriodMonth { get; set; } = string.Empty;
        public int RetainedCount { get; set; }
        public decimal RetentionRate { get; set; }
    }
}
