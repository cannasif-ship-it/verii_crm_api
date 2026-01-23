using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IActivityTypeService
    {
        Task<ApiResponse<PagedResponse<ActivityTypeGetDto>>> GetAllActivityTypesAsync(PagedRequest request);
        Task<ApiResponse<ActivityTypeGetDto>> GetActivityTypeByIdAsync(long id);
        Task<ApiResponse<ActivityTypeGetDto>> CreateActivityTypeAsync(ActivityTypeCreateDto createActivityTypeDto);
        Task<ApiResponse<ActivityTypeGetDto>> UpdateActivityTypeAsync(long id, ActivityTypeUpdateDto updateActivityTypeDto);
        Task<ApiResponse<object>> DeleteActivityTypeAsync(long id);
    }
}
