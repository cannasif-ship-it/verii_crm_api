using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models
{
    [Table("RII_STOCK_IMAGE")]
    public class StockImage : BaseEntity
    {

        // Stock ile ilişkilendirme
        public long StockId { get; set; }
        [ForeignKey("StockId")]
        public Stock Stock { get; set; } = null!;

        // Görsel dosya yolu
        public string FilePath { get; set; } = null!;

        // Görselin açıklaması (alt text)
        public string? AltText { get; set; }

        // Görselin sırası
        public int SortOrder { get; set; }

        // Bu görsel ana görsel mi?
        public bool IsPrimary { get; set; }
    }
}

