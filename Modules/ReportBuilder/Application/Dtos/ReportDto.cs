using System.ComponentModel.DataAnnotations;

namespace crm_api.Modules.ReportBuilder.Application.Dtos
{
    public class ReportCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string ConnectionKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string DataSourceType { get; set; } = "view";

        [Required]
        [MaxLength(128)]
        public string DataSourceName { get; set; } = string.Empty;

        [Required]
        public string ConfigJson { get; set; } = string.Empty;

        public List<long> AssignedUserIds { get; set; } = new();
    }

    public class ReportUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string ConnectionKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string DataSourceType { get; set; } = "view";

        [Required]
        [MaxLength(128)]
        public string DataSourceName { get; set; } = string.Empty;

        [Required]
        public string ConfigJson { get; set; } = string.Empty;

        public List<long> AssignedUserIds { get; set; } = new();
    }

    public class ReportListItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ConnectionKey { get; set; } = string.Empty;
        public string DataSourceType { get; set; } = string.Empty;
        public string DataSourceName { get; set; } = string.Empty;
        public DateTime? UpdatedDate { get; set; }
        public bool CanManage { get; set; }
        public string AccessLevel { get; set; } = "owner";
        public int AssignedUserCount { get; set; }
    }

    public class ReportDetailDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ConnectionKey { get; set; } = string.Empty;
        public string DataSourceType { get; set; } = string.Empty;
        public string DataSourceName { get; set; } = string.Empty;
        public string ConfigJson { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool CanManage { get; set; }
        public string AccessLevel { get; set; } = "owner";
        public List<long> AssignedUserIds { get; set; } = new();
    }
}
