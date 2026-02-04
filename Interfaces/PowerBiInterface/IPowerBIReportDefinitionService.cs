using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.DTOs.PowerBi;

namespace crm_api.Interfaces
{
    public interface IPowerBIReportDefinitionService
    {
        Task<ApiResponse<PagedResponse<PowerBIReportDefinitionGetDto>>> GetAllAsync(PagedRequest request);

        Task<ApiResponse<PowerBIReportDefinitionGetDto>> GetByIdAsync(long id);

        Task<ApiResponse<PowerBIReportDefinitionGetDto>> CreateAsync(CreatePowerBIReportDefinitionDto dto);

        Task<ApiResponse<PowerBIReportDefinitionGetDto>> UpdateAsync(long id, UpdatePowerBIReportDefinitionDto dto);

        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
