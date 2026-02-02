using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;

namespace crm_api.Services
{
    public class ReportPdfGeneratorService : IReportPdfGeneratorService
    {
        private readonly CmsDbContext _context;
        private readonly ILogger<ReportPdfGeneratorService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportPdfGeneratorService(
            CmsDbContext context,
            ILogger<ReportPdfGeneratorService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _logger = logger;
            _httpClientFactory = httpClientFactory;

            // Set QuestPDF license (Community license for non-commercial use)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GeneratePdfAsync(DocumentRuleType ruleType, long entityId, ReportTemplateData templateData)
        {
            // Fetch the entity data based on rule type
            var entityData = await FetchEntityDataAsync(ruleType, entityId);
            if (entityData == null)
            {
                throw new InvalidOperationException($"Entity with ID {entityId} not found for rule type {ruleType}");
            }

            // Convert page size from px to points (96 DPI to 72 DPI)
            var pageWidthPt = ConvertToPoints(templateData.Page.Width, templateData.Page.Unit);
            var pageHeightPt = ConvertToPoints(templateData.Page.Height, templateData.Page.Unit);

            // Generate PDF using simple layout
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(new PageSize((float)pageWidthPt, (float)pageHeightPt));
                    page.Margin(20);

                    // Header section
                    var headerElements = templateData.Elements.Where(e => e.Section == "header").ToList();
                    if (headerElements.Any())
                    {
                        page.Header().Column(column =>
                        {
                            RenderElementsSimple(column, headerElements, entityData);
                        });
                    }

                    // Footer section
                    var footerElements = templateData.Elements.Where(e => e.Section == "footer").ToList();
                    if (footerElements.Any())
                    {
                        page.Footer().Column(column =>
                        {
                            RenderElementsSimple(column, footerElements, entityData);
                        });
                    }

                    // Content section
                    var contentElements = templateData.Elements.Where(e => e.Section == "content").ToList();
                    page.Content().Column(column =>
                    {
                        RenderElementsSimple(column, contentElements, entityData);
                    });
                });
            });

            return document.GeneratePdf();
        }

        private async Task<object?> FetchEntityDataAsync(DocumentRuleType ruleType, long entityId)
        {
            return ruleType switch
            {
                DocumentRuleType.Demand => await (from d in _context.Demands
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
                        Lines = (from dl in _context.DemandLines
                                 where dl.DemandId == d.Id && !dl.IsDeleted
                                 select new
                                 {
                                     dl.ProductCode,
                                     ProductName = dl.ProductCode, // Model'de ProductName yok, ProductCode kullan
                                     GroupCode = "", // Model'de GroupCode yok
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
                                     dl.Description
                                 }).ToList()
                    }).FirstOrDefaultAsync(),

                DocumentRuleType.Quotation => await (from q in _context.Quotations
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
                        Lines = (from ql in _context.QuotationLines
                                 where ql.QuotationId == q.Id && !ql.IsDeleted
                                 select new
                                 {
                                     ql.ProductCode,
                                     ProductName = ql.ProductCode, // Model'de ProductName yok, ProductCode kullan
                                     GroupCode = "", // Model'de GroupCode yok
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
                                     ql.Description
                                 }).ToList()
                    }).FirstOrDefaultAsync(),

                DocumentRuleType.Order => await (from o in _context.Orders
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
                        Lines = (from ol in _context.OrderLines
                                 where ol.OrderId == o.Id && !ol.IsDeleted
                                 select new
                                 {
                                     ol.ProductCode,
                                     ProductName = ol.ProductCode, // Model'de ProductName yok, ProductCode kullan
                                     GroupCode = "", // Model'de GroupCode yok
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
                                     ol.Description
                                 }).ToList()
                    }).FirstOrDefaultAsync(),

                _ => throw new ArgumentException($"Unsupported rule type: {ruleType}")
            };
        }

        private void RenderElementsSimple(ColumnDescriptor column, List<ReportElement> elements, object entityData)
        {
            foreach (var element in elements.OrderBy(e => e.Y).ThenBy(e => e.X))
            {
                switch (element.Type.ToLower())
                {
                    case "text":
                        RenderTextElementSimple(column, element);
                        break;

                    case "field":
                        RenderFieldElementSimple(column, element, entityData);
                        break;

                    case "image":
                        RenderImageElementSimple(column, element);
                        break;

                    case "table":
                        RenderTableElementSimple(column, element, entityData);
                        break;
                }
            }
        }

        private void RenderTextElementSimple(ColumnDescriptor column, ReportElement element)
        {
            if (string.IsNullOrEmpty(element.Text))
                return;

            var fontSize = element.FontSize ?? 12;
            
            column.Item().Text(element.Text).FontSize((float)fontSize);
        }

        private void RenderFieldElementSimple(ColumnDescriptor column, ReportElement element, object entityData)
        {
            if (string.IsNullOrEmpty(element.Path))
                return;

            var value = ResolvePropertyPath(entityData, element.Path);
            var displayValue = value?.ToString() ?? string.Empty;

            var fontSize = element.FontSize ?? 12;
            
            column.Item().Text(displayValue).FontSize((float)fontSize);
        }

        private void RenderImageElementSimple(ColumnDescriptor column, ReportElement element)
        {
            if (string.IsNullOrEmpty(element.Value))
                return;

            try
            {
                byte[] imageBytes;

                if (element.Value.StartsWith("data:"))
                {
                    // Base64 data URI
                    var base64Data = element.Value.Split(',')[1];
                    imageBytes = Convert.FromBase64String(base64Data);
                }
                else
                {
                    // URL - fetch image
                    using var httpClient = _httpClientFactory.CreateClient();
                    imageBytes = httpClient.GetByteArrayAsync(element.Value).GetAwaiter().GetResult();
                }

                column.Item().Image(imageBytes);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load image from {Source}", element.Value);
            }
        }

        private void RenderTableElementSimple(ColumnDescriptor column, ReportElement element, object entityData)
        {
            if (element.Columns == null || !element.Columns.Any())
                return;

            // Get table data
            var firstColumnPath = element.Columns[0].Path;
            var collectionName = firstColumnPath.Contains('.') ? firstColumnPath.Split('.')[0] : firstColumnPath;
            var collection = ResolvePropertyPath(entityData, collectionName) as IEnumerable<object>;

            if (collection == null)
                return;

            var rows = collection.ToList();

            column.Item().Table(table =>
            {
                // Define columns
                foreach (var col in element.Columns)
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                    });
                }

                // Header row
                table.Header(header =>
                {
                    foreach (var col in element.Columns)
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text(col.Label).Bold().FontSize(10);
                    }
                });

                // Data rows
                foreach (var row in rows)
                {
                    foreach (var col in element.Columns)
                    {
                        var propertyPath = col.Path.Contains('.') ? col.Path.Split('.', 2)[1] : col.Path;
                        var cellValue = ResolvePropertyPath(row, propertyPath)?.ToString() ?? string.Empty;
                        
                        table.Cell().Border(1).Padding(5).Text(cellValue).FontSize(9);
                    }
                }
            });
        }

        private object? ResolvePropertyPath(object obj, string path)
        {
            if (obj == null || string.IsNullOrEmpty(path))
                return null;

            var parts = path.Split('.');
            var current = obj;

            foreach (var part in parts)
            {
                if (current == null)
                    return null;

                var type = current.GetType();
                var property = type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (property == null)
                    return null;

                current = property.GetValue(current);
            }

            return current;
        }

        private decimal ConvertToPoints(decimal value, string unit)
        {
            return unit.ToLower() switch
            {
                "px" => value * 72m / 96m, // Convert 96 DPI to 72 DPI
                "pt" => value,
                _ => value
            };
        }

    }
}
