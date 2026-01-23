using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IUserDiscountLimitService
    {
        Task<ApiResponse<PagedResponse<UserDiscountLimitDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<UserDiscountLimitDto>> GetByIdAsync(long id);
        Task<ApiResponse<List<UserDiscountLimitDto>>> GetBySalespersonIdAsync(long salespersonId);
        Task<ApiResponse<List<UserDiscountLimitDto>>> GetByErpProductGroupCodeAsync(string erpProductGroupCode);
        Task<ApiResponse<UserDiscountLimitDto>> GetBySalespersonAndGroupAsync(long salespersonId, string erpProductGroupCode);
        Task<ApiResponse<UserDiscountLimitDto>> CreateAsync(CreateUserDiscountLimitDto createDto);
        Task<ApiResponse<UserDiscountLimitDto>> UpdateAsync(long id, UpdateUserDiscountLimitDto updateDto);
        Task<ApiResponse<object>> DeleteAsync(long id);
        Task<ApiResponse<bool>> ExistsAsync(long id);
        Task<ApiResponse<bool>> ExistsBySalespersonAndGroupAsync(long salespersonId, string erpProductGroupCode);
    }
}
