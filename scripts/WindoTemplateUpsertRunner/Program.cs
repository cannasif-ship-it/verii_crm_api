using System.Text;
using System.Text.Json;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Infrastructure.Time;
using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WindoTemplateUpsertRunner;

public static class Program
{
    private const decimal PxToMm = 210m / 794m;

    public static async Task Main(string[] args)
    {
        var root = ResolveRepositoryRoot();
        var title = args.Length > 0 ? args[0] : "Windo Teklif + 3 Gorsel";
        var imagePaths = args.Skip(1)
            .Select(Path.GetFullPath)
            .Where(File.Exists)
            .Take(3)
            .ToList();

        if (imagePaths.Count != 3)
            throw new InvalidOperationException("Tam 3 gorsel path'i verilmeli.");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(root, "verii_crm_api"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Local.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection bulunamadi.");

        var options = new DbContextOptionsBuilder<CmsDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        await using var db = new CmsDbContext(options);

        var templateData = BuildTemplate(root, imagePaths);
        var templateJson = JsonSerializer.Serialize(templateData, PdfReportTemplateJsonOptions.CamelCase);

        var existing = await db.ReportTemplates
            .Where(x => !x.IsDeleted && x.RuleType == DocumentRuleType.Quotation && x.Title == title)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (existing == null)
        {
            existing = new ReportTemplate
            {
                RuleType = DocumentRuleType.Quotation,
                Title = title,
                TemplateJson = templateJson,
                IsActive = true,
                Default = false,
                CreatedBy = 1,
                CreatedByUserId = 1,
                CreatedDate = DateTimeProvider.Now,
            };
            db.ReportTemplates.Add(existing);
        }
        else
        {
            existing.TemplateJson = templateJson;
            existing.IsActive = true;
            existing.UpdatedBy = 1;
            existing.UpdatedByUserId = 1;
            existing.UpdatedDate = DateTimeProvider.Now;
        }

        await db.SaveChangesAsync();

        Console.WriteLine($"{existing.Id}|{existing.Title}");
    }

    private static ReportTemplateData BuildTemplate(string root, IReadOnlyList<string> imagePagePaths)
    {
        var elements = new List<ReportElement>
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
                X = ScaleX(24),
                Y = ScaleY(382),
                Width = ScaleX(746),
                Height = ScaleY(360),
                PageNumbers = new List<int> { 1 },
                Columns = new List<TableColumn>
                {
                    new() { Label = "Gorsel", Path = "Lines.ProductCode", Format = "text", Width = ScaleX(56) },
                    new() { Label = "Stok Kodu", Path = "Lines.ProductCode", Width = ScaleX(92) },
                    new() { Label = "Stok Adi / Aciklama", Path = "Lines.ProductName", Width = ScaleX(250) },
                    new() { Label = "Miktar", Path = "Lines.Quantity", Width = ScaleX(64), Align = "right", Format = "number" },
                    new() { Label = "Birim Fiyat", Path = "Lines.UnitPrice", Width = ScaleX(96), Align = "right", Format = "currency" },
                    new() { Label = "Iskonto", Path = "Lines.DiscountRate1", Width = ScaleX(76), Align = "right", Format = "number" },
                    new() { Label = "Net Toplam", Path = "Lines.LineTotal", Width = ScaleX(112), Align = "right", Format = "currency" },
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
                        "approval-box","approval-title","approval-sign","quotation-totals","footer-bg","footer-title",
                        "footer-delivery","footer-note","refs-title","refs-copy","ref-1","ref-2","ref-3",
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
                X = ScaleX(528),
                Y = ScaleY(570),
                Width = ScaleX(242),
                Height = ScaleY(124),
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

        var pageNumber = 2;
        foreach (var imagePath in imagePagePaths)
        {
            elements.Add(new ReportElement
            {
                Id = $"image-page-{pageNumber}",
                Type = "image",
                Section = "page",
                X = 0,
                Y = 0,
                Width = 210,
                Height = 297,
                PageNumbers = new List<int> { pageNumber },
                Value = ToDataUri(imagePath),
                Style = new ElementStyle { ImageFit = "cover" },
            });
            pageNumber++;
        }

        return new ReportTemplateData
        {
            SchemaVersion = 1,
            Page = new PageConfig
            {
                Width = 210,
                Height = 297,
                Unit = "mm",
                PageCount = 4,
            },
            Elements = elements.Select(ScaleElement).ToList(),
        };
    }

    private static ReportElement ScaleElement(ReportElement element)
    {
        if (element.Type == "image" && element.Section == "page" && element.X == 0 && element.Y == 0 && element.Width == 210 && element.Height == 297)
            return element;

        element.X = ScaleX(element.X);
        element.Y = ScaleY(element.Y);
        element.Width = ScaleX(element.Width);
        element.Height = ScaleY(element.Height);

        if (element.Columns != null)
        {
            foreach (var column in element.Columns)
            {
                if (column.Width.HasValue)
                    column.Width = ScaleX(column.Width.Value);
            }
        }

        return element;
    }

    private static decimal ScaleX(decimal value) => Math.Round(value * PxToMm, 3);
    private static decimal ScaleY(decimal value) => Math.Round(value * PxToMm, 3);

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
            Style = new ElementStyle
            {
                Background = background,
                Border = border,
                Radius = 12
            }
        };

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
            Style = new ElementStyle
            {
                Background = background,
                Radius = height <= 4 ? 999 : 0
            }
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
            FontFamily = "Outfit"
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
            FontSize = 8.5m,
            Color = "#334155",
            FontFamily = "Outfit"
        };

    private static ReportElement Image(string id, int page, decimal x, decimal y, decimal width, decimal height, string dataUri)
        => new()
        {
            Id = id,
            Type = "image",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Value = dataUri,
            PageNumbers = new List<int> { page },
            Style = new ElementStyle
            {
                Border = "1px solid #d7dde8",
                Radius = 12,
                ImageFit = "contain"
            }
        };

    private static string ToDataUri(string path)
    {
        var bytes = File.ReadAllBytes(path);
        var extension = Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
        var mime = extension switch
        {
            "png" => "image/png",
            "jpg" => "image/jpeg",
            "jpeg" => "image/jpeg",
            "webp" => "image/webp",
            _ => "application/octet-stream"
        };
        return $"data:{mime};base64,{Convert.ToBase64String(bytes)}";
    }

    private static string ResolveRepositoryRoot()
    {
        var current = AppContext.BaseDirectory;
        for (var i = 0; i < 8; i++)
        {
            if (File.Exists(Path.Combine(current, "verii_crm_api", "crm_api.csproj")) &&
                Directory.Exists(Path.Combine(current, "verii_crm_web")))
                return current;

            var parent = Directory.GetParent(current);
            if (parent == null)
                break;
            current = parent.FullName;
        }
        throw new DirectoryNotFoundException("Repository root bulunamadi.");
    }
}
