using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_STOCK_RELATION")]
    public class StockRelation : BaseEntity
    {

    // Stock ile ilişkilendirme
    public long StockId { get; set; }
    [ForeignKey("StockId")]
    public Stock Stock { get; set; } = null!;

   // Bağlı ürün (ana ürüne bağlı olarak kullanılır)
    public long RelatedStockId { get; set; }
    [ForeignKey("RelatedStockId")]
    public Stock RelatedStock { get; set; } = null!;

    // Bağlı ürün sayısı (örneğin 10 küçük tornavida)
    public decimal Quantity { get; set; }

    // Ekstra açıklama
    public string? Description { get; set; }

    // Zorunlu mu? (bazı ürünler opsiyonel olabilir)
    public bool IsMandatory { get; set; } = true;
    }
}
