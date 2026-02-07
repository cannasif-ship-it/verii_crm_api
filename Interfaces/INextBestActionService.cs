using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface INextBestActionService
    {
        Task<List<RecommendedActionDto>> GetCustomerActionsAsync(long customerId, RevenueQualityDto revenueQuality);
        Task<List<RecommendedActionDto>> GetSalesmanActionsAsync(long userId, RevenueQualityDto revenueQuality);
    }
}
