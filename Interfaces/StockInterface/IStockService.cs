using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IStockService
    {
        Task<ApiResponse<PagedResponse<StockGetDto>>> GetAllStocksAsync(PagedRequest request);
        Task<ApiResponse<PagedResponse<StockGetWithMainImageDto>>> GetAllStocksWithImagesAsync(PagedRequest request);
        Task<ApiResponse<StockGetDto>> GetStockByIdAsync(long id);
        Task<ApiResponse<StockGetDto>> CreateStockAsync(StockCreateDto stockCreateDto);
        Task<ApiResponse<StockGetDto>> UpdateStockAsync(long id, StockUpdateDto stockUpdateDto);
        Task<ApiResponse<object>> DeleteStockAsync(long id);
        Task SyncStocksFromErpAsync();
    }
}
