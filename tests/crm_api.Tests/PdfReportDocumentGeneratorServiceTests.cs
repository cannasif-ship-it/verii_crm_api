using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crm_api.DTOs;
using crm_api.Infrastructure;
using crm_api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace crm_api.Tests
{
    public class PdfReportDocumentGeneratorServiceTests
    {
        [Fact]
        public async Task GeneratePdfForEntityDataAsync_RendersQuotationTotalsBlock()
        {
            var service = new PdfReportDocumentGeneratorService(
                null!,
                NullLogger<PdfReportDocumentGeneratorService>.Instance,
                null!,
                Options.Create(new PdfBuilderOptions()));

            var template = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px", PageCount = 1 },
                Elements = new List<ReportElement>
                {
                    new()
                    {
                        Id = "qt-1",
                        Type = "quotationTotals",
                        Section = "content",
                        X = 460,
                        Y = 820,
                        Width = 260,
                        Height = 220,
                        Text = "Teklif Toplamlari",
                        QuotationTotalsOptions = new QuotationTotalsOptions
                        {
                            Layout = "two-column",
                            CurrencyMode = "code",
                            CurrencyPath = "Currency",
                            ShowGross = true,
                            ShowDiscount = true,
                            ShowVat = true,
                            EmphasizeGrandTotal = true,
                            NoteTitle = "Not",
                            NoteText = "Test aciklamasi",
                            ShowNote = true,
                            HideEmptyNote = true,
                        },
                    },
                },
            };

            var entity = new
            {
                Currency = "USD",
                GeneralDiscountAmount = 120m,
                Total = 880m,
                GrandTotal = 1050m,
                Description = "Test aciklamasi",
            };

            var bytes = await service.GeneratePdfForEntityDataAsync(template, entity);
            var header = Encoding.ASCII.GetString(bytes.Take(4).ToArray());

            Assert.NotEmpty(bytes);
            Assert.Equal("%PDF", header);
        }

        [Fact]
        public async Task GeneratePdfForEntityDataAsync_RendersTableWithDetailLines()
        {
            var service = new PdfReportDocumentGeneratorService(
                null!,
                NullLogger<PdfReportDocumentGeneratorService>.Instance,
                null!,
                Options.Create(new PdfBuilderOptions()));

            var template = new ReportTemplateData
            {
                Page = new PageConfig { Width = 794, Height = 1123, Unit = "px", PageCount = 1 },
                Elements = new List<ReportElement>
                {
                    new()
                    {
                        Id = "tbl-1",
                        Type = "table",
                        Section = "content",
                        X = 24,
                        Y = 100,
                        Width = 746,
                        Height = 260,
                        Columns = new List<TableColumn>
                        {
                            new() { Label = "Stok Kodu", Path = "Lines.ProductCode" },
                            new() { Label = "Aciklama", Path = "Lines.ProductName" },
                            new() { Label = "Toplam", Path = "Lines.LineTotal", Align = "right", Format = "currency" },
                        },
                        TableOptions = new TableOptions
                        {
                            RepeatHeader = true,
                            DetailColumnPath = "Lines.ProductName",
                            DetailPaths = new List<string> { "Description", "Description1", "ErpProjectCode" },
                            DetailLineFontSize = 8,
                            DetailLineColor = "#64748b",
                        },
                        RowStyle = new TableStyle { FontSize = 9, Color = "#334155", BackgroundColor = "#ffffff" },
                    },
                },
            };

            var entity = new
            {
                Lines = new[]
                {
                    new
                    {
                        ProductCode = "STK-001",
                        ProductName = "Windo Sistem",
                        Description = "Ana aciklama",
                        Description1 = "Ek teknik detay",
                        ErpProjectCode = "PRJ-01",
                        LineTotal = 1200m,
                    },
                },
            };

            var bytes = await service.GeneratePdfForEntityDataAsync(template, entity);
            var header = Encoding.ASCII.GetString(bytes.Take(4).ToArray());

            Assert.NotEmpty(bytes);
            Assert.Equal("%PDF", header);
        }
    }
}
