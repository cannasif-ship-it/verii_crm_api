namespace crm_api.Modules.Integrations.Application.Dtos.Erp
{
    public class EsnYapMasDto
    {
        public string YapKod { get; set; } = string.Empty;
        public string YapAcik { get; set; } = string.Empty;
        public short SubeKodu { get; set; }
        public string? YplndrStokKod { get; set; }
    }
}
