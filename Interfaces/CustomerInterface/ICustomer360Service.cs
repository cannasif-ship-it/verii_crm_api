using crm_api.DTOs;
using crm_api.DTOs.CustomerDto;

namespace crm_api.Interfaces
{
    public interface ICustomer360Service
    {
        Task<ApiResponse<Customer360OverviewDto>> GetOverviewAsync(long customerId);
        Task<ApiResponse<Customer360AnalyticsSummaryDto>> GetAnalyticsSummaryAsync(long customerId);
        Task<ApiResponse<Customer360AnalyticsChartsDto>> GetAnalyticsChartsAsync(long customerId, int months = 12);
    }
}
