using crm_api.DTOs;

namespace crm_api.Interfaces
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
