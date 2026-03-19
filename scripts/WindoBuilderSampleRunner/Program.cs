using System.Text;
using System.Text.Json;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Infrastructure;
using crm_api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace WindoBuilderSampleRunner;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var root = ResolveRepositoryRoot();
        var samplePath = args.Length > 0
            ? Path.GetFullPath(args[0])
            : Path.Combine(root, "pdf-samples", "windo-quotation-sample.json");
        var outputPath = args.Length > 1
            ? Path.GetFullPath(args[1])
            : Path.Combine(root, "tmp", "candidate-windo-builder.pdf");
        var imagePagePaths = args.Skip(2)
            .Select(Path.GetFullPath)
            .Where(File.Exists)
            .Take(3)
            .ToList();

        var json = await File.ReadAllTextAsync(samplePath);
        var sample = JsonSerializer.Deserialize<WindoQuotationSample>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        }) ?? throw new InvalidOperationException("Sample JSON could not be parsed.");

        sample.Lines = ExpandLines(sample.Lines, sample.RepeatFactor);

        var service = new PdfReportDocumentGeneratorService(
            null!,
            NullLogger<PdfReportDocumentGeneratorService>.Instance,
            null!,
            Options.Create(new PdfBuilderOptions()));

        var template = BuildTemplate(root, imagePagePaths);
        var bytes = await service.GeneratePdfForEntityDataAsync(template, sample);

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        await File.WriteAllBytesAsync(outputPath, bytes);
        Console.WriteLine(outputPath);
    }

    private static ReportTemplateData BuildTemplate(string root, IReadOnlyList<string> imagePagePaths)
    {
        var imagePageCount = imagePagePaths.Count;
        var baseElements = new List<ReportElement>
        {
            Shape("shape-top", 1, 0, 0, 794, 14, "#345A99"),
            Box("hero-left", 1, 24, 28, 250, 120),
            Box("hero-right", 1, 292, 28, 478, 120),
            Image("hero-logo", 1, 102, 52, 94, 24, ToDataUri(Path.Combine(root, "verii_crm_web", "public", "logo.png"))),
            Text("hero-title", 1, 96, 112, 120, 20, "FIYAT TEKLIFI", 12, "#345A99"),
            Text("offer-label", 1, 316, 44, 170, 18, "TEKLIF BILGILERI", 11, "#345A99"),
            Text("offer-no-label", 1, 316, 70, 88, 16, "Teklif No:", 8.5m, "#475569"),
            Field("offer-no", 1, 664, 70, 90, 16, "OfferNo"),
            Text("offer-date-label", 1, 316, 90, 88, 16, "Tarih:", 8.5m, "#475569"),
            Field("offer-date", 1, 664, 90, 90, 16, "OfferDate"),
            Text("offer-delivery-label", 1, 316, 110, 88, 16, "Teslim:", 8.5m, "#475569"),
            Field("offer-delivery", 1, 664, 110, 90, 16, "DeliveryDate"),

            Box("company-box", 1, 24, 166, 370, 120),
            Box("customer-box", 1, 410, 166, 360, 120),
            Text("company-title", 1, 42, 184, 180, 18, "FIRMA BILGILERI", 11, "#345A99"),
            Text("company-name", 1, 42, 210, 220, 18, "WINDOFORM KAPI & PENCERE AKS.", 12, "#0f172a"),
            Text("company-line-1", 1, 42, 234, 300, 16, "Kazim Karabekir Mah. 8501 Sokak No:7-B D:18 Buca / Izmir", 8, "#64748b"),
            Text("company-line-2", 1, 42, 252, 220, 16, "(0232) 854 70 00", 8, "#64748b"),
            Text("company-line-3", 1, 42, 270, 220, 16, "info@windoform.com.tr", 8, "#64748b"),
            Text("customer-title", 1, 428, 184, 180, 18, "MUSTERI (CARI)", 11, "#345A99"),
            Field("customer-name", 1, 428, 210, 260, 18, "CustomerName"),
            Field("customer-rep", 1, 428, 232, 260, 18, "RepresentativeName"),
            Field("customer-address", 1, 428, 252, 300, 16, "ShippingAddressText"),
            Field("customer-code", 1, 428, 270, 160, 16, "ErpCustomerCode"),

            new ReportElement
            {
                Id = "quotation-table",
                Type = "table",
                Section = "content",
                X = 24,
                Y = 382,
                Width = 746,
                Height = 360,
                PageNumbers = new List<int> { 1 },
                Columns = new List<TableColumn>
                {
                    new() { Label = "Gorsel", Path = "Lines.ProductCode", Format = "text", Width = 56 },
                    new() { Label = "Stok Kodu", Path = "Lines.ProductCode", Width = 92 },
                    new() { Label = "Stok Adi / Aciklama", Path = "Lines.ProductName", Width = 250 },
                    new() { Label = "Miktar", Path = "Lines.Quantity", Width = 64, Align = "right", Format = "number" },
                    new() { Label = "Birim Fiyat", Path = "Lines.UnitPrice", Width = 96, Align = "right", Format = "currency" },
                    new() { Label = "Iskonto", Path = "Lines.DiscountRate1", Width = 76, Align = "right", Format = "number" },
                    new() { Label = "Net Toplam", Path = "Lines.LineTotal", Width = 112, Align = "right", Format = "currency" },
                },
                HeaderStyle = new TableStyle { BackgroundColor = "#345A99", Color = "#ffffff", FontSize = 9 },
                RowStyle = new TableStyle { FontSize = 7.5m, Color = "#334155", BackgroundColor = "#ffffff" },
                AlternateRowStyle = new TableStyle { FontSize = 7.5m, Color = "#334155", BackgroundColor = "#ffffff" },
                TableOptions = new TableOptions
                {
                    RepeatHeader = true,
                    PageBreak = "auto",
                    Dense = true,
                    ShowBorders = true,
                    GroupByPath = null,
                    GroupHeaderLabel = "Proje",
                    ShowGroupFooter = false,
                    GroupFooterLabel = "Grup Toplami",
                    GroupFooterValuePath = "LineTotal",
                    DetailColumnPath = "Lines.ProductName",
                    DetailPaths = new List<string> { "Description1", "Description2", "Description3" },
                    DetailLineFontSize = 7,
                    DetailLineColor = "#64748b",
                    ReportRegionMode = "flow",
                    ContinuationElementIds = new List<string> { "shape-top-repeat" },
                    FlowElementIds = new List<string>
                    {
                        "approval-box",
                        "approval-title",
                        "approval-sign",
                        "quotation-totals",
                        "footer-bg",
                        "footer-title",
                        "footer-delivery",
                        "footer-note",
                        "refs-title",
                        "refs-copy",
                        "ref-1",
                        "ref-2",
                        "ref-3",
                    },
                    RepeatedElementIds = new List<string> { "page-footer-line", "page-footer-copy" },
                    FirstPageBudget = 650,
                    ContinuationPageBudget = 900,
                    LastPageBudget = 48,
                },
                Style = new ElementStyle
                {
                    Border = "1px solid #d7dde8",
                    Background = "#ffffff",
                    Radius = 12,
                },
            },

            Box("approval-box", 1, 24, 570, 272, 84),
            Text("approval-title", 1, 40, 590, 160, 16, "MUSTERI ONAYI", 9, "#64748b"),
            Shape("approval-line", 1, 48, 624, 220, 1, "#d2d8e2"),
            Text("approval-sign", 1, 112, 638, 120, 14, "Kase ve imza", 9, "#94a3b8"),
            Shape("shape-top-repeat", 2, 0, 0, 794, 14, "#345A99"),
            new ReportElement
            {
                Id = "quotation-totals",
                Type = "quotationTotals",
                Section = "content",
                X = 528,
                Y = 570,
                Width = 242,
                Height = 124,
                Text = "TOPLAM OZETI",
                PageNumbers = new List<int> { 1 },
                QuotationTotalsOptions = new QuotationTotalsOptions
                {
                    Layout = "single",
                    CurrencyMode = "code",
                    CurrencyPath = "Currency",
                    GrossLabel = "Brut Toplam",
                    DiscountLabel = "Iskonto",
                    NetLabel = "Net Toplam",
                    VatLabel = "KDV",
                    GrandLabel = "Genel Toplam",
                    ShowGross = true,
                    ShowDiscount = true,
                    ShowVat = true,
                    EmphasizeGrandTotal = true,
                    ShowNote = false,
                    HideEmptyNote = true,
                },
                Style = new ElementStyle
                {
                    Background = "#ffffff",
                    Border = "1px solid #d7dde8",
                    Radius = 12,
                },
            },

            Shape("footer-bg", 1, 0, 724, 794, 170, "#f8fafc"),
            Shape("notes-strip", 1, 24, 740, 4, 114, "#345A99"),
            Text("footer-title", 1, 40, 742, 320, 18, "TEKLIF SARTLARI VE ONEMLI NOTLAR", 10, "#345A99"),
            Box("delivery-badge", 1, 40, 772, 220, 28, "#f8fafc", "1px solid #cbd5e1"),
            Field("footer-delivery", 1, 48, 780, 280, 16, "SalesTypeDefinitionName"),
            Field("footer-note", 1, 40, 814, 700, 54, "Note1"),
            Text("refs-title", 1, 24, 896, 340, 18, "SAHA VE KESIF GORSELLERI (REFERANS)", 10, "#345A99"),
            Text("refs-copy", 1, 24, 920, 520, 16, "Ornek saha, montaj ve referans gorselleri", 9, "#64748b"),
            Image("ref-1", 1, 24, 948, 172, 82, ToDataUri(Path.Combine(root, "verii_crm_web", "public", "logo.png"))),
            Image("ref-2", 1, 220, 948, 172, 82, ToDataUri(Path.Combine(root, "verii_crm_web", "public", "login.jpg"))),
            Image("ref-3", 1, 416, 948, 172, 82, ToDataUri(Path.Combine(root, "verii_crm_web", "public", "v3rii.jpeg"))),
            Shape("page-footer-line", 1, 24, 1088, 746, 2, "#d7dde8"),
            Text("page-footer-copy", 1, 24, 1096, 220, 12, "WINDOFORM teklif dokumani", 8, "#94a3b8"),
        };

        if (imagePageCount > 0)
        {
            foreach (var element in baseElements)
            {
                if (element.PageNumbers == null || element.PageNumbers.Count == 0)
                    continue;

                element.PageNumbers = element.PageNumbers
                    .Select(pageNumber => pageNumber + imagePageCount)
                    .ToList();
            }

            var imagePages = new List<ReportElement>();
            for (var index = 0; index < imagePageCount; index++)
            {
                var pageNumber = index + 1;
                imagePages.Add(new ReportElement
                {
                    Id = $"image-page-{pageNumber}",
                    Type = "image",
                    Section = "page",
                    X = 0,
                    Y = 0,
                    Width = 794,
                    Height = 1123,
                    PageNumbers = new List<int> { pageNumber },
                    Value = ToDataUri(imagePagePaths[index]),
                    Style = new ElementStyle { ImageFit = "cover" },
                });
            }

            imagePages.AddRange(baseElements);
            baseElements = imagePages;
        }

        return new ReportTemplateData
        {
            Page = new PageConfig
            {
                Width = 794,
                Height = 1123,
                Unit = "px",
                PageCount = Math.Max(3, 1 + imagePageCount),
            },
            Elements = baseElements
        };
    }

    private static ReportElement Shape(string id, int page, decimal x, decimal y, decimal width, decimal height, string background)
        => new()
        {
            Id = id,
            Type = "shape",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            PageNumbers = new List<int> { page },
            Style = new ElementStyle { Background = background, Radius = 12 },
        };

    private static ReportElement Box(string id, int page, decimal x, decimal y, decimal width, decimal height, string background = "#ffffff", string border = "1px solid #d7dde8")
        => new()
        {
            Id = id,
            Type = "shape",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            PageNumbers = new List<int> { page },
            Style = new ElementStyle { Background = background, Border = border, Radius = 12 },
        };

    private static ReportElement Text(string id, int page, decimal x, decimal y, decimal width, decimal height, string text, decimal fontSize, string color)
        => new()
        {
            Id = id,
            Type = "text",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            PageNumbers = new List<int> { page },
            Text = text,
            FontSize = fontSize,
            Color = color,
            FontFamily = "Arial",
        };

    private static ReportElement Field(string id, int page, decimal x, decimal y, decimal width, decimal height, string path)
        => new()
        {
            Id = id,
            Type = "field",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            PageNumbers = new List<int> { page },
            Path = path,
            FontSize = 11,
            Color = "#475569",
            FontFamily = "Arial",
        };

    private static ReportElement Image(string id, int page, decimal x, decimal y, decimal width, decimal height, string? value)
        => new()
        {
            Id = id,
            Type = "image",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            PageNumbers = new List<int> { page },
            Value = value,
            Style = new ElementStyle { ImageFit = "cover", Border = "1px solid #d7dde8", Radius = 12 },
        };

    private static string? ToDataUri(string path)
    {
        if (!File.Exists(path))
            return null;
        var bytes = File.ReadAllBytes(path);
        var ext = Path.GetExtension(path).ToLowerInvariant();
        var mime = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream",
        };
        return $"data:{mime};base64,{Convert.ToBase64String(bytes)}";
    }

    private static string ResolveRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            var candidate = Path.Combine(current.FullName, "pdf-samples");
            if (Directory.Exists(candidate))
                return current.FullName;
            current = current.Parent;
        }
        throw new DirectoryNotFoundException("Repository root could not be resolved.");
    }

    private static List<WindoQuotationSampleLine> ExpandLines(List<WindoQuotationSampleLine> lines, int repeatFactor)
    {
        if (repeatFactor <= 1)
            return lines;

        var expanded = new List<WindoQuotationSampleLine>();
        for (var iteration = 1; iteration <= repeatFactor; iteration++)
        {
            foreach (var line in lines)
            {
                expanded.Add(new WindoQuotationSampleLine
                {
                    ProductCode = string.IsNullOrWhiteSpace(line.ProductCode) ? line.ProductCode : $"{line.ProductCode}-{iteration}",
                    ProductName = repeatFactor > 1 ? $"{line.ProductName} Faz {iteration}" : line.ProductName,
                    Description = line.Description,
                    Description1 = line.Description1,
                    Description2 = line.Description2,
                    Description3 = line.Description3,
                    Quantity = line.Quantity,
                    UnitPrice = line.UnitPrice,
                    VatRate = line.VatRate,
                    VatAmount = line.VatAmount,
                    LineTotal = line.LineTotal,
                    LineGrandTotal = line.LineGrandTotal,
                    DiscountRate1 = line.DiscountRate1,
                    DiscountAmount1 = line.DiscountAmount1,
                    DiscountRate2 = line.DiscountRate2,
                    DiscountAmount2 = line.DiscountAmount2,
                    DiscountRate3 = line.DiscountRate3,
                    DiscountAmount3 = line.DiscountAmount3,
                    ErpProjectCode = string.IsNullOrWhiteSpace(line.ErpProjectCode) ? line.ErpProjectCode : $"{line.ErpProjectCode}-{iteration}",
                });
            }
        }

        return expanded;
    }
}

public sealed class WindoQuotationSample
{
    public int RepeatFactor { get; set; } = 1;
    public string? OfferNo { get; set; }
    public string? OfferDate { get; set; }
    public string? DeliveryDate { get; set; }
    public string? ValidUntil { get; set; }
    public string? CustomerName { get; set; }
    public string? PotentialCustomerName { get; set; }
    public string? RepresentativeName { get; set; }
    public string? ShippingAddressText { get; set; }
    public string? ErpCustomerCode { get; set; }
    public string? PaymentTypeName { get; set; }
    public string? SalesTypeDefinitionName { get; set; }
    public string? ErpProjectCode { get; set; }
    public string? DocumentSerialTypeName { get; set; }
    public string? Description { get; set; }
    public string? Currency { get; set; }
    public decimal? GeneralDiscountRate { get; set; }
    public decimal? GeneralDiscountAmount { get; set; }
    public decimal Total { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Note1 { get; set; }
    public string? Note2 { get; set; }
    public string? Note3 { get; set; }
    public string? Note4 { get; set; }
    public string? Note5 { get; set; }
    public string? Note6 { get; set; }
    public List<WindoQuotationSampleLine> Lines { get; set; } = new();
}

public sealed class WindoQuotationSampleLine
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
    public decimal DiscountAmount1 { get; set; }
    public decimal DiscountRate2 { get; set; }
    public decimal DiscountAmount2 { get; set; }
    public decimal DiscountRate3 { get; set; }
    public decimal DiscountAmount3 { get; set; }
    public string? ErpProjectCode { get; set; }
}
