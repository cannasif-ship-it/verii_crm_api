using System.ComponentModel.DataAnnotations;
using crm_api.Models;
using crm_api.Models.PowerBi;

namespace crm_api.DTOs.PowerBi
{
    // Liste / genel DTO
    public class PowerBIReportDefinitionDto : BaseHeaderEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string WorkspaceId { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;
        public string? EmbedUrl { get; set; }

        public PowerBIContentType ContentType { get; set; }
        public bool IsActive { get; set; }

        public string? DefaultSettingsJson { get; set; }

        // Quotation örneğindeki gibi isim alanları
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CreatePowerBIReportDefinitionDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required, MaxLength(100)]
        public string WorkspaceId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ReportId { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? EmbedUrl { get; set; }

        [Required]
        public PowerBIContentType ContentType { get; set; } = PowerBIContentType.Report;

        public bool IsActive { get; set; } = true;

        public string? DefaultSettingsJson { get; set; }
    }

    public class UpdatePowerBIReportDefinitionDto
    {
        [Required]
        public long Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required, MaxLength(100)]
        public string WorkspaceId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ReportId { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? EmbedUrl { get; set; }

        [Required]
        public PowerBIContentType ContentType { get; set; } = PowerBIContentType.Report;

        public bool IsActive { get; set; } = true;

        public string? DefaultSettingsJson { get; set; }
    }

    // Tekil get için istersen ayrı sınıf (senin QuotationGetDto gibi)
    public class PowerBIReportDefinitionGetDto : BaseHeaderEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string WorkspaceId { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;
        public string? EmbedUrl { get; set; }

        public PowerBIContentType ContentType { get; set; }
        public bool IsActive { get; set; }

        public string? DefaultSettingsJson { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
