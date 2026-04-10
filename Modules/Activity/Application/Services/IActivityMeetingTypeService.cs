namespace crm_api.Modules.Activity.Application.Services
{
    public interface IActivityMeetingTypeService
    {
        Task<ApiResponse<PagedResponse<ActivityMeetingTypeGetDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<ActivityMeetingTypeGetDto>> GetByIdAsync(long id);
        Task<ApiResponse<ActivityMeetingTypeGetDto>> CreateAsync(ActivityMeetingTypeCreateDto dto);
        Task<ApiResponse<ActivityMeetingTypeGetDto>> UpdateAsync(long id, ActivityMeetingTypeUpdateDto dto);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
