namespace crm_api.DTOs.CustomerDto
{
    public class Customer360KpiDto
    {
        public int TotalDemands { get; set; }
        public int TotalQuotations { get; set; }
        public int TotalOrders { get; set; }
        public int OpenQuotations { get; set; }
        public int OpenOrders { get; set; }
        public DateTime? LastActivityDate { get; set; }
    }
}
