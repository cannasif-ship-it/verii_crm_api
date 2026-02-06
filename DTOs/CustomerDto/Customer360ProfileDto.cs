namespace crm_api.DTOs.CustomerDto
{
    public class Customer360ProfileDto
    {
        public long Id { get; set; }
        public string? CustomerCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public string? TcknNumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? SalesRepCode { get; set; }
        public string? GroupCode { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool IsERPIntegrated { get; set; }
        public DateTime? LastSyncDate { get; set; }
    }
}
