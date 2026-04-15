using crm_api.Modules.Identity.Domain.Entities;

namespace crm_api.Modules.Definitions.Domain.Entities
{
    public class SalesRepCodeUserMatch : BaseEntity
    {
        public long SalesRepCodeId { get; set; }
        public long UserId { get; set; }

        public virtual SalesRepCode SalesRepCode { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
