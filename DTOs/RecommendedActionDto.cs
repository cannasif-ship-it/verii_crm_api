namespace crm_api.DTOs
{
    public class RecommendedActionDto
    {
        public string ActionCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string? Reason { get; set; }
        public DateTime? DueDate { get; set; }
        public string? TargetEntityType { get; set; }
        public long? TargetEntityId { get; set; }
        public string? SourceRuleCode { get; set; }
    }
}
