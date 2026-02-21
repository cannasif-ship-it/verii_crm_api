using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace crm_api.DTOs
{
    public class CustomerCreateFromMobileDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? ContactName { get; set; }

        [MaxLength(100)]
        public string? ContactFirstName { get; set; }

        [MaxLength(100)]
        public string? ContactMiddleName { get; set; }

        [MaxLength(100)]
        public string? ContactLastName { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Phone2 { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? Website { get; set; }

        [MaxLength(250)]
        public string? Notes { get; set; }

        public long? CountryId { get; set; }
        public long? CityId { get; set; }
        public long? DistrictId { get; set; }
        public long? CustomerTypeId { get; set; }

        [MaxLength(50)]
        public string? SalesRepCode { get; set; }

        [MaxLength(50)]
        public string? GroupCode { get; set; }

        public decimal? CreditLimit { get; set; }
        public short? BranchCode { get; set; }
        public short? BusinessUnitCode { get; set; }

        public IFormFile? ImageFile { get; set; }

        [MaxLength(250)]
        public string? ImageDescription { get; set; }
    }

    public class CustomerCreateFromMobileResultDto
    {
        public long CustomerId { get; set; }
        public bool CustomerCreated { get; set; }
        public long? ContactId { get; set; }
        public bool ContactCreated { get; set; }
        public long? TitleId { get; set; }
        public bool TitleCreated { get; set; }
        public bool ImageUploaded { get; set; }
        public string? ImageUploadError { get; set; }
    }
}
