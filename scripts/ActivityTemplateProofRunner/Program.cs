using System.Text.Json;
using crm_api.Infrastructure;
using crm_api.Modules.PdfBuilder.Application.Dtos;
using crm_api.Modules.PdfBuilder.Application.Services;
using crm_api.Modules.PdfBuilder.Domain.Entities;
using crm_api.UnitOfWork;
using crm_api.Modules.Integrations.Domain.ReadModels;
using crm_api.Modules.Integrations.Application.Dtos.Erp;
using crm_api.Modules.Integrations.Application.Services;
using crm_api.Shared.Common.Application;
using crm_api.Shared.Common.Dtos;
using crm_api.Shared.Infrastructure.Abstractions;
using crm_api.Shared.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ActivityTemplateProofRunner;

public static class Program
{
    private const long ActivityId = 70;
    private const string TemplateTitle = "Windo Yeni Aktivite Formu";

    public static async Task Main(string[] args)
    {
        var apiRoot = ResolveApiRoot();
        var root = Directory.GetParent(apiRoot)!.FullName;
        var configuration = BuildConfiguration(apiRoot);
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection could not be resolved.");
        var outputPdf = args.Length > 0
            ? Path.GetFullPath(args[0])
            : Path.Combine(root, "pdf-samples", "activity-70-proof-windo-yeni.pdf");

        var dbOptions = new DbContextOptionsBuilder<CmsDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var fakeLocalization = new FakeLocalizationService();
        var httpAccessor = new HttpContextAccessor();

        await using var db = new CmsDbContext(dbOptions);
        using var uow = new UnitOfWork(db, httpAccessor, fakeLocalization);

        var activity = await db.Activities
            .Include(x => x.ActivityType)
            .Include(x => x.PaymentType)
            .Include(x => x.ActivityMeetingType)
            .Include(x => x.ActivityTopicPurpose)
            .Include(x => x.ActivityShipping)
            .Include(x => x.AssignedUser)
            .Include(x => x.Contact)
            .Include(x => x.PotentialCustomer)
                .ThenInclude(x => x!.DefaultShippingAddress)
            .FirstOrDefaultAsync(x => x.Id == ActivityId && !x.IsDeleted)
            .ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Activity {ActivityId} not found.");

        var template = await UpsertTemplateAsync(db).ConfigureAwait(false);

        var templateData = JsonSerializer.Deserialize<ReportTemplateData>(template.TemplateJson, SerializerOptions)
            ?? throw new InvalidOperationException("Template JSON could not be deserialized.");

        var generator = new PdfReportDocumentGeneratorService(
            uow,
            NullLogger<PdfReportDocumentGeneratorService>.Instance,
            null!,
            Options.Create(new PdfBuilderOptions
            {
                LocalImageBasePath = apiRoot,
            }),
            new FakeErpService());

        var bytes = await generator.GeneratePdfAsync(DocumentRuleType.Activity, activity.Id, templateData).ConfigureAwait(false);
        Directory.CreateDirectory(Path.GetDirectoryName(outputPdf)!);
        await File.WriteAllBytesAsync(outputPdf, bytes).ConfigureAwait(false);

        Console.WriteLine($"template:{template.Id}|{template.Title}");
        Console.WriteLine($"activity:{activity.Id}|{activity.Subject}");
        Console.WriteLine($"paymentTypeId:{activity.PaymentTypeId}");
        Console.WriteLine($"activityMeetingTypeId:{activity.ActivityMeetingTypeId}");
        Console.WriteLine($"activityTopicPurposeId:{activity.ActivityTopicPurposeId}");
        Console.WriteLine($"activityShippingId:{activity.ActivityShippingId}");
        Console.WriteLine($"assigned:{activity.AssignedUser?.FullName}|{activity.AssignedUser?.Email}");
        Console.WriteLine($"contact:{activity.Contact?.FullName}|{activity.Contact?.Email}|{activity.Contact?.Phone}|{activity.Contact?.Mobile}");
        Console.WriteLine($"customer:{activity.PotentialCustomer?.CustomerName}|{activity.PotentialCustomer?.Email}|{activity.PotentialCustomer?.Phone1}|{activity.PotentialCustomer?.Phone2}");
        Console.WriteLine($"pdf:{outputPdf}");
    }

    private static JsonSerializerOptions SerializerOptions => new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private static async Task<ReportTemplate> UpsertTemplateAsync(CmsDbContext db)
    {
        var template = await db.ReportTemplates
            .Where(x => !x.IsDeleted && x.RuleType == DocumentRuleType.Activity)
            .FirstOrDefaultAsync(x => x.Title == TemplateTitle)
            .ConfigureAwait(false);

        var templateData = BuildTemplateData();
        var serialized = JsonSerializer.Serialize(templateData);

        if (template == null)
        {
            template = new ReportTemplate
            {
                RuleType = DocumentRuleType.Activity,
                Title = TemplateTitle,
                TemplateJson = serialized,
                IsActive = true,
                Default = false,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow,
            };
            db.ReportTemplates.Add(template);
        }
        else
        {
            template.TemplateJson = serialized;
            template.IsActive = true;
            template.UpdatedBy = 1;
            template.UpdatedDate = DateTime.UtcNow;
        }

        await db.SaveChangesAsync().ConfigureAwait(false);
        return template;
    }

    private static ReportTemplateData BuildTemplateData()
    {
        var logoPath = Path.Combine(Directory.GetParent(ResolveApiRoot())!.FullName, "pdf-samples", "assets", "bilginoglu-endustri-logo.png");
        var elements = new List<ReportElement>
        {
            CreateShape("page-bg", 8, 8, 194, 281, "#e5edf8", "#ffffff", "1px solid #d6e0ef"),
            CreateShape("header-band", 8, 8, 194, 24, "#123f7a", "#123f7a"),
            new()
            {
                Id = "logo",
                Type = "image",
                Section = "page",
                X = 16,
                Y = 12,
                Width = 52,
                Height = 14,
                Value = ToDataUri(logoPath),
                Style = new ElementStyle
                {
                    ImageFit = "contain",
                },
            },
            CreateText("title", 74, 14, 98, 7, "WINDO YENI FUAR GORUSME FORMU", 15, true, "Helvetica-Bold"),
            CreateFieldBox("fair-box", 156, 11, 36, 17),
            CreateText("fair-label", 160, 14, 28, 4, "FUAR ADI", 8.5m, true, "Helvetica-Bold"),
            CreateText("fair-name", 160, 18.5m, 28, 4, "KAZAKHSTAN 2026", 7.5m, true, "Helvetica-Bold"),
            CreateText("fair-date", 160, 22.5m, 28, 3.5m, "01-03 NISAN", 6.5m, false),

            CreatePanelLabel("company-label", 16, 38, 64, "FIRMA ADI, ADRESI"),
            CreateFieldBox("company-box", 16, 44, 110, 28),
            CreateField("company-name", 20, 48, 102, 5, "CustomerName", false, 10, "Helvetica-Bold"),
            CreateField("company-address", 20, 55, 102, 12, "CustomerAddress", true, 8.5m),

            CreatePanelLabel("contact-label", 16, 78, 78, "GORUSULEN KISI / GOREVI / E-POSTA"),
            CreateFieldBox("contact-box", 16, 84, 110, 18),
            CreateField("contact-name", 20, 89, 102, 5, "ContactName", false, 9.5m, "Helvetica-Bold"),
            CreateField("contact-email", 20, 95, 102, 4, "ContactEmail", false, 8),

            CreatePanelLabel("phone-label", 16, 108, 24, "TELEFON"),
            CreateFieldBox("phone-box", 16, 114, 52, 14),
            CreateField("phone-field", 20, 119, 44, 4, "ContactPhone", false, 9, "Helvetica-Bold"),

            CreatePanelLabel("visitor-label", 74, 108, 38, "GORUSEN KISI"),
            CreateFieldBox("visitor-box", 74, 114, 52, 14),
            CreateField("visitor-field", 78, 119, 44, 4, "AssignedUserName", false, 9, "Helvetica-Bold"),

            CreatePanelLabel("card-label", 132, 38, 46, "MUSTERI KARTVIZITI"),
            CreateFieldBox("card-box", 132, 44, 60, 84),
            new()
            {
                Id = "customer-card-image",
                Type = "image",
                Section = "page",
                X = 136,
                Y = 48,
                Width = 52,
                Height = 76,
                Path = "CustomerLatestImageUrl",
                Style = new ElementStyle
                {
                    ImageFit = "contain",
                    Border = "1px dashed #8fa7c6",
                    Padding = 4,
                },
            },

            CreatePanelLabel("date-label", 16, 138, 36, "ZIYARET TARIHI"),
            CreateFieldBox("date-box", 16, 144, 38, 16),
            CreateField("date-field", 20, 150, 30, 5, "StartDateTime", false, 9.5m, "Helvetica-Bold"),

            CreatePanelLabel("shipping-label", 60, 138, 28, "TESLIMAT"),
            CreateFieldBox("shipping-box", 60, 144, 40, 16),
            CreateField("shipping-field", 64, 150, 32, 5, "ActivityShippingName", true, 9.5m, "Helvetica-Bold"),

            CreatePanelLabel("payment-label", 106, 138, 22, "ODEME"),
            CreateFieldBox("payment-box", 106, 144, 40, 16),
            CreateField("payment-field", 110, 150, 32, 5, "PaymentTypeName", true, 9.5m, "Helvetica-Bold"),

            CreatePanelLabel("meeting-label", 152, 138, 24, "GORUSME"),
            CreateFieldBox("meeting-box", 152, 144, 40, 16),
            CreateField("meeting-field", 156, 150, 32, 5, "ActivityMeetingTypeName", true, 9.5m, "Helvetica-Bold"),

            CreatePanelLabel("topic-label", 16, 170, 58, "ILGILENILEN KONULAR"),
            CreateFieldBox("topic-box", 16, 176, 176, 18),
            CreateField("topic-field", 20, 182, 168, 7, "ActivityTopicPurposeName", true, 9.5m, "Helvetica-Bold"),

            CreatePanelLabel("summary-label", 16, 204, 44, "GORUSME OZETI"),
            CreateFieldBox("summary-box", 16, 210, 176, 56),
        };

        elements.AddRange(CreateGridLines(16, 210, 176, 56, 6, 6));
        elements.Add(CreateField("summary-field", 20, 214, 168, 48, "Description", true, 9));

        elements.Add(CreateText("footer-rev", 160, 274, 26, 4, "REV 02", 6.5m, false));

        return new ReportTemplateData
        {
            SchemaVersion = 1,
            Page = new PageConfig
            {
                Width = 210,
                Height = 297,
                Unit = "mm",
                PageCount = 1,
            },
            Elements = elements
        };
    }

    private static ReportElement CreateText(string id, decimal x, decimal y, decimal width, decimal height, string text, decimal fontSize, bool bold, string? fontFamily = null)
    {
        return new ReportElement
        {
            Id = id,
            Type = "text",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Text = text,
            FontSize = fontSize,
            FontFamily = fontFamily ?? (bold ? "Helvetica-Bold" : "Helvetica"),
            Color = "#1e293b",
        };
    }

    private static ReportElement CreateSectionTitle(string id, decimal x, decimal y, string text)
        => CreateText(id, x, y, 52, 5, text.ToUpperInvariant(), 10.5m, true, "Helvetica-Bold");

    private static ReportElement CreateHeaderLabel(string id, decimal x, decimal y, decimal width, string text)
        => CreateText(id, x, y, width, 4.5m, text.ToUpperInvariant(), 9.5m, true, "Helvetica-Bold");

    private static ReportElement CreatePanelLabel(string id, decimal x, decimal y, decimal width, string text)
        => CreateText(id, x, y, width, 4.8m, text.ToUpperInvariant(), 11.5m, true, "Helvetica-Bold");

    private static ReportElement CreateUnderline(string id, decimal x, decimal y, decimal width)
        => CreateShape(id, x, y, width, 0.35m, "#7b8794");

    private static ReportElement CreateFieldBox(string id, decimal x, decimal y, decimal width, decimal height)
        => CreateShape(id, x, y, width, height, "#b7c0cc", "#ffffff", "1px solid #b7c0cc");

    private static ReportElement CreateCheckboxRow(string id, decimal x, decimal y, string label)
    {
        return new ReportElement
        {
            Id = id,
            Type = "text",
            Section = "page",
            X = x,
            Y = y,
            Width = 28,
            Height = 4,
            Text = $"{label}   □",
            FontSize = 7,
            FontFamily = "Helvetica",
            Color = "#1f2937",
        };
    }

    private static IEnumerable<ReportElement> CreateGridLines(decimal x, decimal y, decimal width, decimal height, decimal stepX, decimal stepY)
    {
        var elements = new List<ReportElement>();
        var rowIndex = 0;
        for (decimal currentY = y + stepY; currentY < y + height; currentY += stepY)
        {
            elements.Add(CreateShape($"grid-h-{rowIndex++}", x, currentY, width, 0.12m, "#e5e7eb"));
        }

        var colIndex = 0;
        for (decimal currentX = x + stepX; currentX < x + width; currentX += stepX)
        {
            elements.Add(CreateShape($"grid-v-{colIndex++}", currentX, y, 0.12m, height, "#e5e7eb"));
        }

        return elements;
    }

    private static ReportElement CreateShape(string id, decimal x, decimal y, decimal width, decimal height, string color, string? background = null, string? border = null)
    {
        return new ReportElement
        {
            Id = id,
            Type = "shape",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Style = new ElementStyle
            {
                Background = background ?? color,
                Border = border,
            },
        };
    }

    private static ReportElement CreateField(string id, decimal x, decimal y, decimal width, decimal height, string path, bool multiline = false, decimal fontSize = 8.5m, string fontFamily = "Helvetica")
    {
        return new ReportElement
        {
            Id = id,
            Type = "field",
            Section = "page",
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Path = path,
            FontSize = fontSize,
            FontFamily = fontFamily,
            Color = "#0f172a",
            TextOverflow = multiline ? "autoHeight" : null,
            Style = new ElementStyle { Padding = 0 }
        };
    }

    private static string ToDataUri(string path)
    {
        var bytes = File.ReadAllBytes(path);
        var ext = Path.GetExtension(path).ToLowerInvariant();
        var mime = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };

        return $"data:{mime};base64,{Convert.ToBase64String(bytes)}";
    }

    private static IConfigurationRoot BuildConfiguration(string apiRoot)
    {
        return new ConfigurationBuilder()
            .SetBasePath(apiRoot)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Local.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    private static string ResolveApiRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            if (File.Exists(Path.Combine(current.FullName, "crm_api.csproj")))
                return current.FullName;

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("API root could not be resolved.");
    }

    private sealed class FakeLocalizationService : ILocalizationService
    {
        public string GetLocalizedString(string key) => key;
        public string GetLocalizedString(string key, params object[] arguments) => string.Format(key, arguments);
    }

    private sealed class FakeErpService : IErpService
    {
        public Task<ApiResponse<short>> GetBranchCodeFromContext() => throw new NotSupportedException();
        public Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu) => throw new NotSupportedException();
        public Task<ApiResponse<List<CariDto>>> GetCarisByCodesAsync(IEnumerable<string> cariKodlari) => throw new NotSupportedException();
        public Task<ApiResponse<List<CariPlasiyerDto>>> GetCariPlasiyerAsync(string? subeKodu = null, string? plasiyerKodu = null) => throw new NotSupportedException();
        public Task<ApiResponse<List<StokFunctionDto>>> GetStoksAsync(string? stokKodu) => throw new NotSupportedException();
        public Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null) => throw new NotSupportedException();
        public Task<ApiResponse<List<ErpCariMovementDto>>> GetCariMovementsAsync(string customerCode) => throw new NotSupportedException();
        public Task<ApiResponse<List<ErpCariBalanceDto>>> GetCariBalancesAsync(string customerCode) => throw new NotSupportedException();
        public Task<ApiResponse<List<ErpShippingAddressDto>>> GetErpShippingAddressAsync(string customerCode) => throw new NotSupportedException();
        public Task<ApiResponse<List<StokGroupDto>>> GetStokGroupAsync(string? grupKodu) => throw new NotSupportedException();
        public Task<ApiResponse<List<ProjeDto>>> GetProjectCodesAsync() => throw new NotSupportedException();
        public Task<ApiResponse<List<EsnYapMasDto>>> GetEsnYapMasAsync() => throw new NotSupportedException();
        public Task<ApiResponse<object>> HealthCheckAsync() => throw new NotSupportedException();
        public Task<ApiResponse<List<KurDto>>> GetExchangeRateAsync(DateTime tarih, int fiyatTipi)
            => Task.FromResult(ApiResponse<List<KurDto>>.SuccessResult(new List<KurDto>(), "ok"));
    }
}
