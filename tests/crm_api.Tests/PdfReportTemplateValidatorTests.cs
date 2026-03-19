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
        public void GetPlacementWarnings_DifferentPages_DoesNotWarn()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 100, Height = 100, PageCount = 3 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "cover-1", Type = "image", X = 0, Y = 0, Width = 100, Height = 100, PageNumbers = new List<int> { 1 } },
                    new() { Id = "cover-2", Type = "image", X = 0, Y = 0, Width = 100, Height = 100, PageNumbers = new List<int> { 2 } }
                }
            };

            var warnings = _validator.GetPlacementWarnings(data);

            Assert.Empty(warnings);
        }

        [Fact]
        public void ValidateTemplateData_RejectsPageNumbersOutsideConfiguredPageCount()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px", PageCount = 3 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "e1", Type = "image", X = 10, Y = 20, Width = 100, Height = 100, PageNumbers = new List<int> { 4 } }
                }
            };

            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);

            Assert.Contains(errors, error => error.Contains("pageNumbers must be between 1 and 3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ValidateTemplateData_RejectsDuplicatePageNumbers()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px", PageCount = 3 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "e1", Type = "image", X = 10, Y = 20, Width = 100, Height = 100, PageNumbers = new List<int> { 1, 1 } }
                }
            };

            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);

            Assert.Contains(errors, error => error.Contains("duplicate page number", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ValidateTemplateData_RejectsPageCountAboveLimit()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px", PageCount = 21 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "e1", Type = "text", X = 10, Y = 20, Width = 100, Height = 24, Text = "Hello" }
                }
            };

            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);

            Assert.Contains(errors, error => error.Contains("Page count must be between 1 and 20", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ValidateTemplateData_AcceptsValidData()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px", PageCount = 3 },
                Elements = new List<ReportElement>
                {
                    new() { Id = "cover-1", Type = "image", X = 0, Y = 0, Width = 794, Height = 1123, PageNumbers = new List<int> { 1 } },
                    new() { Id = "cover-2", Type = "image", X = 0, Y = 0, Width = 794, Height = 1123, PageNumbers = new List<int> { 2 } },
                    new() { Id = "title", Type = "field", X = 10, Y = 20, Width = 100, Height = 24, Path = "CustomerName", Value = "Müşteri" },
                    new()
                    {
                        Id = "lines",
                        Type = "table",
                        X = 10,
                        Y = 120,
                        Width = 700,
                        Height = 240,
                        PageNumbers = new List<int> { 3 },
                        Columns = new List<TableColumn>
                        {
                            new() { Label = "Ürün Kodu", Path = "Lines.ProductCode" },
                            new() { Label = "Ürün Adı", Path = "Lines.ProductName" },
                            new() { Label = "Birim Fiyat", Path = "Lines.UnitPrice" }
                        }
                    }
                }
            };
            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateTemplateData_AcceptsQuotationTotalsElement()
        {
            var data = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px", PageCount = 1 },
                Elements = new List<ReportElement>
                {
                    new()
                    {
                        Id = "totals",
                        Type = "quotationTotals",
                        Section = "content",
                        X = 480,
                        Y = 860,
                        Width = 240,
                        Height = 160,
                        Text = "Teklif Toplamlari",
                        QuotationTotalsOptions = new QuotationTotalsOptions
                        {
                            GrossLabel = "Brut Toplam",
                            DiscountLabel = "Iskonto",
                            NetLabel = "Net Toplam",
                            VatLabel = "KDV",
                            GrandLabel = "Genel Toplam",
                            ShowGross = true,
                            ShowDiscount = true,
                            ShowVat = true,
                            EmphasizeGrandTotal = true
                        }
                    }
                }
            };

            var errors = _validator.ValidateTemplateData(data, DocumentRuleType.Quotation);

            Assert.Empty(errors);
        }
    }
}
