using System.Collections.Generic;

namespace cms_webapi.DTOs
{
    public class Filter
    {
        public string Column { get; set; } = string.Empty;
        public string Operator { get; set; } = "Equals";
        public string Value { get; set; } = string.Empty;
    }

    public class PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } = "Id";
        public string? SortDirection { get; set; } = "desc";
        public List<Filter>? Filters { get; set; } = new();
    }
}
