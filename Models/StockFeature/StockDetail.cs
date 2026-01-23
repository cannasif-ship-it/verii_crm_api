using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace crm_api.Models
{
    [Table("RII_STOCK_DETAIL")]
    public class StockDetail : BaseEntity
    {

        // Stock ile ilişkilendirme
        public long StockId { get; set; }
        [ForeignKey("StockId")]
        public Stock Stock { get; set; } = null!;

        // HTML açıklama (kullanıcı tarafından girilir)
        [Column(TypeName = "nvarchar(max)")]
     public string HtmlDescription { get; set; } = null!;

    // Teknik özellikler (JSON veya Text formatında)
    [Column(TypeName = "nvarchar(max)")]
    public string? TechnicalSpecsJson { get; set; }
    }
}

