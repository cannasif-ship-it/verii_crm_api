using crm_api.Modules.Analytics.Application.Dtos;

namespace crm_api.Modules.Analytics.Application.Services
{
    public interface IRevenueQualityService
    {
        Task<RevenueQualityDto> CalculateCustomerRevenueQualityAsync(long customerId);
        Task<RevenueQualityDto> CalculateSalesmanRevenueQualityAsync(long userId);
    }
}
