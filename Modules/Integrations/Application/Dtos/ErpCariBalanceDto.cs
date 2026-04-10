namespace crm_api.Modules.Integrations.Application.Dtos.Erp
{
    public class ErpCariBalanceDto
    {
        public string CariKod { get; set; } = string.Empty;
        public decimal NetBakiye { get; set; }
        public string BakiyeDurumu { get; set; } = string.Empty;
        public decimal BakiyeTutari { get; set; }
    }
}
