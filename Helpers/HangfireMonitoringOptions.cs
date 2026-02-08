namespace crm_api.Helpers
{
    public class HangfireMonitoringOptions
    {
        public const string SectionName = "HangfireMonitoring";

        public int FinalRetryCountThreshold { get; set; } = 3;
        public List<string> CriticalJobs { get; set; } = new();
        public List<string> AlertEmails { get; set; } = new();
    }
}
