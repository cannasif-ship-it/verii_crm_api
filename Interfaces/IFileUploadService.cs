using crm_api.DTOs;
using Microsoft.AspNetCore.Http;

namespace crm_api.Interfaces
{
    public interface IFileUploadService
    {
        Task<ApiResponse<string>> UploadProfilePictureAsync(IFormFile file, long userId);
        Task<ApiResponse<bool>> DeleteProfilePictureAsync(string fileUrl);
        string GetProfilePictureUrl(string fileName, long userId);
        Task<ApiResponse<string>> UploadStockImageAsync(IFormFile file, long stockId);
        Task<ApiResponse<bool>> DeleteStockImageAsync(string fileUrl);
        string GetStockImageUrl(string fileName, long stockId);
    }
}
