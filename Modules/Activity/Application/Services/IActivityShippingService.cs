namespace crm_api.Modules.Activity.Application.Services
{
    public interface IActivityShippingService
    {
        Task<ApiResponse<PagedResponse<ActivityShippingGetDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<ActivityShippingGetDto>> GetByIdAsync(long id);
        Task<ApiResponse<ActivityShippingGetDto>> CreateAsync(ActivityShippingCreateDto dto);
        Task<ApiResponse<ActivityShippingGetDto>> UpdateAsync(long id, ActivityShippingUpdateDto dto);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
