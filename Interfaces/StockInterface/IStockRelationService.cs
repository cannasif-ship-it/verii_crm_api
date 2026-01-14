using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface IStockRelationService
    {
        Task<ApiResponse<StockRelationDto>> CreateAsync(StockRelationCreateDto relationDto);
        Task<ApiResponse<List<StockRelationDto>>> GetByStockIdAsync(long stockId);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
