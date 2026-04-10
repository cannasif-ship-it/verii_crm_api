using Microsoft.AspNetCore.Http;

namespace crm_api.Shared.Infrastructure.Abstractions
{
    public interface IFileUploadService
    {
        Task<ApiResponse<string>> UploadProfilePictureAsync(IFormFile file, long userId);
        Task<ApiResponse<bool>> DeleteProfilePictureAsync(string fileUrl);
        string GetProfilePictureUrl(string fileName, long userId);

        Task<ApiResponse<string>> UploadStockImageAsync(IFormFile file, long stockId);
        Task<ApiResponse<bool>> DeleteStockImageAsync(string fileUrl);
        string GetStockImageUrl(string fileName, long stockId);

        Task<ApiResponse<string>> UploadActivityImageAsync(IFormFile file, long activityId);
        Task<ApiResponse<bool>> DeleteActivityImageAsync(string fileUrl);
        string GetActivityImageUrl(string fileName, long activityId);

        Task<ApiResponse<string>> UploadCustomerImageAsync(IFormFile file, long customerId);
        Task<ApiResponse<bool>> DeleteCustomerImageAsync(string fileUrl);
        string GetCustomerImageUrl(string fileName, long customerId);

        Task<ApiResponse<string>> UploadCategoryImageAsync(IFormFile file);
        Task<ApiResponse<bool>> DeleteCategoryImageAsync(string fileUrl);
        string GetCategoryImageUrl(string fileName);
    }
}
