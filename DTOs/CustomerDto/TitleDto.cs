using System;
using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class TitleDto : BaseEntityDto
    {
        public string TitleName { get; set; } = string.Empty;
        public string? Code { get; set; }
        public List<ContactDto>? Contacts { get; set; }
    }

    public class CreateTitleDto
    {
        [Required]
        [MaxLength(100)]
        public string TitleName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? Code { get; set; }
    }

    public class UpdateTitleDto
    {
        [Required]
        [MaxLength(100)]
        public string TitleName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? Code { get; set; }
    }
}
