using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface ICityService
    {
        Task<ApiResponse<PagedResponse<CityGetDto>>> GetAllCitiesAsync(PagedRequest request);
        Task<ApiResponse<CityGetDto>> GetCityByIdAsync(long id);
        Task<ApiResponse<CityGetDto>> CreateCityAsync(CityCreateDto cityCreateDto);
        Task<ApiResponse<CityGetDto>> UpdateCityAsync(long id, CityUpdateDto cityUpdateDto);
        Task<ApiResponse<object>> DeleteCityAsync(long id);
    }
}
