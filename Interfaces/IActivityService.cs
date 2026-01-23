using depoWebAPI.Models;
using crm_api.DTOs;
using crm_api.Data;

namespace crm_api.Interfaces
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
