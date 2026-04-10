namespace crm_api.Modules.PowerBI.Domain.Entities
{
    public class UserPowerBIGroup : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }

        public long GroupId { get; set; }
        public PowerBIGroup? Group { get; set; }
    }
}
