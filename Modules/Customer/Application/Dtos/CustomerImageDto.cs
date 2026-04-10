namespace crm_api.Modules.Customer.Application.Dtos
{
    public class CustomerImageDto : BaseEntityDto
    {
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? ImageDescription { get; set; }
    }
}
