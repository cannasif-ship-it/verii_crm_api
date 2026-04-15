namespace crm_api.Modules.Integrations.Domain.ReadModels
{
    public class RII_FN_CARIPLASIYER
    {
        public short SUBE_KODU { get; set; }
        public string PLASIYER_KODU { get; set; } = string.Empty;
        public string? PLASIYER_ACIKLAMA { get; set; }
        public string? ISIM { get; set; }
    }
}
