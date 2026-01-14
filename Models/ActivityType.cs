using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_ACTIVITY_TYPE")]
    public class ActivityType : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}

