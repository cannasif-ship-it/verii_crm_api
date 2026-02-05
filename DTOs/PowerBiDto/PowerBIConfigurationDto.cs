using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.PowerBi
{
    public class PowerBIConfigurationGetDto
    {
        public long Id { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public Guid WorkspaceId { get; set; }
        public string? ApiBaseUrl { get; set; }
        public string? Scope { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreatePowerBIConfigurationDto
    {
        [Required, MaxLength(100)]
        public string TenantId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ClientId { get; set; } = string.Empty;

        [Required]
        public Guid WorkspaceId { get; set; }

        [MaxLength(200)]
        public string? ApiBaseUrl { get; set; }

        [MaxLength(200)]
        public string? Scope { get; set; }
    }

    public class UpdatePowerBIConfigurationDto
    {
        [Required, MaxLength(100)]
        public string TenantId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ClientId { get; set; } = string.Empty;

        [Required]
        public Guid WorkspaceId { get; set; }

        [MaxLength(200)]
        public string? ApiBaseUrl { get; set; }

        [MaxLength(200)]
        public string? Scope { get; set; }
    }
}
