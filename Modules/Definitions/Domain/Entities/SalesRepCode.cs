namespace crm_api.Modules.Definitions.Domain.Entities
{
    public class SalesRepCode : BaseEntity
    {
        public short BranchCode { get; set; }
        public string SalesRepCodeValue { get; set; } = string.Empty;
        public string? SalesRepDescription { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<SalesRepCodeUserMatch> UserMatches { get; set; } = new List<SalesRepCodeUserMatch>();
    }
}
