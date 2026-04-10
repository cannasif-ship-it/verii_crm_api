using crm_api.Modules.Catalog.Domain.Enums;

namespace crm_api.Modules.Catalog.Application.Dtos
{
    public class ProductCatalogGetDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CatalogType CatalogType { get; set; }
        public int? BranchCode { get; set; }
        public int SortOrder { get; set; }
    }

    public class ProductCatalogCreateDto : BaseCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CatalogType CatalogType { get; set; }
        public int? BranchCode { get; set; }
        public int SortOrder { get; set; }
    }

    public class ProductCatalogUpdateDto : BaseUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CatalogType CatalogType { get; set; }
        public int? BranchCode { get; set; }
        public int SortOrder { get; set; }
    }

    public class CatalogCategoryNodeDto
    {
        public long CatalogCategoryId { get; set; }
        public long CategoryId { get; set; }
        public long? ParentCatalogCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Level { get; set; }
        public string? FullPath { get; set; }
        public bool IsLeaf { get; set; }
        public bool HasChildren { get; set; }
        public int SortOrder { get; set; }
        public CategoryVisualPresetType VisualPreset { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconName { get; set; }
        public string? ColorHex { get; set; }
    }

    public class CatalogCategoryCreateDto : BaseCreateDto
    {
        public long? ParentCatalogCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsLeaf { get; set; }
        public CategoryVisualPresetType VisualPreset { get; set; } = CategoryVisualPresetType.General;
        public string? ImageUrl { get; set; }
    }

    public class CatalogCategoryUpdateDto : BaseUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsLeaf { get; set; }
        public CategoryVisualPresetType VisualPreset { get; set; } = CategoryVisualPresetType.General;
        public string? ImageUrl { get; set; }
    }

    public class CatalogCategoryReorderDto : BaseUpdateDto
    {
        public long? ParentCatalogCategoryId { get; set; }
        public List<long> OrderedCatalogCategoryIds { get; set; } = [];
    }

    public class CatalogStockItemDto : BaseEntityDto
    {
        public long StockCategoryId { get; set; }
        public long StockId { get; set; }
        public string ErpStockCode { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public string? GrupKodu { get; set; }
        public string? GrupAdi { get; set; }
        public string? Kod1 { get; set; }
        public string? Kod1Adi { get; set; }
        public string? Kod2 { get; set; }
        public string? Kod2Adi { get; set; }
        public string? Kod3 { get; set; }
        public string? Kod3Adi { get; set; }
        public bool IsPrimaryCategory { get; set; }
    }

    public class StockCategoryCreateDto : BaseCreateDto
    {
        public long StockId { get; set; }
        public bool IsPrimary { get; set; } = true;
        public int SortOrder { get; set; }
        public string? Note { get; set; }
    }

    public class ProductCategoryRuleGetDto : BaseEntityDto
    {
        public long CategoryId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public string? RuleCode { get; set; }
        public StockAttributeType StockAttributeType { get; set; }
        public CategoryRuleOperatorType OperatorType { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Priority { get; set; }
    }

    public class CategoryRuleValueOptionDto
    {
        public string Value { get; set; } = string.Empty;
        public int UsageCount { get; set; }
    }

    public class ProductCategoryRuleCreateDto : BaseCreateDto
    {
        public string RuleName { get; set; } = string.Empty;
        public string? RuleCode { get; set; }
        public StockAttributeType StockAttributeType { get; set; }
        public CategoryRuleOperatorType OperatorType { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Priority { get; set; }
    }

    public class ProductCategoryRuleUpdateDto : BaseUpdateDto
    {
        public string RuleName { get; set; } = string.Empty;
        public string? RuleCode { get; set; }
        public StockAttributeType StockAttributeType { get; set; }
        public CategoryRuleOperatorType OperatorType { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Priority { get; set; }
    }

    public class CategoryRuleApplyResultDto
    {
        public int MatchedStockCount { get; set; }
        public int CreatedAssignmentCount { get; set; }
        public int UpdatedAssignmentCount { get; set; }
        public int SkippedManualAssignmentCount { get; set; }
    }

    public class CategoryRulePreviewItemDto
    {
        public long StockId { get; set; }
        public string ErpStockCode { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public long? ExistingStockCategoryId { get; set; }
        public string MatchedRuleName { get; set; } = string.Empty;
        public string? MatchedRuleCode { get; set; }
        public int Priority { get; set; }
        public CategoryRulePreviewActionType ActionType { get; set; }
    }

    public class CategoryRulePreviewResultDto : CategoryRuleApplyResultDto
    {
        public List<CategoryRulePreviewItemDto> PreviewItems { get; set; } = [];
    }
}
