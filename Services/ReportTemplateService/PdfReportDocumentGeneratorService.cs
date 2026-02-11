using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Infrastructure;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;

namespace crm_api.Services
{
    /// <summary>
    /// PDF report document generator: true absolute layout, SSRF-safe images, structured logging.
    /// </summary>
    public class PdfReportDocumentGeneratorService : IPdfReportDocumentGeneratorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PdfReportDocumentGeneratorService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PdfBuilderOptions _options;

        public PdfReportDocumentGeneratorService(
            IUnitOfWork unitOfWork,
            ILogger<PdfReportDocumentGeneratorService> logger,
            IHttpClientFactory httpClientFactory,
            IOptions<PdfBuilderOptions> options)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options?.Value ?? new PdfBuilderOptions();
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GeneratePdfAsync(DocumentRuleType ruleType, long entityId, ReportTemplateData templateData)
        {
            if (templateData == null)
                throw new ArgumentNullException(nameof(templateData));

            var sw = Stopwatch.StartNew();
            var elementCount = templateData.Elements?.Count ?? 0;
            var warningCount = 0;

            try
            {
                var entityData = await FetchEntityDataAsync(ruleType, entityId);
                if (entityData == null)
                    throw new InvalidOperationException($"Entity with ID {entityId} not found for rule type {ruleType}");

                // Pre-fetch all images async (avoids sync-over-async in render)
                var imageCache = await PreFetchImagesAsync(templateData.Elements ?? new List<ReportElement>(), (reason) =>
                {
                    _logger.LogWarning("PdfReportDocumentGenerator SSRF reject: {Reason}", reason);
                    warningCount++;
                });

                var page = templateData.Page ?? new PageConfig();
                var unit = page.Unit ?? "px";
                var pageWidthPt = PdfUnitConversion.ToPointsFloat(page.Width, unit);
                var pageHeightPt = PdfUnitConversion.ToPointsFloat(page.Height, unit);

                var orderedElements = (templateData.Elements ?? new List<ReportElement>())
                    .OrderBy(e => e.ZIndex)
                    .ThenBy(e => e.Y)
                    .ThenBy(e => e.X)
                    .ToList();

                var document = Document.Create(container =>
                {
                    container.Page(p =>
                    {
                        p.Size(new PageSize(pageWidthPt, pageHeightPt));
                        p.Margin(0);

                        p.Content().Layers(layers =>
                        {
                            layers.PrimaryLayer().Width(pageWidthPt).Height(pageHeightPt).Background(Colors.Transparent);
                            foreach (var element in orderedElements)
                            {
                                var xPt = PdfUnitConversion.ToPointsFloat(element.X, unit);
                                var yPt = PdfUnitConversion.ToPointsFloat(element.Y, unit);
                                var wPt = PdfUnitConversion.ToPointsFloat(element.Width > 0 ? element.Width : 200, unit);
                                var hPt = PdfUnitConversion.ToPointsFloat(element.Height > 0 ? element.Height : 50, unit);

                                layers.Layer()
                                    .TranslateX(xPt)
                                    .TranslateY(yPt)
                                    .Width(wPt)
                                    .Height(hPt)
                                    .Element(c => WrapElementWithStyle(c, element, inner =>
                                    {
                                        RenderElement(inner, element, entityData, unit, imageCache, () => warningCount++);
                                    }));
                            }
                        });
                    });
                });

                var pdfBytes = document.GeneratePdf();
                sw.Stop();

                _logger.LogInformation(
                    "PdfReportDocumentGenerator completed: RuleType={RuleType}, EntityId={EntityId}, ElementCount={ElementCount}, DurationMs={DurationMs}, WarningCount={WarningCount}",
                    ruleType, entityId, elementCount, sw.ElapsedMilliseconds, warningCount);

                return pdfBytes;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex,
                    "PdfReportDocumentGenerator failed: RuleType={RuleType}, EntityId={EntityId}, ElementCount={ElementCount}, DurationMs={DurationMs}",
                    ruleType, entityId, elementCount, sw.ElapsedMilliseconds);
                throw;
            }
        }

        private void WrapElementWithStyle(IContainer container, ReportElement element, Action<IContainer> renderContent)
        {
            var style = element.Style;
            var rotation = ClampRotation(element.Rotation);
            var padding = PdfUnitConversion.ToPointsFloat(style?.Padding ?? 0, "px");
            var bg = style?.Background;
            var border = style?.Border;

            var c = container;
            if (rotation != 0)
                c = c.Rotate(rotation);
            if (padding > 0)
                c = c.Padding(padding);
            if (!string.IsNullOrEmpty(bg))
            {
                try { c = c.Background(bg); } catch { }
            }
            if (!string.IsNullOrEmpty(border))
            {
                try { c = c.Border(1).BorderColor(border); } catch { }
            }

            c.Element(inner => renderContent(inner));
        }

        private static float ClampRotation(decimal v)
        {
            var x = (float)v;
            while (x > 360) x -= 360;
            while (x < -360) x += 360;
            return x;
        }

        private async Task<Dictionary<string, byte[]>> PreFetchImagesAsync(
            List<ReportElement> elements,
            Action<string> onReject)
        {
            var cache = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
            var imageElements = elements.Where(e => "image".Equals(e.Type, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(e.Value)).ToList();

            foreach (var el in imageElements)
            {
                var key = el.Value.Trim();
                if (cache.ContainsKey(key)) continue;

                if (key.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    if (!PdfImageUrlValidator.IsDataUri(key, _options.AllowedImageContentTypes, out var reason))
                    {
                        _logger.LogWarning("PdfReportDocumentGenerator SSRF/data reject: Reason={Reason}, SourceLength={Len}",
                            reason, key.Length);
                        onReject(reason ?? "Invalid data URI");
                        continue;
                    }
                    try
                    {
                        var base64 = key.IndexOf(',') >= 0 ? key.Split(',')[1] : key;
                        var bytes = Convert.FromBase64String(base64);
                        if (bytes.Length > _options.MaxImageSizeBytes)
                        {
                            _logger.LogWarning("PdfReportDocumentGenerator image size exceeded: MaxBytes={Max}, Actual={Actual}",
                                _options.MaxImageSizeBytes, bytes.Length);
                            onReject("Image exceeds max size");
                            continue;
                        }
                        if (!ValidateImageContentType(bytes, out var ctReject))
                        {
                            _logger.LogWarning("PdfReportDocumentGenerator Content-Type reject: {Reason}", ctReject);
                            onReject(ctReject ?? "Invalid content type");
                            continue;
                        }
                        cache[key] = bytes;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "PdfReportDocumentGenerator data URI decode failed");
                        onReject("Failed to decode data URI");
                    }
                    continue;
                }

                if (!PdfImageUrlValidator.IsUrlAllowed(key, _options.AllowlistedImageHosts, out var urlReason))
                {
                    _logger.LogWarning("PdfReportDocumentGenerator SSRF URL reject: Reason={Reason}, Url={Url}",
                        urlReason, key);
                    onReject(urlReason ?? "URL not allowed");
                    continue;
                }

                try
                {
                    using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(_options.ImageFetchTimeoutSeconds));
                    using var httpClient = _httpClientFactory.CreateClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(_options.ImageFetchTimeoutSeconds);
                    var bytes = await httpClient.GetByteArrayAsync(key, cts.Token);
                    if (bytes != null && bytes.Length > _options.MaxImageSizeBytes)
                    {
                        _logger.LogWarning("PdfReportDocumentGenerator URL image size exceeded: MaxBytes={Max}, Actual={Actual}, Url={Url}",
                            _options.MaxImageSizeBytes, bytes?.Length ?? 0, key);
                        onReject("Image exceeds max size");
                        continue;
                    }
                    if (bytes != null && bytes.Length > 0 && !ValidateImageContentType(bytes, out var ctReject2))
                    {
                        _logger.LogWarning("PdfReportDocumentGenerator URL Content-Type reject: {Reason}, Url={Url}", ctReject2, key);
                        onReject(ctReject2 ?? "Invalid content type");
                        continue;
                    }
                    if (bytes != null && bytes.Length > 0)
                        cache[key] = bytes;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "PdfReportDocumentGenerator URL fetch failed: Url={Url}", key);
                    onReject("Failed to fetch image");
                }
            }

            return cache;
        }

        private bool ValidateImageContentType(byte[] bytes, out string? rejectReason)
        {
            rejectReason = null;
            if (bytes == null || bytes.Length < 12) return true;
            var allowed = _options.AllowedImageContentTypes;
            if (allowed == null || allowed.Count == 0) return true;
            var isPng = bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E;
            var isJpeg = bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xD8;
            var isGif = bytes.Length >= 6 && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46;
            var isWebp = bytes.Length >= 12 && bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50;
            if (isPng || isJpeg || isGif || isWebp) return true;
            rejectReason = "Content is not image/png, image/jpeg, image/gif, or image/webp";
            return false;
        }

        private void RenderElement(IContainer container, ReportElement element, object entityData, string unit,
            Dictionary<string, byte[]> imageCache, Action onWarning)
        {
            switch (element.Type?.ToLower())
            {
                case "text":
                    RenderText(container, element);
                    break;
                case "field":
                    RenderField(container, element, entityData);
                    break;
                case "image":
                    RenderImage(container, element, imageCache, onWarning);
                    break;
                case "table":
                    RenderTable(container, element, entityData, unit);
                    break;
                default:
                    onWarning();
                    break;
            }
        }

        private void ApplyTextStyle(IContainer container, ReportElement element, string content)
        {
            var style = element.Style;
            var fontSize = (float)(style?.FontSize ?? element.FontSize ?? 12);
            var color = style?.Color ?? element.Color;
            var fontFamily = style?.FontFamily ?? element.FontFamily;
            var lineHeight = (float)(style?.LineHeight ?? 1.2m);
            var letterSpacing = (float)(style?.LetterSpacing ?? 0);
            var textAlign = (style?.TextAlign ?? "left").ToLowerInvariant();
            var verticalAlign = (style?.VerticalAlign ?? "top").ToLowerInvariant();
            var block = container.DefaultTextStyle(s =>
            {
                var x = s.FontSize(fontSize);
                if (!string.IsNullOrEmpty(color)) x = x.FontColor(color);
                if (!string.IsNullOrEmpty(fontFamily)) x = x.FontFamily(fontFamily);
                if (lineHeight != 1.2f) x = x.LineHeight(lineHeight);
                if (letterSpacing != 0) x = x.LetterSpacing(letterSpacing);
                return x;
            });

            if (textAlign == "center") block = block.AlignCenter();
            else if (textAlign == "right") block = block.AlignRight();
            else block = block.AlignLeft();

            if (verticalAlign == "middle" || verticalAlign == "center") block = block.AlignMiddle();
            else if (verticalAlign == "bottom") block = block.AlignBottom();
            else block = block.AlignTop();

            block.Text(content);
        }

        private void RenderText(IContainer container, ReportElement element)
        {
            if (string.IsNullOrEmpty(element.Text)) return;
            ApplyTextStyle(container, element, element.Text);
        }

        private void RenderField(IContainer container, ReportElement element, object entityData)
        {
            if (string.IsNullOrEmpty(element.Path)) return;
            var value = ResolvePropertyPath(entityData, element.Path);
            var displayValue = value?.ToString() ?? string.Empty;
            if (displayValue.IndexOf('<') >= 0)
                displayValue = StripHtml(displayValue);
            ApplyTextStyle(container, element, displayValue);
        }

        private static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return html;
            var stripped = Regex.Replace(html, @"<[^>]+>", " ");
            return Regex.Replace(stripped, @"\s+", " ").Trim();
        }

        private void RenderImage(IContainer container, ReportElement element,
            Dictionary<string, byte[]> imageCache, Action onWarning)
        {
            if (string.IsNullOrEmpty(element.Value)) return;
            var key = element.Value.Trim();
            if (!imageCache.TryGetValue(key, out var imageBytes) || imageBytes == null || imageBytes.Length == 0)
            {
                onWarning();
                return;
            }
            container.Image(imageBytes).FitArea();
        }

        private int GetTableRowCount(ReportElement element, object entityData)
        {
            if (element.Columns == null || !element.Columns.Any()) return 0;
            var firstPath = element.Columns[0].Path;
            var collectionName = firstPath.Contains('.') ? firstPath.Split('.')[0] : firstPath;
            var collection = ResolvePropertyPath(entityData, collectionName) as IEnumerable<object>;
            return collection?.Count() ?? 0;
        }

        private void RenderTable(IContainer container, ReportElement element, object entityData, string unit)
        {
            if (element.Columns == null || !element.Columns.Any()) return;

            var firstPath = element.Columns[0].Path;
            var collectionName = firstPath.Contains('.') ? firstPath.Split('.')[0] : firstPath;
            var collection = ResolvePropertyPath(entityData, collectionName) as IEnumerable<object>;
            if (collection == null) return;

            var rows = collection.ToList();
            var headerStyle = element.HeaderStyle;
            var rowStyle = element.RowStyle;
            var altStyle = element.AlternateRowStyle;
            var repeatHeader = element.TableOptions?.RepeatHeader ?? true;

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    if (element.ColumnWidths != null && element.ColumnWidths.Count == element.Columns.Count)
                    {
                        foreach (var w in element.ColumnWidths)
                            columns.ConstantColumn(PdfUnitConversion.ToPointsFloat(w, unit));
                    }
                    else
                    {
                        foreach (var _ in element.Columns)
                            columns.RelativeColumn();
                    }
                });

                table.Header(header =>
                {
                    foreach (var col in element.Columns)
                    {
                        var cell = header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text(col.Label).Bold();
                        if (headerStyle?.FontSize.HasValue == true)
                            cell = cell.FontSize((float)headerStyle.FontSize.Value);
                        if (!string.IsNullOrEmpty(headerStyle?.Color))
                            cell = cell.FontColor(headerStyle.Color);
                    }
                });

                var rowIndex = 0;
                foreach (var row in rows)
                {
                    foreach (var col in element.Columns)
                    {
                        var propertyPath = col.Path.Contains('.') ? col.Path.Split('.', 2)[1] : col.Path;
                        var cellValue = ResolvePropertyPath(row, propertyPath)?.ToString() ?? string.Empty;
                        if (cellValue.IndexOf('<') >= 0) cellValue = StripHtml(cellValue);

                        var style = (rowIndex % 2 == 1 && altStyle != null) ? altStyle : rowStyle;
                        var textBlock = !string.IsNullOrEmpty(style?.BackgroundColor)
                            ? table.Cell().Background(style.BackgroundColor).Border(1).Padding(5).Text(cellValue)
                            : table.Cell().Border(1).Padding(5).Text(cellValue);
                        if (style?.FontSize.HasValue == true) textBlock = textBlock.FontSize((float)style.FontSize.Value);
                        if (!string.IsNullOrEmpty(style?.Color)) textBlock = textBlock.FontColor(style.Color);
                    }
                    rowIndex++;
                }
            });
        }

        private object? ResolvePropertyPath(object obj, string path)
        {
            if (obj == null || string.IsNullOrEmpty(path)) return null;
            var parts = path.Split('.');
            var current = obj;
            foreach (var part in parts)
            {
                if (current == null) return null;
                var type = current.GetType();
                var property = type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null) return null;
                current = property.GetValue(current);
            }
            return current;
        }

        private async Task<object?> FetchEntityDataAsync(DocumentRuleType ruleType, long entityId)
        {
            return ruleType switch
            {
                DocumentRuleType.Demand => await (from d in _unitOfWork.Demands.Query(false, false)
                    where d.Id == entityId && !d.IsDeleted
                    select new
                    {
                        d.Id,
                        d.OfferNo,
                        d.OfferDate,
                        d.OfferType,
                        d.RevisionNo,
                        PotentialCustomerName = d.PotentialCustomer != null ? d.PotentialCustomer.CustomerName : "",
                        d.ErpCustomerCode,
                        d.DeliveryDate,
                        ShippingAddressText = d.ShippingAddress != null ? d.ShippingAddress.Address : "",
                        RepresentativeName = d.Representative != null ? d.Representative.FullName : "",
                        d.Description,
                        PaymentTypeName = d.PaymentType != null ? d.PaymentType.Name : "",
                        DocumentSerialTypeName = d.DocumentSerialType != null ? d.DocumentSerialType.SerialPrefix : "",
                        d.Currency,
                        d.CreatedBy,
                        d.UpdatedBy,
                        ExchangeRates = (from er in _unitOfWork.DemandExchangeRates.Query(false, false)
                                where er.DemandId == d.Id && !er.IsDeleted
                                select new { er.Currency, er.ExchangeRate, er.ExchangeRateDate, er.IsOfficial }).ToList(),
                        Lines = (from dl in _unitOfWork.DemandLines.Query(false, false)
                                where dl.DemandId == d.Id && !dl.IsDeleted
                                select new
                                {
                                    dl.ProductCode,
                                    ProductName = dl.ProductCode,
                                    GroupCode = "",
                                    dl.Quantity,
                                    dl.UnitPrice,
                                    dl.DiscountRate1,
                                    dl.DiscountAmount1,
                                    dl.DiscountRate2,
                                    dl.DiscountAmount2,
                                    dl.DiscountRate3,
                                    dl.DiscountAmount3,
                                    dl.VatRate,
                                    dl.VatAmount,
                                    dl.LineTotal,
                                    dl.LineGrandTotal,
                                    dl.Description,
                                    HtmlDescription = dl.RelatedStockId.HasValue
                                        ? (_unitOfWork.StockDetails.Query(false, false).Where(sd => sd.StockId == dl.RelatedStockId && !sd.IsDeleted).Select(sd => sd.HtmlDescription).FirstOrDefault() ?? "")
                                        : "",
                                    DefaultImagePath = dl.RelatedStockId.HasValue
                                        ? (_unitOfWork.Repository<StockImage>().Query(false, false).Where(si => si.StockId == dl.RelatedStockId && !si.IsDeleted).OrderByDescending(si => si.IsPrimary).ThenBy(si => si.SortOrder).Select(si => si.FilePath).FirstOrDefault() ?? "")
                                        : ""
                                }).ToList()
                    }).FirstOrDefaultAsync(),

                DocumentRuleType.Quotation => await (from q in _unitOfWork.Quotations.Query(false, false)
                    where q.Id == entityId && !q.IsDeleted
                    select new
                    {
                        q.Id,
                        q.OfferNo,
                        q.OfferDate,
                        q.OfferType,
                        q.RevisionNo,
                        PotentialCustomerName = q.PotentialCustomer != null ? q.PotentialCustomer.CustomerName : "",
                        q.ErpCustomerCode,
                        q.DeliveryDate,
                        ShippingAddressText = q.ShippingAddress != null ? q.ShippingAddress.Address : "",
                        RepresentativeName = q.Representative != null ? q.Representative.FullName : "",
                        q.Description,
                        PaymentTypeName = q.PaymentType != null ? q.PaymentType.Name : "",
                        DocumentSerialTypeName = q.DocumentSerialType != null ? q.DocumentSerialType.SerialPrefix : "",
                        q.Currency,
                        q.CreatedBy,
                        q.UpdatedBy,
                        ExchangeRates = (from er in _unitOfWork.QuotationExchangeRates.Query(false, false)
                                where er.QuotationId == q.Id && !er.IsDeleted
                                select new { er.Currency, er.ExchangeRate, er.ExchangeRateDate, er.IsOfficial }).ToList(),
                        Lines = (from ql in _unitOfWork.QuotationLines.Query(false, false)
                                where ql.QuotationId == q.Id && !ql.IsDeleted
                                select new
                                {
                                    ql.ProductCode,
                                    ProductName = ql.ProductCode,
                                    GroupCode = "",
                                    ql.Quantity,
                                    ql.UnitPrice,
                                    ql.DiscountRate1,
                                    ql.DiscountAmount1,
                                    ql.DiscountRate2,
                                    ql.DiscountAmount2,
                                    ql.DiscountRate3,
                                    ql.DiscountAmount3,
                                    ql.VatRate,
                                    ql.VatAmount,
                                    ql.LineTotal,
                                    ql.LineGrandTotal,
                                    ql.Description,
                                    HtmlDescription = ql.RelatedStockId.HasValue
                                        ? (_unitOfWork.StockDetails.Query(false, false).Where(sd => sd.StockId == ql.RelatedStockId && !sd.IsDeleted).Select(sd => sd.HtmlDescription).FirstOrDefault() ?? "")
                                        : "",
                                    DefaultImagePath = ql.RelatedStockId.HasValue
                                        ? (_unitOfWork.Repository<StockImage>().Query(false, false).Where(si => si.StockId == ql.RelatedStockId && !si.IsDeleted).OrderByDescending(si => si.IsPrimary).ThenBy(si => si.SortOrder).Select(si => si.FilePath).FirstOrDefault() ?? "")
                                        : ""
                                }).ToList()
                    }).FirstOrDefaultAsync(),

                DocumentRuleType.Order => await (from o in _unitOfWork.Orders.Query(false, false)
                    where o.Id == entityId && !o.IsDeleted
                    select new
                    {
                        o.Id,
                        o.OfferNo,
                        o.OfferDate,
                        o.OfferType,
                        o.RevisionNo,
                        PotentialCustomerName = o.PotentialCustomer != null ? o.PotentialCustomer.CustomerName : "",
                        o.ErpCustomerCode,
                        o.DeliveryDate,
                        ShippingAddressText = o.ShippingAddress != null ? o.ShippingAddress.Address : "",
                        RepresentativeName = o.Representative != null ? o.Representative.FullName : "",
                        o.Description,
                        PaymentTypeName = o.PaymentType != null ? o.PaymentType.Name : "",
                        DocumentSerialTypeName = o.DocumentSerialType != null ? o.DocumentSerialType.SerialPrefix : "",
                        o.Currency,
                        o.CreatedBy,
                        o.UpdatedBy,
                        ExchangeRates = (from er in _unitOfWork.OrderExchangeRates.Query(false, false)
                                where er.OrderId == o.Id && !er.IsDeleted
                                select new { er.Currency, er.ExchangeRate, er.ExchangeRateDate, er.IsOfficial }).ToList(),
                        Lines = (from ol in _unitOfWork.OrderLines.Query(false, false)
                                where ol.OrderId == o.Id && !ol.IsDeleted
                                select new
                                {
                                    ol.ProductCode,
                                    ProductName = ol.ProductCode,
                                    GroupCode = "",
                                    ol.Quantity,
                                    ol.UnitPrice,
                                    ol.DiscountRate1,
                                    ol.DiscountAmount1,
                                    ol.DiscountRate2,
                                    ol.DiscountAmount2,
                                    ol.DiscountRate3,
                                    ol.DiscountAmount3,
                                    ol.VatRate,
                                    ol.VatAmount,
                                    ol.LineTotal,
                                    ol.LineGrandTotal,
                                    ol.Description,
                                    HtmlDescription = ol.RelatedStockId.HasValue
                                        ? (_unitOfWork.StockDetails.Query(false, false).Where(sd => sd.StockId == ol.RelatedStockId && !sd.IsDeleted).Select(sd => sd.HtmlDescription).FirstOrDefault() ?? "")
                                        : "",
                                    DefaultImagePath = ol.RelatedStockId.HasValue
                                        ? (_unitOfWork.Repository<StockImage>().Query(false, false).Where(si => si.StockId == ol.RelatedStockId && !si.IsDeleted).OrderByDescending(si => si.IsPrimary).ThenBy(si => si.SortOrder).Select(si => si.FilePath).FirstOrDefault() ?? "")
                                        : ""
                                }).ToList()
                    }).FirstOrDefaultAsync(),

                _ => throw new ArgumentException($"Unsupported rule type: {ruleType}")
            };
        }
    }
}
