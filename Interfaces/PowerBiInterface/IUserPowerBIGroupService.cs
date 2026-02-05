using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IUserPowerBIGroupService
    {
        Task<ApiResponse<PagedResponse<UserPowerBIGroupGetDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<UserPowerBIGroupGetDto>> GetByIdAsync(long id);
        Task<ApiResponse<UserPowerBIGroupGetDto>> CreateAsync(CreateUserPowerBIGroupDto dto);
        Task<ApiResponse<UserPowerBIGroupGetDto>> UpdateAsync(long id, UpdateUserPowerBIGroupDto dto);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
