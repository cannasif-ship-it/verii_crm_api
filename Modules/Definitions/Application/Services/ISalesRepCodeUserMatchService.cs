using crm_api.Modules.Definitions.Application.Dtos;

namespace crm_api.Modules.Definitions.Application.Services
{
    public interface ISalesRepCodeUserMatchService
    {
        Task<ApiResponse<PagedResponse<SalesRepCodeUserMatchGetDto>>> GetAllSalesRepCodeUserMatchesAsync(PagedRequest request);
        Task<ApiResponse<SalesRepCodeUserMatchGetDto>> GetSalesRepCodeUserMatchByIdAsync(long id);
        Task<ApiResponse<SalesRepCodeUserMatchGetDto>> CreateSalesRepCodeUserMatchAsync(SalesRepCodeUserMatchCreateDto dto);
        Task<ApiResponse<SalesRepCodeUserMatchGetDto>> UpdateSalesRepCodeUserMatchAsync(long id, SalesRepCodeUserMatchUpdateDto dto);
        Task<ApiResponse<object>> DeleteSalesRepCodeUserMatchAsync(long id);
    }
}
