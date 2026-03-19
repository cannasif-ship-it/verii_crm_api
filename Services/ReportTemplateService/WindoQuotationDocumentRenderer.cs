using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace crm_api.Services
{
    public static class WindoQuotationDocumentRenderer
    {
        public const string LayoutKey = "windo-quotation-v1";

        private static readonly string[] CompanyContactLines =
        {
            "Kazim Karabekir Mah. 8501 Sokak No:7-B D:18 Buca / Izmir",
            "(0232) 854 70 00",
            "info@windoform.com.tr",
        };

        private static readonly string[] DefaultTerms =
        {
            "Yukaridaki fiyatlara KDV dahil degildir.",
            "Bu teklif olusturulduktan sonra 15 gun gecerlidir.",
            "Fiyatlara fabrika teslimi veya belirtilen teslim sekline gore fiyatlandirma dahildir.",
            "Odemeler siparis onayi ile %30 pesin, kalan teslimatta yapilir.",
            "Belirtilen teslim tarihi siparis onayindan itibaren gecerlidir.",
        };

        private static readonly byte[]? BrandLogoBytes = TryLoadPublicAsset("logo.png");
        private static readonly byte[]? ReferenceImageOne = TryLoadPublicAsset("logo.png");
        private static readonly byte[]? ReferenceImageTwo = TryLoadPublicAsset("login.jpg");
        private static readonly byte[]? ReferenceImageThree = TryLoadPublicAsset("v3rii.jpeg");
        private static readonly WindoLayoutSpec LayoutSpec = LoadLayoutSpec();
        private const string DefaultBrandColor = "#345A99";

        private static class Tokens
        {
            public const float BorderRadius = 6;
            public const float PageMargin = 28;
            public const float TableHeaderHeight = 26;
            public const float SmallText = 8;
            public const float BodyText = 9;
            public const float SectionTitle = 10;
            public const float Title = 18;
            public static readonly string Brand = DefaultBrandColor;
            public static readonly string BrandMuted = "#F6F8FC";
            public static readonly string Border = "#D7DDE8";
            public static readonly string MutedText = "#707C90";
            public static readonly string HeadingText = "#3A4153";
            public static readonly string Surface = "#FFFFFF";
            public static readonly string NoteBackground = "#F8FAFD";
        }

        private static class TableLayout
        {
            public const float FirstPageBudget = 980f;
            public const float ContinuationBudget = 1450f;
            public const float LastPageBudget = 180f;
            public const float DescriptionColumnWidthUnits = 40f;
            public const float TitleFontWidthScale = 1.02f;
            public const float BodyFontWidthScale = 0.94f;
            public const float ProjectFontWidthScale = 0.86f;
            public const float CellPadding = 3f;
            public const float MinRowHeight = 15f;
            public const float LogoBoxSize = 16f;
            public const float TitleLineHeight = 4.4f;
            public const float BodyLineHeight = 3.7f;
            public const float ProjectLineHeight = 3.1f;
            public const float InterLineGap = 0.8f;
            public const float HeaderFontSize = 7.2f;
            public const float BodyFontSize = 7.5f;
            public const float DescriptionTitleFontSize = 8f;
            public const float DescriptionBodyFontSize = 7f;
            public const float ProjectFontSize = 6.2f;
        }

        public static bool CanRender(Models.DocumentRuleType ruleType, DTOs.ReportTemplateData templateData)
        {
            return ruleType == Models.DocumentRuleType.Quotation &&
                string.Equals(templateData.LayoutKey, LayoutKey, StringComparison.OrdinalIgnoreCase);
        }

        public static byte[] GeneratePdf(object entityData)
        {
            return GeneratePdf(entityData, null);
        }

        public static byte[] GeneratePdf(object entityData, DTOs.ReportTemplateData? templateData)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var documentData = BuildData(entityData, templateData);
            var pagination = Paginate(documentData.Lines, TableLayout.FirstPageBudget, TableLayout.ContinuationBudget, TableLayout.LastPageBudget);

            var document = Document.Create(container =>
            {
                for (var pageIndex = 0; pageIndex < pagination.Count; pageIndex++)
                {
                    var chunk = pagination[pageIndex];
                    var isFirstPage = pageIndex == 0;
                    var isLastPage = pageIndex == pagination.Count - 1;

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(Tokens.PageMargin);
                        page.DefaultTextStyle(s => s.FontSize(Tokens.BodyText).FontColor(Tokens.HeadingText));

                        page.Content().Column(column =>
                        {
                            if (isFirstPage)
                                RenderHero(column, documentData);

                            RenderLinesTable(column, chunk.Lines, documentData.CurrencyCode, documentData.Layout);

                            if (isLastPage)
                            {
                                RenderSummary(column, documentData);
                                if (documentData.Layout.ShowTerms)
                                    RenderTerms(column, documentData);
                                if (documentData.Layout.ShowReferenceGallery)
                                    RenderReferenceGallery(column, documentData);
                            }
                        });

                        page.Footer().PaddingTop(8).Row(row =>
                        {
                            row.RelativeItem().Text("WINDOFORM").FontColor(documentData.Layout.AccentColor).SemiBold().FontSize(8);
                            row.ConstantItem(100).AlignRight().Text($"Sayfa {pageIndex + 1}/{pagination.Count}").FontColor(Tokens.MutedText).FontSize(8);
                        });
                    });
                }
            });

            return document.GeneratePdf();
        }

        public static WindoQuotationPaginationDebug DescribePagination(object entityData)
        {
            var documentData = BuildData(entityData);
            return DescribePagination(documentData.Lines);
        }

        public static WindoQuotationPaginationDebug DescribePagination(IReadOnlyList<WindoQuotationLineData> lines)
        {
            var pages = Paginate(lines, TableLayout.FirstPageBudget, TableLayout.ContinuationBudget, TableLayout.LastPageBudget);
            var rows = new List<WindoQuotationPaginationRowDebug>();

            for (var pageIndex = 0; pageIndex < pages.Count; pageIndex++)
            {
                var page = pages[pageIndex];
                for (var rowIndex = 0; rowIndex < page.Lines.Count; rowIndex++)
                {
                    var line = page.Lines[rowIndex];
                    rows.Add(new WindoQuotationPaginationRowDebug
                    {
                        GlobalIndex = rows.Count,
                        PageNumber = pageIndex + 1,
                        PageRowIndex = rowIndex,
                        EstimatedHeight = EstimateRowHeight(line),
                        ProductCode = line.ProductCode,
                        ProductName = line.ProductName,
                        TitleLineCount = WrapTextByWidth(
                            string.IsNullOrWhiteSpace(line.ProductName) ? "-" : line.ProductName,
                            TableLayout.DescriptionColumnWidthUnits,
                            TableLayout.TitleFontWidthScale).Count,
                        DescriptionLineCount = WrapTextByWidth(
                            BuildLineDescription(line),
                            TableLayout.DescriptionColumnWidthUnits,
                            TableLayout.BodyFontWidthScale).Count,
                    });
                }
            }

            return new WindoQuotationPaginationDebug
            {
                FirstPageBudget = TableLayout.FirstPageBudget,
                ContinuationBudget = TableLayout.ContinuationBudget,
                LastPageBudget = TableLayout.LastPageBudget,
                PageCount = pages.Count,
                Pages = pages.Select(page => new WindoQuotationPaginationPageDebug
                {
                    PageNumber = page.PageNumber,
                    RowCount = page.Lines.Count,
                    UsedHeight = page.UsedHeight,
                }).ToList(),
                Rows = rows,
            };
        }

        public static int CountWrappedLines(string? text, int maxCharactersPerLine)
        {
            return WrapTextByWidth(text, maxCharactersPerLine, 1f).Count;
        }

        public static IReadOnlyList<string> WrapTextByWidth(string? text, float maxWidthUnits, float fontWidthScale)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new[] { string.Empty };

            var normalized = text.Replace("\r\n", "\n");
            var paragraphLines = normalized.Split('\n', StringSplitOptions.None);
            var wrapped = new List<string>();

            foreach (var rawLine in paragraphLines)
            {
                var line = rawLine.Trim();
                if (line.Length == 0)
                {
                    wrapped.Add(string.Empty);
                    continue;
                }

                var current = string.Empty;
                var currentWidth = 0f;
                foreach (var word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    var wordWidth = EstimateTextWidth(word, fontWidthScale);
                    var spaceWidth = current.Length == 0 ? 0f : EstimateTextWidth(" ", fontWidthScale);
                    if (current.Length == 0)
                    {
                        current = word;
                        currentWidth = wordWidth;
                        continue;
                    }

                    if (currentWidth + spaceWidth + wordWidth <= maxWidthUnits)
                    {
                        current = $"{current} {word}";
                        currentWidth += spaceWidth + wordWidth;
                        continue;
                    }

                    wrapped.Add(current);
                    current = word;
                    currentWidth = wordWidth;
                }

                wrapped.Add(current);
            }

            return wrapped.Count == 0 ? new[] { string.Empty } : wrapped;
        }

        public static float EstimateRowHeight(WindoQuotationLineData line)
        {
            var titleLines = WrapTextByWidth(
                string.IsNullOrWhiteSpace(line.ProductName) ? "-" : line.ProductName,
                TableLayout.DescriptionColumnWidthUnits,
                TableLayout.TitleFontWidthScale);
            var detailText = BuildLineDescription(line);
            var detailLines = string.IsNullOrWhiteSpace(detailText)
                ? Array.Empty<string>()
                : WrapTextByWidth(detailText, TableLayout.DescriptionColumnWidthUnits, TableLayout.BodyFontWidthScale);
            var projectLines = string.IsNullOrWhiteSpace(line.ProjectCode)
                ? Array.Empty<string>()
                : WrapTextByWidth($"Proje: {line.ProjectCode}", TableLayout.DescriptionColumnWidthUnits, TableLayout.ProjectFontWidthScale);

            var textHeight =
                TableLayout.CellPadding * 2 +
                titleLines.Count * TableLayout.TitleLineHeight +
                Math.Max(0, titleLines.Count - 1) * TableLayout.InterLineGap +
                detailLines.Count * TableLayout.BodyLineHeight +
                Math.Max(0, detailLines.Count) * (detailLines.Count > 0 ? TableLayout.InterLineGap : 0) +
                projectLines.Count * TableLayout.ProjectLineHeight;

            return Math.Max(TableLayout.MinRowHeight, Math.Max(TableLayout.LogoBoxSize + TableLayout.CellPadding * 2, textHeight));
        }

        public static IReadOnlyList<WindoQuotationPagePlan> Paginate(IReadOnlyList<WindoQuotationLineData> lines, float firstPageBudget, float continuationBudget, float lastPageBudget)
        {
            if (lines.Count == 0)
            {
                return new[]
                {
                    new WindoQuotationPagePlan(1, new[] { new WindoQuotationLineData() }, 0f),
                };
            }

            var totalHeight = lines.Sum(EstimateRowHeight);
            if (totalHeight <= firstPageBudget)
                return PaginateSequentially(lines, firstPageBudget, continuationBudget);

            var lastPageLines = new List<WindoQuotationLineData>();
            var lastPageUsed = 0f;

            for (var index = lines.Count - 1; index >= 0; index--)
            {
                var line = lines[index];
                var rowHeight = EstimateRowHeight(line);
                if (lastPageLines.Count > 0 && lastPageUsed + rowHeight > lastPageBudget)
                    break;

                lastPageLines.Insert(0, line);
                lastPageUsed += rowHeight;
            }

            if (lastPageLines.Count == 0)
                lastPageLines.Add(lines[^1]);

            if (lastPageLines.Count >= lines.Count)
                return PaginateSequentially(lines, firstPageBudget, continuationBudget);

            var prefixLines = lines.Take(lines.Count - lastPageLines.Count).ToList();
            var pages = PaginateSequentially(prefixLines, firstPageBudget, continuationBudget).ToList();
            pages.Add(new WindoQuotationPagePlan(pages.Count + 1, lastPageLines, lastPageUsed));
            return pages;
        }

        private static IReadOnlyList<WindoQuotationPagePlan> PaginateSequentially(IReadOnlyList<WindoQuotationLineData> lines, float firstPageBudget, float continuationBudget)
        {
            var pages = new List<WindoQuotationPagePlan>();
            var currentLines = new List<WindoQuotationLineData>();
            var currentBudget = firstPageBudget;
            var currentUsed = 0f;

            foreach (var line in lines)
            {
                var rowHeight = EstimateRowHeight(line);
                if (currentLines.Count > 0 && currentUsed + rowHeight > currentBudget)
                {
                    pages.Add(new WindoQuotationPagePlan(pages.Count + 1, currentLines.ToList(), currentUsed));
                    currentLines.Clear();
                    currentBudget = continuationBudget;
                    currentUsed = 0f;
                }

                currentLines.Add(line);
                currentUsed += rowHeight;
            }

            if (currentLines.Count == 0)
                currentLines.Add(new WindoQuotationLineData());

            pages.Add(new WindoQuotationPagePlan(pages.Count + 1, currentLines.ToList(), currentUsed));
            return pages;
        }

        private static WindoQuotationDocumentData BuildData(object entityData, DTOs.ReportTemplateData? templateData = null)
        {
            var lines = ReadSequence(entityData, "Lines")
                .Select(BuildLine)
                .ToList();

            var notes = Enumerable.Range(1, 15)
                .Select(index => ReadString(entityData, $"Note{index}"))
                .Where(note => !string.IsNullOrWhiteSpace(note))
                .Cast<string>()
                .ToList();

            var layout = ParseLayoutOptions(templateData?.LayoutOptions);

            return new WindoQuotationDocumentData
            {
                OfferNo = ReadString(entityData, "OfferNo") ?? "-",
                OfferDate = ReadDate(entityData, "OfferDate"),
                DeliveryDate = ReadDate(entityData, "DeliveryDate"),
                ValidUntil = ReadDate(entityData, "ValidUntil"),
                CustomerName = NormalizeCustomerName(ReadString(entityData, "CustomerName") ?? ReadString(entityData, "PotentialCustomerName")),
                ErpCustomerCode = ReadString(entityData, "ErpCustomerCode"),
                RepresentativeName = ReadString(entityData, "RepresentativeName"),
                ShippingAddress = ReadString(entityData, "ShippingAddressText"),
                PaymentTypeName = ReadString(entityData, "PaymentTypeName"),
                SalesTypeName = ReadString(entityData, "SalesTypeDefinitionName"),
                ProjectCode = ReadString(entityData, "ErpProjectCode"),
                DocumentSerialTypeName = ReadString(entityData, "DocumentSerialTypeName"),
                Description = ReadString(entityData, "Description"),
                CurrencyCode = NormalizeCurrencyCode(ReadString(entityData, "Currency")),
                GeneralDiscountRate = ReadDecimal(entityData, "GeneralDiscountRate"),
                GeneralDiscountAmount = ReadDecimal(entityData, "GeneralDiscountAmount"),
                NetTotal = ReadDecimal(entityData, "Total"),
                GrandTotal = ReadDecimal(entityData, "GrandTotal"),
                Notes = notes.Count > 0 ? notes : DefaultTerms.ToList(),
                Lines = lines,
                Layout = layout,
            };
        }

        private static WindoQuotationLineData BuildLine(object source)
        {
            return new WindoQuotationLineData
            {
                ProductCode = ReadString(source, "ProductCode"),
                ProductName = ReadString(source, "ProductName"),
                Description = ReadString(source, "Description"),
                Description1 = ReadString(source, "Description1"),
                Description2 = ReadString(source, "Description2"),
                Description3 = ReadString(source, "Description3"),
                Quantity = ReadDecimal(source, "Quantity"),
                UnitPrice = ReadDecimal(source, "UnitPrice"),
                VatRate = ReadDecimal(source, "VatRate"),
                VatAmount = ReadDecimal(source, "VatAmount"),
                LineTotal = ReadDecimal(source, "LineTotal"),
                LineGrandTotal = ReadDecimal(source, "LineGrandTotal"),
                DiscountRate1 = ReadDecimal(source, "DiscountRate1"),
                DiscountRate2 = ReadDecimal(source, "DiscountRate2"),
                DiscountRate3 = ReadDecimal(source, "DiscountRate3"),
                ProjectCode = ReadString(source, "ErpProjectCode"),
            };
        }

        private static void RenderHero(ColumnDescriptor column, WindoQuotationDocumentData data)
        {
            column.Item().Height(4).Background(data.Layout.AccentColor);
            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Element(container => RenderInfoCard(container, card =>
                {
                    card.Column(col =>
                    {
                        if (BrandLogoBytes != null)
                        {
                            col.Item().Height(30).AlignCenter().Image(BrandLogoBytes).FitHeight();
                        }
                        else
                        {
                            col.Item().AlignCenter().Text("WINDOFORM").FontColor(data.Layout.AccentColor).Bold().FontSize(Tokens.Title);
                        }

                        col.Item().PaddingTop(12).Text(data.Layout.TitleText).FontColor(data.Layout.AccentColor).SemiBold().FontSize(10);
                    });
                }));

                row.ConstantItem(220).Element(container => RenderInfoCard(container, card =>
                {
                    card.Column(col =>
                    {
                        col.Item().Text("TEKLIF BILGILERI").FontColor(data.Layout.AccentColor).SemiBold().FontSize(Tokens.SectionTitle);
                        col.Item().PaddingTop(6).Column(infoCol =>
                        {
                            infoCol.Item().Element(c => RenderLabelValue(c, "Teklif No", data.OfferNo));
                            infoCol.Item().Element(c => RenderLabelValue(c, "Tarih", FormatDate(data.OfferDate)));
                            infoCol.Item().Element(c => RenderLabelValue(c, "Teslim", FormatDate(data.DeliveryDate)));
                            infoCol.Item().Element(c => RenderLabelValue(c, "Gecerlilik", FormatDate(data.ValidUntil)));
                        });
                    });
                }));
            });

            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Element(container => RenderInfoCard(container, card =>
                {
                    card.Column(col =>
                    {
                        col.Item().Text("FIRMA BILGILERI").FontColor(data.Layout.AccentColor).SemiBold().FontSize(Tokens.SectionTitle);
                        col.Item().PaddingTop(6).Text("WINDOFORM KAPI & PENCERE AKS.").Bold().FontSize(10);
                        foreach (var line in CompanyContactLines)
                            col.Item().PaddingTop(2).Text(line).FontColor(Tokens.MutedText).FontSize(Tokens.SmallText);
                    });
                }));

                row.RelativeItem().Element(container => RenderInfoCard(container, card =>
                {
                    card.Column(col =>
                    {
                        col.Item().Text("MUSTERI (CARI)").FontColor(data.Layout.AccentColor).SemiBold().FontSize(Tokens.SectionTitle);
                        if (!string.IsNullOrWhiteSpace(data.CustomerName))
                            col.Item().PaddingTop(6).Text(data.CustomerName).Bold().FontSize(10);
                        if (!string.IsNullOrWhiteSpace(data.RepresentativeName))
                            col.Item().PaddingTop(2).Text($"Yetkili: {data.RepresentativeName}").FontColor(Tokens.MutedText).FontSize(Tokens.SmallText);
                        if (!string.IsNullOrWhiteSpace(data.ShippingAddress))
                            col.Item().PaddingTop(2).Text(data.ShippingAddress).FontColor(Tokens.MutedText).FontSize(Tokens.SmallText);
                        if (!string.IsNullOrWhiteSpace(data.ErpCustomerCode))
                            col.Item().PaddingTop(2).Text(data.ErpCustomerCode).FontColor(Tokens.MutedText).FontSize(Tokens.SmallText);
                    });
                }));
            });

            var introNote = !string.IsNullOrWhiteSpace(data.Layout.IntroNote)
                ? data.Layout.IntroNote
                : data.Description;

            if (!string.IsNullOrWhiteSpace(introNote))
            {
                column.Item().PaddingTop(8).Element(container => RenderInfoCard(container, card =>
                {
                    card.Text(introNote).FontColor(Tokens.MutedText).FontSize(Tokens.SmallText);
                }));
            }

            column.Item().PaddingTop(8);
        }

        private static void RenderLinesTable(ColumnDescriptor column, IReadOnlyList<WindoQuotationLineData> lines, string currencyCode, WindoQuotationLayoutOptions layout)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(34);
                    columns.ConstantColumn(72);
                    columns.RelativeColumn(2.6f);
                    columns.ConstantColumn(42);
                    columns.ConstantColumn(70);
                    columns.ConstantColumn(54);
                    columns.ConstantColumn(78);
                });

                table.Header(header =>
                {
                    foreach (var title in new[] { "Gorsel", "Stok Kodu", "Stok Adi / Aciklama", "Miktar", "Birim Fiyat", "Iskonto", "Net Toplam" })
                    {
                        header.Cell().Element(c => HeaderCell(c, title, layout.AccentColor));
                    }
                });

                foreach (var line in lines)
                {
                    table.Cell().Element(c => LogoCell(c, layout.AccentColor));
                    table.Cell().Element(c => BodyCell(c, line.ProductCode, "left"));
                    table.Cell().Element(c => DescriptionCell(c, line, layout.AccentColor));
                    table.Cell().Element(c => BodyCell(c, line.Quantity.ToString("0.##", CultureInfo.InvariantCulture), "center"));
                    table.Cell().Element(c => BodyCell(c, FormatCurrency(line.UnitPrice, currencyCode), "right"));
                    table.Cell().Element(c => BodyCell(c, BuildDiscountSummary(line), "right"));
                    table.Cell().Element(c => BodyCell(c, FormatCurrency(line.LineTotal, currencyCode), "right"));
                }
            });
        }

        private static void RenderSummary(ColumnDescriptor column, WindoQuotationDocumentData data)
        {
            var grossTotal = data.Lines.Sum(line => line.Quantity * line.UnitPrice);
            var discountTotal = data.Lines.Sum(line => (line.DiscountRate1 > 0 ? line.Quantity * line.UnitPrice * line.DiscountRate1 / 100m : 0m) +
                (line.DiscountRate2 > 0 ? line.Quantity * line.UnitPrice * line.DiscountRate2 / 100m : 0m) +
                (line.DiscountRate3 > 0 ? line.Quantity * line.UnitPrice * line.DiscountRate3 / 100m : 0m));

            var lineNetTotal = data.NetTotal > 0 ? data.NetTotal : data.Lines.Sum(line => line.LineTotal);
            var generalDiscount = data.GeneralDiscountAmount ?? 0m;
            var netAfterGeneralDiscount = Math.Max(0m, lineNetTotal - generalDiscount);
            var vatTotal = data.Lines.Sum(line => line.VatAmount);
            var grandTotal = data.GrandTotal > 0 ? data.GrandTotal : data.Lines.Sum(line => line.LineGrandTotal) - generalDiscount;

            column.Item().PaddingTop(LayoutSpec.Summary.TopPadding).Row(row =>
            {
                row.RelativeItem().Element(container => RenderInfoCard(container, card =>
                {
                    card.Column(col =>
                    {
                        col.Item().Text("MUSTERI ONAYI").FontColor(Tokens.MutedText).FontSize(Tokens.SmallText).SemiBold();
                        col.Item().PaddingTop(26).AlignCenter().Text("Kase ve imza").FontColor(Tokens.MutedText).FontSize(Tokens.SmallText);
                    });
                }));

                row.ConstantItem(240).Element(container => RenderInfoCard(container, card =>
                {
                    card.Column(col =>
                    {
                        col.Item().Element(c => RenderLabelValue(c, "Brut Toplam", FormatCurrency(grossTotal, data.CurrencyCode), LayoutSpec.Summary.LabelFontSize, LayoutSpec.Summary.ValueFontSize));
                        col.Item().Element(c => RenderLabelValue(c, "Iskonto Toplam", FormatCurrency(discountTotal + generalDiscount, data.CurrencyCode), LayoutSpec.Summary.LabelFontSize, LayoutSpec.Summary.ValueFontSize));
                        col.Item().Element(c => RenderLabelValue(c, "Net Ara Toplam", FormatCurrency(netAfterGeneralDiscount, data.CurrencyCode), LayoutSpec.Summary.LabelFontSize, LayoutSpec.Summary.ValueFontSize));
                        col.Item().Element(c => RenderLabelValue(c, "KDV Toplam", FormatCurrency(vatTotal, data.CurrencyCode), LayoutSpec.Summary.LabelFontSize, LayoutSpec.Summary.ValueFontSize));
                        col.Item().PaddingTop(6).Row(totalRow =>
                        {
                            totalRow.RelativeItem().Text("Genel Toplam").FontColor(data.Layout.AccentColor).Bold().FontSize(LayoutSpec.Summary.TotalFontSize);
                            totalRow.ConstantItem(96).AlignRight().Text(FormatCurrency(grandTotal, data.CurrencyCode)).FontColor(data.Layout.AccentColor).Bold().FontSize(LayoutSpec.Summary.TotalFontSize);
                        });
                    });
                }));
            });
        }

        private static void RenderTerms(ColumnDescriptor column, WindoQuotationDocumentData data)
        {
            column.Item().PaddingTop(LayoutSpec.Notes.TopPadding).Background(Tokens.NoteBackground).Padding(14).Column(col =>
            {
                col.Item().Text("TEKLIF SARTLARI VE ONEMLI NOTLAR").FontColor(data.Layout.AccentColor).SemiBold().FontSize(LayoutSpec.Notes.TitleFontSize);
                if (!string.IsNullOrWhiteSpace(data.SalesTypeName))
                {
                    col.Item().PaddingTop(8).Border(1).BorderColor(Tokens.Border).Padding(6)
                        .Text($"TESLIM SEKLI: {data.SalesTypeName}")
                        .FontColor(Tokens.HeadingText)
                        .FontSize(LayoutSpec.Notes.BodyFontSize);
                }

                col.Item().PaddingTop(8).Row(row =>
                {
                    var leftNotes = data.Notes.Take((data.Notes.Count + 1) / 2).ToList();
                    var rightNotes = data.Notes.Skip(leftNotes.Count).ToList();

                    row.RelativeItem().Column(left =>
                    {
                        foreach (var note in leftNotes)
                            left.Item().PaddingBottom(4).Text($"• {note}").FontColor(Tokens.MutedText).FontSize(LayoutSpec.Notes.BodyFontSize);
                    });

                    row.RelativeItem().Column(right =>
                    {
                        foreach (var note in rightNotes)
                            right.Item().PaddingBottom(4).Text($"• {note}").FontColor(Tokens.MutedText).FontSize(LayoutSpec.Notes.BodyFontSize);
                    });
                });
            });
        }

        private static void RenderReferenceGallery(ColumnDescriptor column, WindoQuotationDocumentData data)
        {
            column.Item().PaddingTop(12).Column(col =>
            {
                col.Item().Text("SAHA VE KESIF GORSELLERI (REFERANS)").FontColor(data.Layout.AccentColor).SemiBold().FontSize(Tokens.SectionTitle);
                col.Item().PaddingTop(4).Text("Bu gorseller teklifin montaj ve proje sureclerine ait ornek basliklar icin eklenmistir.")
                    .FontColor(Tokens.MutedText)
                    .FontSize(Tokens.SmallText);

                col.Item().PaddingTop(8).Row(row =>
                {
                    row.RelativeItem().Element(c => RenderReferenceImageCard(c, ReferenceImageOne, "Referans 1"));
                    row.RelativeItem().PaddingLeft(8).Element(c => RenderReferenceImageCard(c, ReferenceImageTwo, "Referans 2"));
                    row.RelativeItem().PaddingLeft(8).Element(c => RenderReferenceImageCard(c, ReferenceImageThree, "Referans 3"));
                });
            });
        }

        private static void RenderInfoCard(IContainer container, Action<IContainer> render)
        {
            container.Background(Tokens.Surface)
                .Border(1)
                .BorderColor(Tokens.Border)
                .Padding(12)
                .Element(render);
        }

        private static void RenderMetaBadge(IContainer container, string label, string? value)
        {
            RenderInfoCard(container, badge =>
            {
                badge.Column(col =>
                {
                    col.Item().Text(label).FontColor(Tokens.MutedText).FontSize(7).SemiBold();
                    col.Item().PaddingTop(4).Text(string.IsNullOrWhiteSpace(value) ? "-" : value).FontColor(Tokens.HeadingText).FontSize(Tokens.SmallText);
                });
            });
        }

        private static void RenderLabelValue(IContainer container, string label, string value, float labelFontSize = Tokens.SmallText, float valueFontSize = Tokens.SmallText)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text(label).FontColor(Tokens.MutedText).FontSize(labelFontSize);
                row.ConstantItem(90).AlignRight().Text(value).FontColor(Tokens.HeadingText).FontSize(valueFontSize).SemiBold();
            });
        }

        private static void HeaderCell(IContainer container, string title, string accentColor)
        {
            container.Background(accentColor)
                .Border(1)
                .BorderColor(accentColor)
                .PaddingVertical(4)
                .PaddingHorizontal(3)
                .AlignMiddle()
                .Text(title)
                .FontColor(Colors.White)
                .FontSize(TableLayout.HeaderFontSize)
                .SemiBold();
        }

        private static void LogoCell(IContainer container, string accentColor)
        {
            var cell = container.Border(1).BorderColor(Tokens.Border).Padding(2).AlignCenter().AlignMiddle()
                .Background(Colors.White)
                .Width(TableLayout.LogoBoxSize)
                .Height(TableLayout.LogoBoxSize);

            if (BrandLogoBytes != null)
            {
                cell.Image(BrandLogoBytes).FitArea();
                return;
            }

            cell.Text("WF").FontColor(accentColor).FontSize(8).SemiBold();
        }

        private static void DescriptionCell(IContainer container, WindoQuotationLineData line, string accentColor)
        {
            var description = BuildLineDescription(line);
            container.Border(1).BorderColor(Tokens.Border).Padding(TableLayout.CellPadding).Column(col =>
            {
                col.Item().Text(string.IsNullOrWhiteSpace(line.ProductName) ? "-" : line.ProductName)
                    .FontColor(Tokens.HeadingText).FontSize(TableLayout.DescriptionTitleFontSize).SemiBold();
                if (!string.IsNullOrWhiteSpace(description))
                {
                    col.Item().PaddingTop(1).Text(description).FontColor(Tokens.MutedText).FontSize(TableLayout.DescriptionBodyFontSize);
                }
                if (!string.IsNullOrWhiteSpace(line.ProjectCode))
                {
                    col.Item().PaddingTop(1).Text($"Proje: {line.ProjectCode}").FontColor(accentColor).FontSize(TableLayout.ProjectFontSize);
                }
            });
        }

        private static void BodyCell(IContainer container, string? value, string alignment)
        {
            var aligned = container.Border(1).BorderColor(Tokens.Border).Padding(TableLayout.CellPadding).AlignMiddle();
            aligned = alignment switch
            {
                "center" => aligned.AlignCenter(),
                "right" => aligned.AlignRight(),
                _ => aligned.AlignLeft()
            };

            aligned.Text(string.IsNullOrWhiteSpace(value) ? "-" : value)
                .FontColor(Tokens.HeadingText)
                .FontSize(TableLayout.BodyFontSize);
        }

        private static string BuildLineDescription(WindoQuotationLineData line)
        {
            return string.Join(" • ", new[]
            {
                line.Description,
                line.Description1,
                line.Description2,
                line.Description3,
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }

        private static string BuildDiscountSummary(WindoQuotationLineData line)
        {
            return string.Join(" / ", new[]
            {
                $"%{line.DiscountRate1:0.##}",
                $"%{line.DiscountRate2:0.##}",
                $"%{line.DiscountRate3:0.##}",
            });
        }

        private static string FormatCurrency(decimal value, string currencyCode)
        {
            return $"{value:N2} {currencyCode}";
        }

        private static string FormatDate(DateTime? value)
        {
            return value?.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) ?? "-";
        }

        private static string NormalizeCustomerName(string? value)
        {
            var trimmed = value?.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                return string.Empty;

            var prefixIndex = trimmed.IndexOf(" - ", StringComparison.Ordinal);
            if (trimmed.StartsWith("ERP:", StringComparison.OrdinalIgnoreCase) && prefixIndex >= 0)
                return trimmed[(prefixIndex + 3)..].Trim();

            return trimmed;
        }

        private static string NormalizeCurrencyCode(string? rawCurrency)
        {
            return (rawCurrency ?? string.Empty).Trim().ToUpperInvariant() switch
            {
                "" => "TRY",
                "0" => "TRY",
                "1" => "USD",
                "2" => "EUR",
                "3" => "GBP",
                "TL" => "TRY",
                _ => (rawCurrency ?? "TRY").Trim().ToUpperInvariant()
            };
        }

        private static WindoQuotationLayoutOptions ParseLayoutOptions(Dictionary<string, string>? rawOptions)
        {
            var options = new WindoQuotationLayoutOptions();
            if (rawOptions == null)
                return options;

            if (rawOptions.TryGetValue("accentColor", out var accentColor) && !string.IsNullOrWhiteSpace(accentColor))
                options.AccentColor = NormalizeAccentColor(accentColor);
            if (rawOptions.TryGetValue("titleText", out var titleText) && !string.IsNullOrWhiteSpace(titleText))
                options.TitleText = titleText.Trim();
            if (rawOptions.TryGetValue("introNote", out var introNote) && !string.IsNullOrWhiteSpace(introNote))
                options.IntroNote = introNote.Trim();
            if (rawOptions.TryGetValue("showReferenceGallery", out var showReferenceGallery) && bool.TryParse(showReferenceGallery, out var parsedGallery))
                options.ShowReferenceGallery = parsedGallery;
            if (rawOptions.TryGetValue("showTerms", out var showTerms) && bool.TryParse(showTerms, out var parsedTerms))
                options.ShowTerms = parsedTerms;

            return options;
        }

        private static string NormalizeAccentColor(string rawColor)
        {
            var trimmed = rawColor.Trim();
            if (Regex.IsMatch(trimmed, "^#([0-9a-fA-F]{6})$"))
                return trimmed;

            return DefaultBrandColor;
        }

        private static decimal ReadDecimal(object source, string propertyName)
        {
            var raw = ReadValue(source, propertyName);
            if (raw == null)
                return 0m;

            if (raw is decimal decimalValue)
                return decimalValue;

            if (raw is int intValue)
                return intValue;

            if (raw is long longValue)
                return longValue;

            if (raw is double doubleValue)
                return (decimal)doubleValue;

            if (raw is float floatValue)
                return (decimal)floatValue;

            return decimal.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : 0m;
        }

        private static DateTime? ReadDate(object source, string propertyName)
        {
            var raw = ReadValue(source, propertyName);
            if (raw == null)
                return null;

            if (raw is DateTime value)
                return value;

            return DateTime.TryParse(raw.ToString(), out var parsed) ? parsed : null;
        }

        private static string? ReadString(object source, string propertyName)
        {
            return ReadValue(source, propertyName)?.ToString()?.Trim();
        }

        private static IEnumerable<object> ReadSequence(object source, string propertyName)
        {
            if (ReadValue(source, propertyName) is not System.Collections.IEnumerable sequence || ReadValue(source, propertyName) is string)
                return Enumerable.Empty<object>();

            return sequence.Cast<object>();
        }

        private static object? ReadValue(object source, string propertyName)
        {
            var property = source.GetType().GetProperty(propertyName);
            return property?.GetValue(source);
        }

        private static void RenderReferenceImageCard(IContainer container, byte[]? imageBytes, string label)
        {
            container.Border(1).BorderColor(Tokens.Border).Padding(6).Column(col =>
            {
                col.Item().Height(86).AlignMiddle().AlignCenter().Element(imageContainer =>
                {
                    if (imageBytes != null)
                    {
                        imageContainer.Image(imageBytes).FitArea();
                        return;
                    }

                    imageContainer.Text(label).FontColor(Tokens.MutedText).FontSize(Tokens.SmallText);
                });
                col.Item().PaddingTop(4).AlignCenter().Text(label).FontColor(Tokens.MutedText).FontSize(7);
            });
        }

        private static byte[]? TryLoadPublicAsset(string fileName)
        {
            try
            {
                var current = new DirectoryInfo(AppContext.BaseDirectory);
                while (current != null)
                {
                    var candidate = Path.Combine(current.FullName, "verii_crm_web", "public", fileName);
                    if (File.Exists(candidate))
                        return File.ReadAllBytes(candidate);

                    current = current.Parent;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private static float EstimateTextWidth(string text, float fontWidthScale)
        {
            var width = 0f;
            foreach (var character in text)
            {
                width += character switch
                {
                    'i' or 'l' or 'I' or '|' or '!' or ':' or ';' or '.' or ',' => 0.38f,
                    'f' or 'j' or 't' or 'r' => 0.52f,
                    'm' or 'w' or 'M' or 'W' or '@' or '%' => 1.18f,
                    ' ' => 0.42f,
                    '-' or '_' or '/' or '\\' or '(' or ')' => 0.46f,
                    >= '0' and <= '9' => 0.82f,
                    >= 'A' and <= 'Z' => 0.9f,
                    _ => 0.78f
                };
            }

            return width * fontWidthScale;
        }

        private static WindoLayoutSpec LoadLayoutSpec()
        {
            try
            {
                var current = new DirectoryInfo(AppContext.BaseDirectory);
                while (current != null)
                {
                    var candidate = Path.Combine(current.FullName, "pdf-samples", "windo-quotation-layout-spec.json");
                    if (File.Exists(candidate))
                    {
                        var json = File.ReadAllText(candidate);
                        var spec = JsonSerializer.Deserialize<WindoLayoutSpec>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        });

                        if (spec != null)
                            return spec;
                    }

                    current = current.Parent;
                }
            }
            catch
            {
            }

            return new WindoLayoutSpec
            {
                Pagination = new WindoPaginationSpec
                {
                    FirstPageBudget = 520,
                    ContinuationBudget = 900,
                    RowBaseHeight = 18,
                    RowLineHeight = 6,
                    ProjectLineHeight = 4,
                    DescriptionMaxCharacters = 52,
                },
                Summary = new WindoSummarySpec
                {
                    TopPadding = 8,
                    LabelFontSize = 7.5f,
                    ValueFontSize = 7.5f,
                    TotalFontSize = 11,
                },
                Notes = new WindoNotesSpec
                {
                    TopPadding = 10,
                    BodyFontSize = 7,
                    TitleFontSize = 9,
                },
            };
        }
    }

    public sealed class WindoQuotationDocumentData
    {
        public string OfferNo { get; set; } = "-";
        public DateTime? OfferDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? CustomerName { get; set; }
        public string? ErpCustomerCode { get; set; }
        public string? RepresentativeName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PaymentTypeName { get; set; }
        public string? SalesTypeName { get; set; }
        public string? ProjectCode { get; set; }
        public string? DocumentSerialTypeName { get; set; }
        public string? Description { get; set; }
        public string CurrencyCode { get; set; } = "TRY";
        public decimal? GeneralDiscountRate { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal NetTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public WindoQuotationLayoutOptions Layout { get; set; } = new();
        public List<string> Notes { get; set; } = new();
        public List<WindoQuotationLineData> Lines { get; set; } = new();
    }

    public sealed class WindoQuotationLayoutOptions
    {
        public string AccentColor { get; set; } = "#345A99";
        public string TitleText { get; set; } = "FIYAT TEKLIFI";
        public string? IntroNote { get; set; }
        public bool ShowReferenceGallery { get; set; } = true;
        public bool ShowTerms { get; set; } = true;
    }

    public sealed class WindoQuotationLineData
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description3 { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineGrandTotal { get; set; }
        public decimal DiscountRate1 { get; set; }
        public decimal DiscountRate2 { get; set; }
        public decimal DiscountRate3 { get; set; }
        public string? ProjectCode { get; set; }
    }

    public sealed class WindoQuotationPagePlan
    {
        public WindoQuotationPagePlan(int pageNumber, IReadOnlyList<WindoQuotationLineData> lines, float usedHeight)
        {
            PageNumber = pageNumber;
            Lines = lines;
            UsedHeight = usedHeight;
        }

        public int PageNumber { get; }
        public IReadOnlyList<WindoQuotationLineData> Lines { get; }
        public float UsedHeight { get; }
    }

    public sealed class WindoQuotationPaginationDebug
    {
        public float FirstPageBudget { get; set; }
        public float ContinuationBudget { get; set; }
        public float LastPageBudget { get; set; }
        public int PageCount { get; set; }
        public List<WindoQuotationPaginationPageDebug> Pages { get; set; } = new();
        public List<WindoQuotationPaginationRowDebug> Rows { get; set; } = new();
    }

    public sealed class WindoQuotationPaginationPageDebug
    {
        public int PageNumber { get; set; }
        public int RowCount { get; set; }
        public float UsedHeight { get; set; }
    }

    public sealed class WindoQuotationPaginationRowDebug
    {
        public int GlobalIndex { get; set; }
        public int PageNumber { get; set; }
        public int PageRowIndex { get; set; }
        public float EstimatedHeight { get; set; }
        public int TitleLineCount { get; set; }
        public int DescriptionLineCount { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
    }

    public sealed class WindoLayoutSpec
    {
        public WindoPaginationSpec Pagination { get; set; } = new();
        public WindoSummarySpec Summary { get; set; } = new();
        public WindoNotesSpec Notes { get; set; } = new();
    }

    public sealed class WindoPaginationSpec
    {
        public float FirstPageBudget { get; set; }
        public float ContinuationBudget { get; set; }
        public float RowBaseHeight { get; set; }
        public float RowLineHeight { get; set; }
        public float ProjectLineHeight { get; set; }
        public int DescriptionMaxCharacters { get; set; }
    }

    public sealed class WindoSummarySpec
    {
        public float TopPadding { get; set; }
        public float LabelFontSize { get; set; }
        public float ValueFontSize { get; set; }
        public float TotalFontSize { get; set; }
    }

    public sealed class WindoNotesSpec
    {
        public float TopPadding { get; set; }
        public float BodyFontSize { get; set; }
        public float TitleFontSize { get; set; }
    }
}
