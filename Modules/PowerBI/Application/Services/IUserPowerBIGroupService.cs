using crm_api.Helpers;
using crm_api.Modules.PowerBI.Application.Dtos;

namespace crm_api.Modules.PowerBI.Application.Services
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
