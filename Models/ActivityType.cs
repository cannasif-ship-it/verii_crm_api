using System;
using System.Collections.Generic;
namespace crm_api.Models
{
    public class ActivityType : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}