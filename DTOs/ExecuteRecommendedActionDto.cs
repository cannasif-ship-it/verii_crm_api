using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class ExecuteRecommendedActionDto
    {
        [Required]
        public string ActionCode { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Reason { get; set; }
        public int? DueInDays { get; set; }
        public string? Priority { get; set; }
        public long? AssignedUserId { get; set; }
    }
}
