namespace crm_api.Modules.Integrations.Application.Dtos.Erp
{
    public class CariPlasiyerDto
    {
        public short SubeKodu { get; set; }
        public string PlasiyerKodu { get; set; } = string.Empty;
        public string? PlasiyerAciklama { get; set; }
        public string? Isim { get; set; }
    }
}
