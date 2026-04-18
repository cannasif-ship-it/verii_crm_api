using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.Modules.System.Application.Dtos
{
    public class SystemSettingsDto
    {
        public string NumberFormat { get; set; } = "tr-TR";
        public int DecimalPlaces { get; set; } = 2;
        public bool RestrictCustomersBySalesRepMatch { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateSystemSettingsDto
    {
        [Required]
        [MaxLength(20)]
        public string NumberFormat { get; set; } = "tr-TR";

        [Range(0, 6)]
        public int DecimalPlaces { get; set; } = 2;

        public bool RestrictCustomersBySalesRepMatch { get; set; }
    }
}
