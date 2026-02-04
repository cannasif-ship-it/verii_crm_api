namespace crm_api.DTOs.PowerBi
{
    public class PowerBIEmbedInfoDto
    {
        public string ReportId { get; set; } = string.Empty;
        public string EmbedUrl { get; set; } = string.Empty;
        public string EmbedToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
