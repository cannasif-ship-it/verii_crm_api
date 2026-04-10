namespace crm_api.Modules.Identity.Application.Services
{
    public interface IUserContextService
    {
        long? GetCurrentUserId();
        string? GetCurrentRole();
        Guid? GetCurrentTenantId();
        Guid ResolveTenantIdOrThrow();
    }
}
