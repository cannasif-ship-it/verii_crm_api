using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IDemandLineService
    {
        Task<ApiResponse<PagedResponse<DemandLineGetDto>>> GetAllDemandLinesAsync(PagedRequest request);
        Task<ApiResponse<DemandLineGetDto>> GetDemandLineByIdAsync(long id);
        Task<ApiResponse<DemandLineDto>> CreateDemandLineAsync(CreateDemandLineDto createDemandLineDto);
        Task<ApiResponse<DemandLineDto>> UpdateDemandLineAsync(long id, UpdateDemandLineDto updateDemandLineDto);
        Task<ApiResponse<object>> DeleteDemandLineAsync(long id);
        Task<ApiResponse<List<DemandLineGetDto>>> GetDemandLinesByDemandIdAsync(long demandId);
    }
}
