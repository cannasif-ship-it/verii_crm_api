using crm_api.Modules.Catalog.Domain.Enums;

namespace crm_api.Modules.Catalog.Domain.Entities
{
    public class ProductCategoryRule : BaseEntity
    {
        public long CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;

        public string RuleName { get; set; } = string.Empty;
        public string? RuleCode { get; set; }
        public StockAttributeType StockAttributeType { get; set; }
        public CategoryRuleOperatorType OperatorType { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Priority { get; set; }

        public ICollection<StockCategory> StockCategories { get; set; } = new List<StockCategory>();
    }
}
