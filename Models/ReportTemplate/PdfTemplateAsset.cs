using System;

namespace crm_api.Models
{
    /// <summary>
    /// Uploaded media file used by PDF report templates.
    /// </summary>
    public class PdfTemplateAsset : BaseEntity
    {
        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string RelativeUrl { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long SizeBytes { get; set; }
    }
}
