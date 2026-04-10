using crm_api.Helpers;
using crm_api.Modules.PowerBI.Application.Dtos;

namespace crm_api.Modules.PowerBI.Application.Services
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
