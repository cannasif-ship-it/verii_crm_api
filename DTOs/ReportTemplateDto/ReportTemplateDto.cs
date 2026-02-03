using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using crm_api.Models;

namespace crm_api.DTOs
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
        public decimal Width { get; set; } = 794;

        [JsonPropertyName("height")]
        public decimal Height { get; set; } = 1123;

        [JsonPropertyName("unit")]
        public string Unit { get; set; } = "px";
    }

    /// <summary>
    /// Report element (text, field, image, table)
    /// </summary>
    public class ReportElement
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty; // "text" | "field" | "image" | "table"

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

        // Text element properties
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        // Field element properties
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        // Style properties
        [JsonPropertyName("fontSize")]
        public decimal? FontSize { get; set; }

        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }

        // Table element properties
        [JsonPropertyName("columns")]
        public List<TableColumn>? Columns { get; set; }
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
}
