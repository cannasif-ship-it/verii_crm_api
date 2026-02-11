using System;

namespace crm_api.Models
{
    public class SalesTypeDefinition : BaseEntity
    {
        public string SalesType { get; set; } = string.Empty; // YURTICI | YURTDISI
        public string Name { get; set; } = string.Empty;
    }
}
