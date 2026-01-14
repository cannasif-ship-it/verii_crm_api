using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface IDistrictService
    {
        Task<ApiResponse<PagedResponse<DistrictGetDto>>> GetAllDistrictsAsync(PagedRequest request);
        Task<ApiResponse<DistrictGetDto>> GetDistrictByIdAsync(long id);
        Task<ApiResponse<DistrictGetDto>> CreateDistrictAsync(DistrictCreateDto districtCreateDto);
        Task<ApiResponse<DistrictGetDto>> UpdateDistrictAsync(long id, DistrictUpdateDto districtUpdateDto);
        Task<ApiResponse<object>> DeleteDistrictAsync(long id);
    }
}
