using System;

namespace crm_api.Models
{
    public class DemandNotes : BaseEntity
    {
        public long DemandId { get; set; }
        public Demand Demand { get; set; } = null!;

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
