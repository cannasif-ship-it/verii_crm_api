namespace crm_api.DTOs
{
    public class CustomerImageDto : BaseEntityDto
    {
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? ImageDescription { get; set; }
    }
}
