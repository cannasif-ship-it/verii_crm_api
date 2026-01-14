using cms_webapi.Models;
using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface IJwtService
    {
        ApiResponse<string> GenerateToken(User user);
    }
}