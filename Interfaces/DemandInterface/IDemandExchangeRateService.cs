using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IDemandExchangeRateService
    {
        Task<ApiResponse<PagedResponse<DemandExchangeRateGetDto>>> GetAllDemandExchangeRatesAsync(PagedRequest request);
        Task<ApiResponse<DemandExchangeRateGetDto>> GetDemandExchangeRateByIdAsync(long id);
        Task<ApiResponse<DemandExchangeRateGetDto>> CreateDemandExchangeRateAsync(DemandExchangeRateCreateDto createDto);
        Task<ApiResponse<DemandExchangeRateGetDto>> UpdateDemandExchangeRateAsync(long id, DemandExchangeRateUpdateDto updateDto);
        Task<ApiResponse<bool>> UpdateExchangeRateInDemand(List<DemandExchangeRateGetDto> updateDtos);
        Task<ApiResponse<object>> DeleteDemandExchangeRateAsync(long id);
        Task<ApiResponse<List<DemandExchangeRateGetDto>>> GetDemandExchangeRatesByDemandIdAsync(long demandId);
    }
}
