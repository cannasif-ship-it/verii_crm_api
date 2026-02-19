namespace crm_api.Models
{
    public class CustomerImage : BaseEntity
    {
        public long CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string ImageUrl { get; set; } = string.Empty;
        public string? ImageDescription { get; set; }
    }
}
