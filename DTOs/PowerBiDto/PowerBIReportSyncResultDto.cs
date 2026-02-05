namespace crm_api.DTOs.PowerBi
{
    public class PowerBIReportSyncResultDto
    {
        public int TotalRemote { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Reactivated { get; set; }
        public int Deleted { get; set; }
    }
}
