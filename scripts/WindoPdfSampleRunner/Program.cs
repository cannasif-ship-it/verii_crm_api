using System.Text.Json;
using crm_api.Services;

namespace WindoPdfSampleRunner;

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
            : Path.Combine(root, "tmp", "candidate-windo-quotation.pdf");
        var planOutputPath = TryReadOption(args, "--plan-output");

        var json = await File.ReadAllTextAsync(samplePath);
        var sample = JsonSerializer.Deserialize<WindoQuotationSample>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        }) ?? throw new InvalidOperationException("Sample JSON could not be parsed.");

        sample.Lines = ExpandLines(sample.Lines, sample.RepeatFactor);

        var pdfBytes = WindoQuotationDocumentRenderer.GeneratePdf(sample);
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        await File.WriteAllBytesAsync(outputPath, pdfBytes);

        if (!string.IsNullOrWhiteSpace(planOutputPath))
        {
            var plan = WindoQuotationDocumentRenderer.DescribePagination(sample);
            Directory.CreateDirectory(Path.GetDirectoryName(planOutputPath)!);
            await File.WriteAllTextAsync(
                planOutputPath,
                JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }));
        }

        Console.WriteLine(outputPath);
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

    private static string? TryReadOption(string[] args, string optionName)
    {
        var index = Array.IndexOf(args, optionName);
        if (index < 0 || index + 1 >= args.Length)
            return null;

        return Path.GetFullPath(args[index + 1]);
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
