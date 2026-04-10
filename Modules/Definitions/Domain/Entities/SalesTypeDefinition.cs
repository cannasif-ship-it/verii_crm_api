using System;

namespace crm_api.Modules.Definitions.Domain.Entities
{
    public class SalesTypeDefinition : BaseEntity
    {
        public string SalesType { get; set; } = string.Empty; // YURTICI | YURTDISI
        public string Name { get; set; } = string.Empty;
    }
}
