namespace crm_api.Modules.Catalog.Domain.Entities
{
    public class CatalogCategory : BaseEntity
    {
        public long CatalogId { get; set; }
        public ProductCatalog Catalog { get; set; } = null!;

        public long CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;

        public long? ParentCatalogCategoryId { get; set; }
        public CatalogCategory? ParentCatalogCategory { get; set; }

        public int SortOrder { get; set; }
        public bool IsRoot { get; set; }

        public ICollection<CatalogCategory> Children { get; set; } = new List<CatalogCategory>();
    }
}
