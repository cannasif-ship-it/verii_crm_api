using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IStockRelationService
    {
        Task<ApiResponse<StockRelationDto>> CreateAsync(StockRelationCreateDto relationDto);
        Task<ApiResponse<List<StockRelationDto>>> GetByStockIdAsync(long stockId);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
