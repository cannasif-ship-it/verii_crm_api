
namespace crm_api.Modules.Integrations.Application.Services
{
    public interface IGoogleIntegrationLogService
    {
        Task WriteAsync(GoogleIntegrationLogWriteDto dto, CancellationToken cancellationToken = default);
        Task<PagedResponse<GoogleIntegrationLogDto>> GetPagedAsync(
            Guid tenantId,
            long? userId,
            GoogleIntegrationLogsQueryDto query,
            CancellationToken cancellationToken = default);
    }
}
