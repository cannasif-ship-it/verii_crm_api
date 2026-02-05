namespace crm_api.DTOs.PowerBi
{
    /// <summary>
    /// Response for GET /api/powerbi/reports/{id}/embed-config
    /// </summary>
    public class EmbedConfigDto
    {
        public Guid ReportId { get; set; }
        public string EmbedUrl { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string Expiration { get; set; } = string.Empty;
    }
}
