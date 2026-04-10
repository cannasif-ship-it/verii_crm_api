using System;

namespace crm_api.Modules.Activity.Domain.Entities
{
    public class ActivityImage : BaseEntity
    {
        public long ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        public string? ResimAciklama { get; set; }
        public string ResimUrl { get; set; } = string.Empty;
    }
}
