using System.Text.Json;

namespace crm_api.Modules.PdfBuilder.Infrastructure.Helpers
{
    /// <summary>
    /// Central JSON serializer options for PDF report template data (camelCase, consistent with frontend).
    /// </summary>
    public static class PdfReportTemplateJsonOptions
    {
        public static readonly JsonSerializerOptions CamelCase = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }
}
