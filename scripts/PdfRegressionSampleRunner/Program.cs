using System.Text.Json;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Infrastructure;
using crm_api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace PdfRegressionSampleRunner;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var root = ResolveRepositoryRoot();
        var samplePath = args.Length > 0
            ? Path.GetFullPath(args[0])
            : Path.Combine(root, "pdf-samples", "quotation-totals-regression-sample.json");
        var outputPath = args.Length > 1
            ? Path.GetFullPath(args[1])
            : Path.Combine(root, "tmp", "quotation-totals-candidate.pdf");

        var json = await File.ReadAllTextAsync(samplePath);
        var sample = JsonSerializer.Deserialize<QuotationTotalsRegressionSample>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        }) ?? throw new InvalidOperationException("Sample JSON could not be parsed.");

        var templateData = BuildTemplate(sample);
        var entityData = BuildEntity(sample);

        var service = new PdfReportDocumentGeneratorService(
            null!,
            NullLogger<PdfReportDocumentGeneratorService>.Instance,
            null!,
            Options.Create(new PdfBuilderOptions()));

        var bytes = await service.GeneratePdfForEntityDataAsync(templateData, entityData);
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        await File.WriteAllBytesAsync(outputPath, bytes);
        Console.WriteLine(outputPath);
    }

    private static ReportTemplateData BuildTemplate(QuotationTotalsRegressionSample sample)
    {
        return new ReportTemplateData
        {
            Page = new PageConfig
            {
                Width = sample.Page.Width,
                Height = sample.Page.Height,
                Unit = "px",
                PageCount = 1,
            },
            Elements = new List<ReportElement>
            {
                new()
                {
                    Id = "quotation-totals-block",
                    Type = "quotationTotals",
                    Section = "content",
                    X = sample.Block.X,
                    Y = sample.Block.Y,
                    Width = sample.Block.Width,
                    Height = sample.Block.Height,
                    ZIndex = 1,
                    FontSize = 13,
                    Text = sample.Title,
                    QuotationTotalsOptions = new QuotationTotalsOptions
                    {
                        Layout = sample.Options.Layout,
                        CurrencyMode = sample.Options.CurrencyMode,
                        CurrencyPath = "Currency",
                        GrossLabel = sample.Options.GrossLabel,
                        DiscountLabel = sample.Options.DiscountLabel,
                        NetLabel = sample.Options.NetLabel,
                        VatLabel = sample.Options.VatLabel,
                        GrandLabel = sample.Options.GrandLabel,
                        ShowGross = sample.Options.ShowGross,
                        ShowDiscount = sample.Options.ShowDiscount,
                        ShowVat = sample.Options.ShowVat,
                        EmphasizeGrandTotal = sample.Options.EmphasizeGrandTotal,
                        NoteTitle = sample.Note.Title,
                        NoteText = sample.Note.Text,
                        ShowNote = sample.Options.ShowNote,
                        HideEmptyNote = sample.Options.HideEmptyNote,
                    },
                    Style = new ElementStyle
                    {
                        Background = "#ffffff",
                        Border = "1px solid #cbd5e1",
                        Radius = 12,
                        Padding = 0,
                    },
                },
            },
        };
    }

    private static QuotationTotalsEntity BuildEntity(QuotationTotalsRegressionSample sample)
    {
        var values = sample.Values;
        return new QuotationTotalsEntity
        {
            Currency = values.Currency,
            GeneralDiscountAmount = values.Discount,
            Total = values.Net,
            GrandTotal = values.Grand,
            Description = sample.Note.Text,
        };
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
}

public sealed class QuotationTotalsRegressionSample
{
    public RegressionPage Page { get; set; } = new();
    public RegressionBlock Block { get; set; } = new();
    public string Title { get; set; } = "Teklif Toplamlari";
    public RegressionOptions Options { get; set; } = new();
    public RegressionValues Values { get; set; } = new();
    public RegressionNote Note { get; set; } = new();
}

public sealed class RegressionPage
{
    public decimal Width { get; set; }
    public decimal Height { get; set; }
}

public sealed class RegressionBlock
{
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
}

public sealed class RegressionOptions
{
    public string? Layout { get; set; }
    public string? CurrencyMode { get; set; }
    public string? GrossLabel { get; set; }
    public string? DiscountLabel { get; set; }
    public string? NetLabel { get; set; }
    public string? VatLabel { get; set; }
    public string? GrandLabel { get; set; }
    public bool ShowGross { get; set; } = true;
    public bool ShowDiscount { get; set; } = true;
    public bool ShowVat { get; set; } = true;
    public bool EmphasizeGrandTotal { get; set; } = true;
    public bool ShowNote { get; set; }
    public bool HideEmptyNote { get; set; } = true;
}

public sealed class RegressionValues
{
    public string? Currency { get; set; }
    public decimal Gross { get; set; }
    public decimal Discount { get; set; }
    public decimal Net { get; set; }
    public decimal Vat { get; set; }
    public decimal Grand { get; set; }
}

public sealed class RegressionNote
{
    public string? Title { get; set; }
    public string? Text { get; set; }
}

public sealed class QuotationTotalsEntity
{
    public string? Currency { get; set; }
    public decimal GeneralDiscountAmount { get; set; }
    public decimal Total { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Description { get; set; }
}
