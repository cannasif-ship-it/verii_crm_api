using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface ISalesTypeService
    {
        Task<ApiResponse<PagedResponse<SalesTypeGetDto>>> GetAllSalesTypesAsync(PagedRequest request);
        Task<ApiResponse<SalesTypeGetDto>> GetSalesTypeByIdAsync(long id);
        Task<ApiResponse<SalesTypeGetDto>> CreateSalesTypeAsync(SalesTypeCreateDto createSalesTypeDto);
        Task<ApiResponse<SalesTypeGetDto>> UpdateSalesTypeAsync(long id, SalesTypeUpdateDto updateSalesTypeDto);
        Task<ApiResponse<object>> DeleteSalesTypeAsync(long id);
    }
}
