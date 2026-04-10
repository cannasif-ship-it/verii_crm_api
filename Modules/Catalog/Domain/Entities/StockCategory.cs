using crm_api.Modules.Catalog.Domain.Enums;
using StockEntity = crm_api.Modules.Stock.Domain.Entities.Stock;

namespace crm_api.Modules.Catalog.Domain.Entities
{
    public class StockCategory : BaseEntity
    {
        public long StockId { get; set; }
        public StockEntity Stock { get; set; } = null!;

        public long CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;

        public StockCategoryAssignmentType AssignmentType { get; set; }
        public long? RuleId { get; set; }
        public ProductCategoryRule? Rule { get; set; }
        public bool IsPrimary { get; set; } = true;
        public int SortOrder { get; set; }
        public string? Note { get; set; }
    }
}
