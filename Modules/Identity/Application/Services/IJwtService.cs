
namespace crm_api.Modules.Identity.Application.Services
{
    public interface IJwtService
    {
        ApiResponse<string> GenerateToken(User user, Guid sessionId, DateTime issuedAtUtc);
    }
}
