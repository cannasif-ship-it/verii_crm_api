using System.ComponentModel.DataAnnotations;
using crm_api.Models;

namespace crm_api.DTOs.PowerBi
{
    public class PowerBIGroupDto : BaseHeaderEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CreatePowerBIGroupDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdatePowerBIGroupDto
    {
        [Required]
        public long Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class PowerBIGroupGetDto : BaseHeaderEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
