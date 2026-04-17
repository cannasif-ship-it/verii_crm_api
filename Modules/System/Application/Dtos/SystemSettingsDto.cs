using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.Modules.System.Application.Dtos
{
    public class SystemSettingsDto
    {
        public string DefaultLanguage { get; set; } = "tr";
        public string DefaultCurrencyCode { get; set; } = "TRY";
        public string DefaultTimeZone { get; set; } = "Europe/Istanbul";
        public string DateFormat { get; set; } = "dd.MM.yyyy";
        public string TimeFormat { get; set; } = "HH:mm";
        public string NumberFormat { get; set; } = "tr-TR";
        public int DecimalPlaces { get; set; } = 2;
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateSystemSettingsDto
    {
        [Required]
        [MaxLength(10)]
        public string DefaultLanguage { get; set; } = "tr";

        [Required]
        [MaxLength(10)]
        public string DefaultCurrencyCode { get; set; } = "TRY";

        [Required]
        [MaxLength(100)]
        public string DefaultTimeZone { get; set; } = "Europe/Istanbul";

        [Required]
        [MaxLength(20)]
        public string DateFormat { get; set; } = "dd.MM.yyyy";

        [Required]
        [MaxLength(20)]
        public string TimeFormat { get; set; } = "HH:mm";

        [Required]
        [MaxLength(20)]
        public string NumberFormat { get; set; } = "tr-TR";

        [Range(0, 6)]
        public int DecimalPlaces { get; set; } = 2;
    }
}
