using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IStockDetailService
    {
        Task<ApiResponse<PagedResponse<StockDetailGetDto>>> GetAllStockDetailsAsync(PagedRequest request);
        Task<ApiResponse<StockDetailGetDto>> GetStockDetailByIdAsync(long id);
        Task<ApiResponse<StockDetailGetDto>> GetStockDetailByStockIdAsync(long stockId);
        Task<ApiResponse<StockDetailGetDto>> CreateStockDetailAsync(StockDetailCreateDto stockDetailCreateDto);
        Task<ApiResponse<StockDetailGetDto>> UpdateStockDetailAsync(long id, StockDetailUpdateDto stockDetailUpdateDto);
        Task<ApiResponse<object>> DeleteStockDetailAsync(long id);
    }
}
