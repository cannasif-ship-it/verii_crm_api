using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IPowerBIGroupReportDefinitionService
    {
        Task<ApiResponse<PagedResponse<PowerBIGroupReportDefinitionGetDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<PowerBIGroupReportDefinitionGetDto>> GetByIdAsync(long id);
        Task<ApiResponse<PowerBIGroupReportDefinitionGetDto>> CreateAsync(CreatePowerBIGroupReportDefinitionDto dto);
        Task<ApiResponse<PowerBIGroupReportDefinitionGetDto>> UpdateAsync(long id, UpdatePowerBIGroupReportDefinitionDto dto);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
