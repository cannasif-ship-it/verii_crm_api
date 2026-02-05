namespace crm_api.Models.PowerBi
{
    public class PowerBIGroup : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
