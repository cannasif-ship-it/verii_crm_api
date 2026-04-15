using crm_api.Modules.Definitions.Application.Dtos;

namespace crm_api.Modules.Definitions.Application.Services
{
    public interface ISalesRepCodeService
    {
        Task<ApiResponse<PagedResponse<SalesRepCodeGetDto>>> GetAllSalesRepCodesAsync(PagedRequest request);
        Task<ApiResponse<SalesRepCodeGetDto>> GetSalesRepCodeByIdAsync(long id);
        Task<ApiResponse<SalesRepCodeGetDto>> CreateSalesRepCodeAsync(SalesRepCodeCreateDto dto);
        Task<ApiResponse<SalesRepCodeGetDto>> UpdateSalesRepCodeAsync(long id, SalesRepCodeUpdateDto dto);
        Task<ApiResponse<object>> DeleteSalesRepCodeAsync(long id);
        Task<ApiResponse<SalesRepCodeSyncResponseDto>> SyncSalesRepCodesFromErpAsync();
    }
}
