using System.ComponentModel.DataAnnotations;

namespace crm_api.Modules.Definitions.Application.Dtos
{
    public class SalesRepCodeUserMatchGetDto : BaseEntityDto
    {
        public long SalesRepCodeId { get; set; }
        public long UserId { get; set; }
        public string SalesRepCode { get; set; } = string.Empty;
        public string? SalesRepName { get; set; }
        public string? UserFullName { get; set; }
        public string? Username { get; set; }
        public string? UserEmail { get; set; }
    }

    public class SalesRepCodeUserMatchCreateDto
    {
        [Required]
        public long SalesRepCodeId { get; set; }

        [Required]
        public long UserId { get; set; }
    }

    public class SalesRepCodeUserMatchUpdateDto
    {
        [Required]
        public long SalesRepCodeId { get; set; }

        [Required]
        public long UserId { get; set; }
    }
}
