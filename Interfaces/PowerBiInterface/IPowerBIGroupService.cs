using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IPowerBIGroupService
    {
        Task<ApiResponse<PagedResponse<PowerBIGroupGetDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<PowerBIGroupGetDto>> GetByIdAsync(long id);
        Task<ApiResponse<PowerBIGroupGetDto>> CreateAsync(CreatePowerBIGroupDto dto);
        Task<ApiResponse<PowerBIGroupGetDto>> UpdateAsync(long id, UpdatePowerBIGroupDto dto);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
