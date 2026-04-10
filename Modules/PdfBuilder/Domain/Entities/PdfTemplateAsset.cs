using System;
using System.Collections.Generic;

namespace crm_api.Modules.PdfBuilder.Domain.Entities
{
    /// <summary>
    /// Uploaded image file used by PDF/quotation/report designers.
    /// </summary>
    public class PdfTemplateAsset : BaseEntity
    {
        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string RelativeUrl { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long SizeBytes { get; set; }

        public ICollection<PdfImageUsage> Usages { get; set; } = new List<PdfImageUsage>();
    }
}
