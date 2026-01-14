using System;

namespace cms_webapi.Models
{
    /// <summary>
    /// Fiyat kuralının hangi iş akışında geçerli olacağını belirtir
    /// </summary>
    public enum PricingRuleType
    {
        /// <summary>
        /// Talep aşamasında geçerli
        /// </summary>
        Demand = 1,

        /// <summary>
        /// Teklif aşamasında geçerli
        /// </summary>
        Quotation = 2,

        /// <summary>
        /// Sipariş aşamasında geçerli
        /// </summary>
        Order = 3
    }
}
