using crm_api.Modules.PowerBI.Application.Dtos;

namespace crm_api.Modules.PowerBI.Application.Services
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
