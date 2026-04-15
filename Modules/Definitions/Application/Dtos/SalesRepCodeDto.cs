using System.ComponentModel.DataAnnotations;

namespace crm_api.Modules.Definitions.Application.Dtos
{
    public class SalesRepCodeGetDto : BaseEntityDto
    {
        public short BranchCode { get; set; }
        public string SalesRepCode { get; set; } = string.Empty;
        public string? SalesRepDescription { get; set; }
        public string? Name { get; set; }
    }

    public class SalesRepCodeCreateDto
    {
        public short BranchCode { get; set; }

        [Required]
        [MaxLength(8)]
        public string SalesRepCode { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? SalesRepDescription { get; set; }

        [MaxLength(35)]
        public string? Name { get; set; }
    }

    public class SalesRepCodeUpdateDto
    {
        public short BranchCode { get; set; }

        [Required]
        [MaxLength(8)]
        public string SalesRepCode { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? SalesRepDescription { get; set; }

        [MaxLength(35)]
        public string? Name { get; set; }
    }

    public class SalesRepCodeSyncResponseDto
    {
        public int CreatedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int DeactivatedCount { get; set; }
    }
}
