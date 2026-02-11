using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class OrderNotesDto : BaseEntityDto
    {
        public long OrderId { get; set; }
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

    public class CreateOrderNotesDto
    {
        [Required] public long OrderId { get; set; }
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

    public class UpdateOrderNotesDto
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

    public class UpdateOrderNotesListDto
    {
        public List<string>? Notes { get; set; } = new();
    }

    public class OrderNotesGetDto : BaseEntityDto
    {
        public long OrderId { get; set; }
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
