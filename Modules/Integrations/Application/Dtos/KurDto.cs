namespace crm_api.Modules.Integrations.Application.Dtos.Erp
{
    public class KurDto
    {
       public int DovizTipi { get; set; }
        public string? DovizIsmi { get; set; } = string.Empty;
        public double? KurDegeri { get; set; }
    }
}
