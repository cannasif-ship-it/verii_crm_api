using crm_api.DTOs;
using Microsoft.AspNetCore.Http;

namespace crm_api.Interfaces
{
    public interface IActivityImageService
    {
        Task<ApiResponse<List<ActivityImageDto>>> AddImagesAsync(List<CreateActivityImageDto> request);
        Task<ApiResponse<List<ActivityImageDto>>> UploadImagesAsync(long activityId, List<IFormFile> files, List<string>? resimAciklamalar = null);
        Task<ApiResponse<List<ActivityImageDto>>> GetByActivityIdAsync(long activityId);
        Task<ApiResponse<ActivityImageDto>> UpdateAsync(long id, UpdateActivityImageDto request);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
