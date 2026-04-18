namespace crm_api.Modules.System.Domain.Entities
{
    public class SystemSetting : BaseEntity
    {
        public string NumberFormat { get; set; } = "tr-TR";
        public int DecimalPlaces { get; set; } = 2;
        public bool RestrictCustomersBySalesRepMatch { get; set; }
    }
}
