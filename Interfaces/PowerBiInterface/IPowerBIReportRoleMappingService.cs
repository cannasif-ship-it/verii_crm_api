using crm_api.DTOs;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IPowerBIReportRoleMappingService
    {
        Task<ApiResponse<PagedResponse<PowerBIReportRoleMappingGetDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<PowerBIReportRoleMappingGetDto>> GetByIdAsync(long id);
        Task<ApiResponse<PowerBIReportRoleMappingGetDto>> CreateAsync(CreatePowerBIReportRoleMappingDto dto);
        Task<ApiResponse<PowerBIReportRoleMappingGetDto>> UpdateAsync(long id, UpdatePowerBIReportRoleMappingDto dto);
        Task<ApiResponse<bool>> DeleteAsync(long id);
    }
}
