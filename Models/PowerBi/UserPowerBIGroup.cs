namespace crm_api.Models.PowerBi
{
    public class UserPowerBIGroup : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }

        public long GroupId { get; set; }
        public PowerBIGroup? Group { get; set; }
    }
}
