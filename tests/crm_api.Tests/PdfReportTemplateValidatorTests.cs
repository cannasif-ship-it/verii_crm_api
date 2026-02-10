using crm_api.DTOs;
using crm_api.Models;
using crm_api.Services;
using Xunit;

namespace crm_api.Tests
{
    public class PdfReportTemplateValidatorTests
    {
        private readonly PdfReportTemplateValidator _validator = new();

        [Fact]
        public void ValidateTemplateData_RequiresData()
        {
            var errors = _validator.ValidateTemplateData(null, DocumentRuleType.Quotation);
            Assert.Single(errors);
            Assert.Contains("required", errors[0], StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ValidateTemplateData_RejectsDuplicateIds()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 100, Height = 100 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "a", Type = "text", X = 0, Y = 0, Width = 10, Height = 10 },
                    new() { Id = "a", Type = "text", X = 20, Y = 20, Width = 10, Height = 10 }
                }
            };
            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);
            Assert.Contains(errors, e => e.Contains("Duplicate"));
        }

        [Fact]
        public void ValidateTemplateData_RejectsInvalidColumnWidths()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 100, Height = 100 },
                Elements = new List<ReportElement>
                {
                    new()
                    {
                        Id = "t1",
                        Type = "table",
                        X = 0,
                        Y = 0,
                        Width = 100,
                        Height = 50,
                        Columns = new List<TableColumn> { new() { Label = "A", Path = "Lines.X" } },
                        ColumnWidths = new List<decimal> { -1 }
                    }
                }
            };
            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);
            Assert.Contains(errors, e => e.Contains("columnWidths"));
        }

        [Fact]
        public void ValidateTemplateData_RejectsInvalidOpacity()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 100, Height = 100 },
                Elements = new List<ReportElement>
                {
                    new()
                    {
                        Id = "e1",
                        Type = "text",
                        X = 0,
                        Y = 0,
                        Width = 10,
                        Height = 10,
                        Style = new ElementStyle { Opacity = 1.5m }
                    }
                }
            };
            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);
            Assert.Contains(errors, e => e.Contains("opacity"));
        }

        [Fact]
        public void GetPlacementWarnings_DetectsOverlap()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 100, Height = 100 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "a", Type = "text", X = 0, Y = 0, Width = 50, Height = 50 },
                    new() { Id = "b", Type = "text", X = 25, Y = 25, Width = 50, Height = 50 }
                }
            };
            var warnings = _validator.GetPlacementWarnings(data);
            Assert.NotEmpty(warnings);
            Assert.Contains(warnings, w => w.Contains("overlap"));
        }

        [Fact]
        public void GetPlacementWarnings_NoOverlap_ReturnsEmpty()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 100, Height = 100 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "a", Type = "text", X = 0, Y = 0, Width = 10, Height = 10 },
                    new() { Id = "b", Type = "text", X = 50, Y = 50, Width = 10, Height = 10 }
                }
            };
            var warnings = _validator.GetPlacementWarnings(data);
            Assert.Empty(warnings);
        }

        [Fact]
        public void ValidateTemplateData_AcceptsValidData()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px" },
                Elements = new List<ReportElement>
                {
                    new() { Id = "e1", Type = "text", X = 10, Y = 20, Width = 100, Height = 24, Text = "Hello" }
                }
            };
            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);
            Assert.Empty(errors);
        }
    }
}
