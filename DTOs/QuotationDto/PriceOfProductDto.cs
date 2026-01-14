using System;
using System.ComponentModel.DataAnnotations;

namespace cms_webapi.DTOs
{

    public class PriceOfProductRequestDto
    {
        public string ProductCode { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
    }
    public class PriceOfProductDto
    {
        public string ProductCode { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal ListPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal? Discount1 { get; set; }
        public decimal? Discount2 { get; set; }
        public decimal? Discount3 { get; set; }
    }


}
