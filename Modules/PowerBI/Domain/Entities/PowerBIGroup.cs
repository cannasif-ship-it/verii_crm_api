namespace crm_api.Modules.PowerBI.Domain.Entities
{
    public class PowerBIGroup : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
