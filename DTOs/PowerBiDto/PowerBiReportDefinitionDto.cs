using System.ComponentModel.DataAnnotations;
using crm_api.Models;

namespace crm_api.DTOs.PowerBi
{
    public class CreatePowerBIReportDefinitionDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Guid WorkspaceId { get; set; }

        [Required]
        public Guid ReportId { get; set; }

        public Guid? DatasetId { get; set; }

        [MaxLength(500)]
        public string? EmbedUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public string? RlsRoles { get; set; }
        public string? AllowedUserIds { get; set; }
        public string? AllowedRoleIds { get; set; }
    }

    public class UpdatePowerBIReportDefinitionDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Guid WorkspaceId { get; set; }

        [Required]
        public Guid ReportId { get; set; }

        public Guid? DatasetId { get; set; }

        [MaxLength(500)]
        public string? EmbedUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public string? RlsRoles { get; set; }
        public string? AllowedUserIds { get; set; }
        public string? AllowedRoleIds { get; set; }
    }

    public class PowerBIReportDefinitionGetDto : BaseHeaderEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public Guid WorkspaceId { get; set; }
        public Guid ReportId { get; set; }
        public Guid? DatasetId { get; set; }
        public string? EmbedUrl { get; set; }

        public bool IsActive { get; set; }

        public string? RlsRoles { get; set; }
        public string? AllowedUserIds { get; set; }
        public string? AllowedRoleIds { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
