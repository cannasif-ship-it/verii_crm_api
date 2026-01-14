using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface IStockImageService
    {
        Task<ApiResponse<List<StockImageDto>>> AddImagesAsync(List<StockImageCreateDto> imageDtos);
        Task<ApiResponse<List<StockImageDto>>> UploadImagesAsync(long stockId, List<IFormFile> files, List<string>? altTexts = null);
        Task<ApiResponse<List<StockImageDto>>> GetByStockIdAsync(long stockId);
        Task<ApiResponse<object>> DeleteAsync(long id);
        Task<ApiResponse<StockImageDto>> SetPrimaryImageAsync(long imageId);
    }
}
