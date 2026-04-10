namespace crm_api.Modules.Identity.Application.Services
{
    public interface IUserSessionCacheService
    {
        string GetCacheKey(Guid sessionId);
        void SetActiveSession(Guid sessionId, long userId, DateTime? absoluteExpirationUtc = null);
        void RemoveSession(Guid sessionId);
        Task<bool> RestoreSessionAsync(Guid sessionId, long userId, CancellationToken cancellationToken = default);
    }
}
