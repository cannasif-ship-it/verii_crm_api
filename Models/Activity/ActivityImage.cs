using System;

namespace crm_api.Models
{
    public class ActivityImage : BaseEntity
    {
        public long ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        public string? ResimAciklama { get; set; }
        public string ResimUrl { get; set; } = string.Empty;
    }
}
