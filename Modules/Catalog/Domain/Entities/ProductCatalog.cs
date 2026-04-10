using crm_api.Modules.Catalog.Domain.Enums;

namespace crm_api.Modules.Catalog.Domain.Entities
{
    public class ProductCatalog : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CatalogType CatalogType { get; set; }
        public int? BranchCode { get; set; }
        public int SortOrder { get; set; }

        public ICollection<CatalogCategory> CatalogCategories { get; set; } = new List<CatalogCategory>();
    }
}
