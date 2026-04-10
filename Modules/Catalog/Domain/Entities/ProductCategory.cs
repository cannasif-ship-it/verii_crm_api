using crm_api.Modules.Catalog.Domain.Enums;

namespace crm_api.Modules.Catalog.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public long? ParentCategoryId { get; set; }
        public ProductCategory? ParentCategory { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Level { get; set; } = 1;
        public string? FullPath { get; set; }
        public int SortOrder { get; set; }
        public CategoryVisualPresetType VisualPreset { get; set; } = CategoryVisualPresetType.General;
        public string? ImageUrl { get; set; }
        public string? IconName { get; set; }
        public string? ColorHex { get; set; }
        public bool IsLeaf { get; set; }

        public ICollection<ProductCategory> Children { get; set; } = new List<ProductCategory>();
        public ICollection<CatalogCategory> CatalogCategories { get; set; } = new List<CatalogCategory>();
        public ICollection<StockCategory> StockCategories { get; set; } = new List<StockCategory>();
        public ICollection<ProductCategoryRule> CategoryRules { get; set; } = new List<ProductCategoryRule>();
    }
}
