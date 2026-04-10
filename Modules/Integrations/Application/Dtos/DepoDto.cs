namespace crm_api.Modules.Integrations.Application.Dtos.Erp
{
    public class DepoDto
    {
        public short DepoKodu { get; set; }
        public string? DepoIsmi { get; set; }
        public string? CariKodu { get; set; }
        public short SubeKodu { get; set; }
        public char? DepoKilitLe { get; set; }
        public char? Eksibakiye { get; set; }
    }
}
