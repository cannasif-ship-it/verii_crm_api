using ActivityEntity = crm_api.Modules.Activity.Domain.Entities.Activity;

namespace crm_api.Modules.Integrations.Application.Services
{
    public interface IGoogleCalendarService
    {
        Task<string> CreateTestEventAsync(long userId, CancellationToken cancellationToken = default);
        Task<string> CreateActivityEventAsync(long userId, ActivityEntity activity, CancellationToken cancellationToken = default);
        Task<string> SyncActivityEventAsync(long userId, ActivityEntity activity, CancellationToken cancellationToken = default);
        Task DeleteActivityEventAsync(long userId, string calendarEventId, CancellationToken cancellationToken = default);
    }
}
