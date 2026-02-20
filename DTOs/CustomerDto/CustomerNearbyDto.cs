using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs
{
    public class CustomerNearbyQueryDto
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public double RadiusKm { get; set; } = 10;

        public bool IncludeShippingAddresses { get; set; } = true;

        /// <summary>
        /// JSON string for List&lt;Filter&gt;.
        /// Example: [{"column":"customerTypeName","operator":"Equals","value":"Potansiyel"}]
        /// </summary>
        public string? Filters { get; set; }

        /// <summary>
        /// "and" or "or". Defaults to and.
        /// </summary>
        public string FilterLogic { get; set; } = "and";
    }

    public class NearbyCustomerPinDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AddressDisplay { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Source { get; set; } = "main";
        public long? ShippingAddressId { get; set; }
        public string? CustomerTypeName { get; set; }
        public string? Phone { get; set; }
    }
}
