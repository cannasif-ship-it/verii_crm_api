using depoWebAPI.Models;
using cms_webapi.DTOs;
using cms_webapi.Data;

namespace cms_webapi.Interfaces
{
    public interface IActivityService
    {
        Task<ApiResponse<PagedResponse<ActivityDto>>> GetAllActivitiesAsync(PagedRequest request);
        Task<ApiResponse<ActivityDto>> GetActivityByIdAsync(long id);
        Task<ApiResponse<ActivityDto>> CreateActivityAsync(CreateActivityDto createActivityDto);
        Task<ApiResponse<ActivityDto>> UpdateActivityAsync(long id, UpdateActivityDto updateActivityDto);
        Task<ApiResponse<object>> DeleteActivityAsync(long id);
    }
}
