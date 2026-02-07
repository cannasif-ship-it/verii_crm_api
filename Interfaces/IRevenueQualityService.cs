using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IRevenueQualityService
    {
        Task<RevenueQualityDto> CalculateCustomerRevenueQualityAsync(long customerId);
        Task<RevenueQualityDto> CalculateSalesmanRevenueQualityAsync(long userId);
    }
}
