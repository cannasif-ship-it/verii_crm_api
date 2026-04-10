using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace crm_api.Modules.PdfBuilder.Application.Dtos
{
    /// <summary>
    /// Report template DTO for API responses
    /// </summary>
    public class ReportTemplateDto : BaseEntityDto
    {
        public DocumentRuleType RuleType { get; set; }
        public string Title { get; set; } = string.Empty;
        public ReportTemplateData? TemplateData { get; set; }
        public bool IsActive { get; set; }
        /// <summary>Her RuleType için tek bir şablon default olabilir.</summary>
        public bool Default { get; set; }
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
    }

    /// <summary>
    /// Create report template DTO
    /// </summary>
    public class CreateReportTemplateDto
    {
        [Required]
        public DocumentRuleType RuleType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ReportTemplateData TemplateData { get; set; } = new ReportTemplateData();

        public bool IsActive { get; set; } = true;

        /// <summary>true ise bu şablon o RuleType için default olur; aynı tipteki diğerleri false yapılır.</summary>
        public bool Default { get; set; } = false;
    }

    /// <summary>
    /// Update report template DTO
    /// </summary>
    public class UpdateReportTemplateDto
    {
        [Required]
        public DocumentRuleType RuleType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ReportTemplateData TemplateData { get; set; } = new ReportTemplateData();

        public bool IsActive { get; set; } = true;

        /// <summary>true ise bu şablon o RuleType için default olur; aynı tipteki diğerleri false yapılır.</summary>
        public bool Default { get; set; } = false;
    }

    /// <summary>
    /// Report template data structure (matches frontend JSON)
    /// </summary>
    public class ReportTemplateData
    {
        [JsonPropertyName("schemaVersion")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("layoutKey")]
        public string? LayoutKey { get; set; }

        [JsonPropertyName("layoutOptions")]
        public Dictionary<string, string>? LayoutOptions { get; set; }

        [JsonPropertyName("page")]
        public PageConfig Page { get; set; } = new PageConfig();

        [JsonPropertyName("elements")]
        public List<ReportElement> Elements { get; set; } = new List<ReportElement>();
    }

    /// <summary>
    /// Page configuration
    /// </summary>
    public class PageConfig
    {
        [JsonPropertyName("width")]
        public decimal Width { get; set; } = 210;

        [JsonPropertyName("height")]
        public decimal Height { get; set; } = 297;

        [JsonPropertyName("unit")]
        public string Unit { get; set; } = "mm";

        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; } = 1;
    }

    /// <summary>
    /// Report element (text, field, image, table) with absolute placement and styling.
    /// </summary>
    public class ReportElement
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty; // "text" | "field" | "image" | "table" | "shape" | "container" | "note" | "summary" | "quotationTotals"

        [JsonPropertyName("section")]
        public string Section { get; set; } = string.Empty; // "header" | "content" | "footer"

        [JsonPropertyName("x")]
        public decimal X { get; set; }

        [JsonPropertyName("y")]
        public decimal Y { get; set; }

        [JsonPropertyName("width")]
        public decimal Width { get; set; }

        [JsonPropertyName("height")]
        public decimal Height { get; set; }

        [JsonPropertyName("zIndex")]
        public int ZIndex { get; set; }

        [JsonPropertyName("rotation")]
        public decimal Rotation { get; set; }

        [JsonPropertyName("style")]
        public ElementStyle? Style { get; set; }

        [JsonPropertyName("pageNumbers")]
        public List<int>? PageNumbers { get; set; }

        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }

        [JsonPropertyName("binding")]
        public string? Binding { get; set; }

        // Text element properties
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        // Field element properties
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        // Style properties (legacy / inline)
        [JsonPropertyName("fontSize")]
        public decimal? FontSize { get; set; }

        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }

        /// <summary>Text overflow: wrap | ellipsis | clip | autoHeight</summary>
        [JsonPropertyName("textOverflow")]
        public string? TextOverflow { get; set; }

        // Table element properties
        [JsonPropertyName("columns")]
        public List<TableColumn>? Columns { get; set; }

        [JsonPropertyName("headerStyle")]
        public TableStyle? HeaderStyle { get; set; }

        [JsonPropertyName("rowStyle")]
        public TableStyle? RowStyle { get; set; }

        [JsonPropertyName("alternateRowStyle")]
        public TableStyle? AlternateRowStyle { get; set; }

        [JsonPropertyName("columnWidths")]
        public List<decimal>? ColumnWidths { get; set; }

        [JsonPropertyName("tableOptions")]
        public TableOptions? TableOptions { get; set; }

        [JsonPropertyName("summaryItems")]
        public List<SummaryItem>? SummaryItems { get; set; }

        [JsonPropertyName("quotationTotalsOptions")]
        public QuotationTotalsOptions? QuotationTotalsOptions { get; set; }
    }

    public class QuotationTotalsOptions
    {
        [JsonPropertyName("layout")]
        public string? Layout { get; set; }

        [JsonPropertyName("currencyMode")]
        public string? CurrencyMode { get; set; }

        [JsonPropertyName("currencyPath")]
        public string? CurrencyPath { get; set; }

        [JsonPropertyName("grossLabel")]
        public string? GrossLabel { get; set; }

        [JsonPropertyName("discountLabel")]
        public string? DiscountLabel { get; set; }

        [JsonPropertyName("netLabel")]
        public string? NetLabel { get; set; }

        [JsonPropertyName("vatLabel")]
        public string? VatLabel { get; set; }

        [JsonPropertyName("grandLabel")]
        public string? GrandLabel { get; set; }

        [JsonPropertyName("showGross")]
        public bool? ShowGross { get; set; }

        [JsonPropertyName("showDiscount")]
        public bool? ShowDiscount { get; set; }

        [JsonPropertyName("showVat")]
        public bool? ShowVat { get; set; }

        [JsonPropertyName("emphasizeGrandTotal")]
        public bool? EmphasizeGrandTotal { get; set; }

        [JsonPropertyName("noteTitle")]
        public string? NoteTitle { get; set; }

        [JsonPropertyName("notePath")]
        public string? NotePath { get; set; }

        [JsonPropertyName("noteText")]
        public string? NoteText { get; set; }

        [JsonPropertyName("showNote")]
        public bool? ShowNote { get; set; }

        [JsonPropertyName("hideEmptyNote")]
        public bool? HideEmptyNote { get; set; }
    }

    /// <summary>
    /// Inline style for an element (font, color, layout, etc.)
    /// </summary>
    public class ElementStyle
    {
        [JsonPropertyName("fontSize")]
        public decimal? FontSize { get; set; }

        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }

        [JsonPropertyName("opacity")]
        public decimal? Opacity { get; set; }

        [JsonPropertyName("lineHeight")]
        public decimal? LineHeight { get; set; }

        [JsonPropertyName("letterSpacing")]
        public decimal? LetterSpacing { get; set; }

        [JsonPropertyName("textAlign")]
        public string? TextAlign { get; set; }

        [JsonPropertyName("verticalAlign")]
        public string? VerticalAlign { get; set; }

        [JsonPropertyName("background")]
        public string? Background { get; set; }

        [JsonPropertyName("border")]
        public string? Border { get; set; }

        [JsonPropertyName("radius")]
        public decimal? Radius
        {
            get => BorderRadius;
            set => BorderRadius = value;
        }

        [JsonPropertyName("borderRadius")]
        public decimal? BorderRadius { get; set; }

        [JsonPropertyName("imageFit")]
        public string? ImageFit { get; set; }

        [JsonPropertyName("padding")]
        public decimal? Padding { get; set; }
    }

    /// <summary>
    /// Table cell/row style
    /// </summary>
    public class TableStyle
    {
        [JsonPropertyName("fontSize")]
        public decimal? FontSize { get; set; }

        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }

        [JsonPropertyName("backgroundColor")]
        public string? BackgroundColor { get; set; }
    }

    /// <summary>
    /// Table element options (page-break, header repeat).
    /// </summary>
    public class TableOptions
    {
        [JsonPropertyName("repeatHeader")]
        public bool RepeatHeader { get; set; } = true;

        [JsonPropertyName("pageBreak")]
        public string? PageBreak { get; set; } // "auto" | "avoid" | "always"

        [JsonPropertyName("reportRegionMode")]
        public string? ReportRegionMode { get; set; } // "flow"

        [JsonPropertyName("dense")]
        public bool Dense { get; set; }

        [JsonPropertyName("showBorders")]
        public bool ShowBorders { get; set; } = true;

        [JsonPropertyName("presetName")]
        public string? PresetName { get; set; }

        [JsonPropertyName("groupByPath")]
        public string? GroupByPath { get; set; }

        [JsonPropertyName("groupHeaderLabel")]
        public string? GroupHeaderLabel { get; set; }

        [JsonPropertyName("showGroupFooter")]
        public bool ShowGroupFooter { get; set; }

        [JsonPropertyName("groupFooterLabel")]
        public string? GroupFooterLabel { get; set; }

        [JsonPropertyName("groupFooterValuePath")]
        public string? GroupFooterValuePath { get; set; }

        [JsonPropertyName("detailColumnPath")]
        public string? DetailColumnPath { get; set; }

        [JsonPropertyName("detailPaths")]
        public List<string>? DetailPaths { get; set; }

        [JsonPropertyName("detailLineFontSize")]
        public decimal? DetailLineFontSize { get; set; }

        [JsonPropertyName("detailLineColor")]
        public string? DetailLineColor { get; set; }

        [JsonPropertyName("continuationElementIds")]
        public List<string>? ContinuationElementIds { get; set; }

        [JsonPropertyName("flowElementIds")]
        public List<string>? FlowElementIds { get; set; }

        [JsonPropertyName("repeatedElementIds")]
        public List<string>? RepeatedElementIds { get; set; }

        [JsonPropertyName("firstPageBudget")]
        public decimal? FirstPageBudget { get; set; }

        [JsonPropertyName("continuationPageBudget")]
        public decimal? ContinuationPageBudget { get; set; }

        [JsonPropertyName("lastPageBudget")]
        public decimal? LastPageBudget { get; set; }
    }

    /// <summary>
    /// Table column definition
    /// </summary>
    public class TableColumn
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("width")]
        public decimal? Width { get; set; }

        [JsonPropertyName("align")]
        public string? Align { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }
    }

    public class SummaryItem
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("format")]
        public string? Format { get; set; }
    }

    /// <summary>
    /// Generate PDF request DTO
    /// </summary>
    public class GeneratePdfRequest
    {
        [Required]
        public long TemplateId { get; set; }

        [Required]
        public long EntityId { get; set; }
    }

    // ----- PDF-focused DTOs (report-designer / pdf-report-templates API) -----

    /// <summary>
    /// List query for PDF report templates (search, pagination, filters).
    /// </summary>
    public class PdfReportTemplateListRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DocumentRuleType? RuleType { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PdfTablePresetListRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public DocumentRuleType? RuleType { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PdfTemplateAssetDto : BaseEntityDto
    {
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string RelativeUrl { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public long? ReportTemplateId { get; set; }
        public string? ElementId { get; set; }
        public int? PageNumber { get; set; }
        public long? TempQuotattionId { get; set; }
        public long? TempQuotattionLineId { get; set; }
        public string? ProductCode { get; set; }
    }

    public class PdfTablePresetDto : BaseEntityDto
    {
        public DocumentRuleType RuleType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public List<TableColumn> Columns { get; set; } = new();
        public TableOptions? TableOptions { get; set; }
        public bool IsActive { get; set; }
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
    }

    public class CreatePdfTablePresetDto
    {
        [Required]
        public DocumentRuleType RuleType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Key { get; set; } = string.Empty;

        [Required]
        public List<TableColumn> Columns { get; set; } = new();

        public TableOptions? TableOptions { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdatePdfTablePresetDto
    {
        [Required]
        public DocumentRuleType RuleType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Key { get; set; } = string.Empty;

        [Required]
        public List<TableColumn> Columns { get; set; } = new();

        public TableOptions? TableOptions { get; set; }

        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// PDF report template DTO for API responses (PDF builder discipline).
    /// </summary>
    public class PdfReportTemplateDto : BaseEntityDto
    {
        public DocumentRuleType RuleType { get; set; }
        public string Title { get; set; } = string.Empty;
        public ReportTemplateData? TemplateData { get; set; }
        public bool IsActive { get; set; }
        public bool Default { get; set; }
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
    }

    /// <summary>
    /// Create PDF report template DTO.
    /// </summary>
    public class CreatePdfReportTemplateDto
    {
        [Required]
        public DocumentRuleType RuleType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ReportTemplateData TemplateData { get; set; } = new ReportTemplateData();

        public bool IsActive { get; set; } = true;

        public bool Default { get; set; } = false;
    }

    /// <summary>
    /// Update PDF report template DTO.
    /// </summary>
    public class UpdatePdfReportTemplateDto
    {
        [Required]
        public DocumentRuleType RuleType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ReportTemplateData TemplateData { get; set; } = new ReportTemplateData();

        public bool IsActive { get; set; } = true;

        public bool Default { get; set; } = false;
    }
}
