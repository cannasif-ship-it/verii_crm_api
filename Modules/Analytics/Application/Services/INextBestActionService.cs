using crm_api.Modules.Analytics.Application.Dtos;

namespace crm_api.Modules.Analytics.Application.Services
{
    public interface INextBestActionService
    {
        Task<List<RecommendedActionDto>> GetCustomerActionsAsync(long customerId, RevenueQualityDto revenueQuality);
        Task<List<RecommendedActionDto>> GetSalesmanActionsAsync(long userId, RevenueQualityDto revenueQuality);
    }
}
