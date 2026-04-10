namespace crm_api.Modules.Activity.Application.Services
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
