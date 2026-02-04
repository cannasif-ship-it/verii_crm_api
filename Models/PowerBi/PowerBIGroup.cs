namespace crm_api.Models.PowerBi
{
    /// <summary>
    /// User -> Group üyeliği
    /// </summary>
    public class UserPowerBIGroup : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }

        public long GroupId { get; set; }
        public PowerBIGroup? Group { get; set; }
    }
}
