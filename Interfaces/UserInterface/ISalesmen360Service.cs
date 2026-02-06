using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface ISalesmen360Service
    {
        Task<ApiResponse<Salesmen360OverviewDto>> GetOverviewAsync(long userId, string? currency = null);
        Task<ApiResponse<Salesmen360AnalyticsSummaryDto>> GetAnalyticsSummaryAsync(long userId, string? currency = null);
        Task<ApiResponse<Salesmen360AnalyticsChartsDto>> GetAnalyticsChartsAsync(long userId, int months = 12, string? currency = null);
    }
}
