using System.Collections.Generic;
using crm_api.DTOs;

namespace crm_api.DTOs.CustomerDto
{
    public class Customer360OverviewDto
    {
        public Customer360ProfileDto Profile { get; set; } = null!;
        public Customer360KpiDto Kpis { get; set; } = null!;
        public List<Customer360SimpleItemDto> Contacts { get; set; } = new();
        public List<Customer360SimpleItemDto> ShippingAddresses { get; set; } = new();
        public List<Customer360SimpleItemDto> RecentDemands { get; set; } = new();
        public List<Customer360SimpleItemDto> RecentQuotations { get; set; } = new();
        public List<Customer360SimpleItemDto> RecentOrders { get; set; } = new();
        public List<Customer360SimpleItemDto> RecentActivities { get; set; } = new();
        public List<Customer360TimelineItemDto> Timeline { get; set; } = new();
        public RevenueQualityDto RevenueQuality { get; set; } = new();
        public List<RecommendedActionDto> RecommendedActions { get; set; } = new();
    }
}
