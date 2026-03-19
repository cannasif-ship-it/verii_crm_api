using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Infrastructure;
using crm_api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace VisualBrochureRunner;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length < 4)
            throw new InvalidOperationException("Usage: <output-pdf> <image-1> <image-2> <image-3>");

        var outputPath = Path.GetFullPath(args[0]);
        var imagePaths = args.Skip(1).Take(3).Select(Path.GetFullPath).ToList();

        var service = new PdfReportDocumentGeneratorService(
            null!,
            NullLogger<PdfReportDocumentGeneratorService>.Instance,
            null!,
            Options.Create(new PdfBuilderOptions()));

        var bytes = await service.GeneratePdfForEntityDataAsync(BuildTemplate(imagePaths), new { });
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        await File.WriteAllBytesAsync(outputPath, bytes);
        Console.WriteLine(outputPath);
    }

    private static ReportTemplateData BuildTemplate(IReadOnlyList<string> imagePaths)
    {
        var elements = new List<ReportElement>();
        for (var index = 0; index < imagePaths.Count; index++)
        {
            elements.Add(new ReportElement
            {
                Id = $"brochure-image-{index + 1}",
                Type = "image",
                Section = "page",
                X = 0,
                Y = 0,
                Width = 210,
                Height = 297,
                PageNumbers = new List<int> { index + 1 },
                Value = ToDataUri(imagePaths[index]),
                Style = new ElementStyle
                {
                    ImageFit = "cover",
                },
            });
        }

        return new ReportTemplateData
        {
            Page = new PageConfig
            {
                Width = 210,
                Height = 297,
                Unit = "mm",
                PageCount = imagePaths.Count,
            },
            Elements = elements,
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
            _ => "application/octet-stream",
        };

        return $"data:{mime};base64,{Convert.ToBase64String(bytes)}";
    }
}
