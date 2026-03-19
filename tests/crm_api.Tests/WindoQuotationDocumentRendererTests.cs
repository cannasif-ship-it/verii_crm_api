using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using crm_api.DTOs;
using crm_api.Services;
using Xunit;

namespace crm_api.Tests
{
    public class WindoQuotationDocumentRendererTests
    {
        [Fact]
        public void CountWrappedLines_AccountsForExplicitLineBreaks()
        {
            var result = WindoQuotationDocumentRenderer.CountWrappedLines("Ilk satir\nIkinci satir", 40);

            Assert.Equal(2, result);
        }

        [Fact]
        public void WrapTextByWidth_WrapsByEstimatedWidthNotRawLength()
        {
            var wrapped = WindoQuotationDocumentRenderer.WrapTextByWidth(
                "Genis kelimeler ve dar kelimeler birlikte olculmelidir",
                12f,
                1f);

            Assert.True(wrapped.Count > 2);
        }

        [Fact]
        public void EstimateRowHeight_GrowsForLongDescriptions()
        {
            var shortLine = new WindoQuotationLineData
            {
                ProductName = "Standart pencere",
                Description = "Kisa aciklama",
            };

            var longLine = new WindoQuotationLineData
            {
                ProductName = "Standart pencere",
                Description = "Bu satir uzun aciklama davranisini tetiklemek icin olusturuldu ve layout hesaplamasinda daha fazla yer kaplamalidir.",
                Description1 = "Ek teknik detay",
                ProjectCode = "PRJ-01",
            };

            Assert.True(WindoQuotationDocumentRenderer.EstimateRowHeight(longLine) > WindoQuotationDocumentRenderer.EstimateRowHeight(shortLine));
        }

        [Fact]
        public void Paginate_SplitsLongQuotationIntoMultiplePages()
        {
            var lines = Enumerable.Range(1, 18)
                .Select(index => new WindoQuotationLineData
                {
                    ProductCode = $"STK-{index:000}",
                    ProductName = $"Urun {index}",
                    Description = "Cok satirli aciklama testi icin yeterince uzun bir metin blogu.",
                    Quantity = 2,
                    UnitPrice = 1200,
                    LineTotal = 2400,
                })
                .ToList();

            var pages = WindoQuotationDocumentRenderer.Paginate(lines, 120f, 120f, 60f);

            Assert.True(pages.Count > 1);
            Assert.Equal(lines.Count, pages.Sum(page => page.Lines.Count));
        }

        [Fact]
        public void DescribePagination_ReturnsPageAndRowDebugInformation()
        {
            var lines = Enumerable.Range(1, 3)
                .Select(index => new WindoQuotationLineData
                {
                    ProductCode = $"STK-{index:000}",
                    ProductName = $"Urun {index}",
                    Description = "Aciklama satiri",
                    Quantity = 1,
                    UnitPrice = 100,
                    LineTotal = 100,
                })
                .ToList();

            var debug = WindoQuotationDocumentRenderer.DescribePagination(lines);

            Assert.Equal(lines.Count, debug.Rows.Count);
            Assert.True(debug.Pages.Count >= 1);
            Assert.All(debug.Rows, row => Assert.True(row.EstimatedHeight > 0));
        }

        [Fact]
        public void GeneratePdf_ReturnsPdfBytesForWindoQuotation()
        {
            var quotation = new
            {
                OfferNo = "TKL-2026-001",
                OfferDate = new DateTime(2026, 3, 18),
                DeliveryDate = new DateTime(2026, 4, 8),
                ValidUntil = new DateTime(2026, 4, 2),
                CustomerName = "ERP: C001 - Windoform Test Musterisi",
                ErpCustomerCode = "C001",
                RepresentativeName = "Ayse Yilmaz",
                ShippingAddressText = "Izmir / Turkiye",
                PaymentTypeName = "Pesin",
                SalesTypeDefinitionName = "Fabrika Teslim",
                ErpProjectCode = "PRJ-2026-01",
                DocumentSerialTypeName = "A Serisi",
                Description = "Referans teklif aciklamasi",
                Currency = "TRY",
                GeneralDiscountAmount = 250m,
                Total = 8750m,
                GrandTotal = 10250m,
                Note1 = "KDV dahil degildir.",
                Note2 = "Teslim suresi siparis onayindan sonra baslar.",
                Lines = Enumerable.Range(1, 6)
                    .Select(index => new
                    {
                        ProductCode = $"STK-{index:000}",
                        ProductName = $"Windo Sistem {index}",
                        Description = "Ana satir aciklamasi",
                        Description1 = "Ek aciklama",
                        Description2 = index % 2 == 0 ? "Cam ve aksesuar dahil" : string.Empty,
                        Description3 = string.Empty,
                        Quantity = 1 + index,
                        UnitPrice = 1000m + index * 50,
                        DiscountRate1 = 5m,
                        DiscountRate2 = 0m,
                        DiscountRate3 = 0m,
                        VatRate = 20m,
                        VatAmount = 200m + index * 10,
                        LineTotal = 1000m + index * 100,
                        LineGrandTotal = 1200m + index * 110,
                        ErpProjectCode = index % 2 == 0 ? "PRJ-2026-01" : null as string,
                    })
                    .ToList(),
            };

            var bytes = WindoQuotationDocumentRenderer.GeneratePdf(quotation);
            var header = Encoding.ASCII.GetString(bytes.Take(4).ToArray());

            Assert.NotEmpty(bytes);
            Assert.Equal("%PDF", header);
        }

        [Fact]
        public void GeneratePdf_AppliesLayoutOptionsWithoutFailing()
        {
            var quotation = new
            {
                OfferNo = "TKL-2026-002",
                OfferDate = new DateTime(2026, 3, 18),
                DeliveryDate = new DateTime(2026, 4, 8),
                ValidUntil = new DateTime(2026, 4, 2),
                CustomerName = "ERP: C002 - Override Musterisi",
                ErpCustomerCode = "C002",
                RepresentativeName = "Mehmet Demir",
                ShippingAddressText = "Istanbul / Turkiye",
                PaymentTypeName = "Pesin",
                SalesTypeDefinitionName = "Fabrika Teslim",
                Currency = "TRY",
                Description = "Varsayilan aciklama",
                Lines = Enumerable.Range(1, 3)
                    .Select(index => new
                    {
                        ProductCode = $"STK-{index:000}",
                        ProductName = $"Windo Sistem {index}",
                        Description = "Ana satir aciklamasi",
                        Quantity = 2,
                        UnitPrice = 1000m,
                        DiscountRate1 = 5m,
                        DiscountRate2 = 0m,
                        DiscountRate3 = 0m,
                        VatRate = 20m,
                        VatAmount = 200m,
                        LineTotal = 1900m,
                        LineGrandTotal = 2280m,
                    })
                    .ToList(),
            };

            var templateData = new ReportTemplateData
            {
                LayoutKey = WindoQuotationDocumentRenderer.LayoutKey,
                LayoutOptions = new Dictionary<string, string>
                {
                    ["accentColor"] = "#AA5500",
                    ["titleText"] = "OZEL BASLIK",
                    ["introNote"] = "Builder override notu",
                    ["showReferenceGallery"] = "false",
                    ["showTerms"] = "true",
                },
            };

            var bytes = WindoQuotationDocumentRenderer.GeneratePdf(quotation, templateData);

            Assert.NotEmpty(bytes);
            Assert.Equal("%PDF", Encoding.ASCII.GetString(bytes.Take(4).ToArray()));
        }
    }
}
