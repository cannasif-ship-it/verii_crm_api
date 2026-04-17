namespace crm_api.Modules.System.Domain.Entities
{
    public class SystemSetting : BaseEntity
    {
        public string DefaultLanguage { get; set; } = "tr";
        public string DefaultCurrencyCode { get; set; } = "TRY";
        public string DefaultTimeZone { get; set; } = "Europe/Istanbul";
        public string DateFormat { get; set; } = "dd.MM.yyyy";
        public string TimeFormat { get; set; } = "HH:mm";
        public string NumberFormat { get; set; } = "tr-TR";
        public int DecimalPlaces { get; set; } = 2;
    }
}
