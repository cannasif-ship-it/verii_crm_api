using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class QuotationNotesDto : BaseEntityDto
    {
        public long QuotationId { get; set; }
        public string? Note1 { get; set; }
        public string? Note2 { get; set; }
        public string? Note3 { get; set; }
        public string? Note4 { get; set; }
        public string? Note5 { get; set; }
        public string? Note6 { get; set; }
        public string? Note7 { get; set; }
        public string? Note8 { get; set; }
        public string? Note9 { get; set; }
        public string? Note10 { get; set; }
        public string? Note11 { get; set; }
        public string? Note12 { get; set; }
        public string? Note13 { get; set; }
        public string? Note14 { get; set; }
        public string? Note15 { get; set; }
    }

    public class CreateQuotationNotesDto
    {
        [Required]
        public long QuotationId { get; set; }

        [MaxLength(100)] public string? Note1 { get; set; }
        [MaxLength(100)] public string? Note2 { get; set; }
        [MaxLength(100)] public string? Note3 { get; set; }
        [MaxLength(100)] public string? Note4 { get; set; }
        [MaxLength(100)] public string? Note5 { get; set; }
        [MaxLength(100)] public string? Note6 { get; set; }
        [MaxLength(100)] public string? Note7 { get; set; }
        [MaxLength(100)] public string? Note8 { get; set; }
        [MaxLength(100)] public string? Note9 { get; set; }
        [MaxLength(100)] public string? Note10 { get; set; }
        [MaxLength(100)] public string? Note11 { get; set; }
        [MaxLength(100)] public string? Note12 { get; set; }
        [MaxLength(100)] public string? Note13 { get; set; }
        [MaxLength(100)] public string? Note14 { get; set; }
        [MaxLength(100)] public string? Note15 { get; set; }
    }

    public class UpdateQuotationNotesDto
    {
        [MaxLength(100)] public string? Note1 { get; set; }
        [MaxLength(100)] public string? Note2 { get; set; }
        [MaxLength(100)] public string? Note3 { get; set; }
        [MaxLength(100)] public string? Note4 { get; set; }
        [MaxLength(100)] public string? Note5 { get; set; }
        [MaxLength(100)] public string? Note6 { get; set; }
        [MaxLength(100)] public string? Note7 { get; set; }
        [MaxLength(100)] public string? Note8 { get; set; }
        [MaxLength(100)] public string? Note9 { get; set; }
        [MaxLength(100)] public string? Note10 { get; set; }
        [MaxLength(100)] public string? Note11 { get; set; }
        [MaxLength(100)] public string? Note12 { get; set; }
        [MaxLength(100)] public string? Note13 { get; set; }
        [MaxLength(100)] public string? Note14 { get; set; }
        [MaxLength(100)] public string? Note15 { get; set; }
    }

    public class UpdateQuotationNotesListDto
    {
        public List<string>? Notes { get; set; } = new();
    }

    public class QuotationNotesGetDto : BaseEntityDto
    {
        public long QuotationId { get; set; }
        public string? Note1 { get; set; }
        public string? Note2 { get; set; }
        public string? Note3 { get; set; }
        public string? Note4 { get; set; }
        public string? Note5 { get; set; }
        public string? Note6 { get; set; }
        public string? Note7 { get; set; }
        public string? Note8 { get; set; }
        public string? Note9 { get; set; }
        public string? Note10 { get; set; }
        public string? Note11 { get; set; }
        public string? Note12 { get; set; }
        public string? Note13 { get; set; }
        public string? Note14 { get; set; }
        public string? Note15 { get; set; }
    }
}
