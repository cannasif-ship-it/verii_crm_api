using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface ITempQuotattionService
    {
        Task<ApiResponse<PagedResponse<TempQuotattionGetDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<TempQuotattionGetDto>> GetByIdAsync(long id);
        Task<ApiResponse<TempQuotattionGetDto>> CreateAsync(TempQuotattionCreateDto dto);
        Task<ApiResponse<TempQuotattionGetDto>> UpdateAsync(long id, TempQuotattionUpdateDto dto);
        Task<ApiResponse<TempQuotattionGetDto>> CreateRevisionAsync(long id);
        Task<ApiResponse<long>> ConvertToQuotationAsync(long id);
        Task<ApiResponse<TempQuotattionGetDto>> SetApprovedAsync(long id);
        Task<ApiResponse<object>> DeleteAsync(long id);

        Task<ApiResponse<List<TempQuotattionLineGetDto>>> GetLinesByHeaderIdAsync(long tempQuotattionId);
        Task<ApiResponse<TempQuotattionLineGetDto>> GetLineByIdAsync(long lineId);
        Task<ApiResponse<TempQuotattionLineGetDto>> CreateLineAsync(TempQuotattionLineCreateDto dto);
        Task<ApiResponse<List<TempQuotattionLineGetDto>>> CreateLinesAsync(List<TempQuotattionLineCreateDto> dtos);
        Task<ApiResponse<TempQuotattionLineGetDto>> UpdateLineAsync(long lineId, TempQuotattionLineUpdateDto dto);
        Task<ApiResponse<object>> DeleteLineAsync(long lineId);

        Task<ApiResponse<List<TempQuotattionExchangeLineGetDto>>> GetExchangeLinesByHeaderIdAsync(long tempQuotattionId);
        Task<ApiResponse<TempQuotattionExchangeLineGetDto>> GetExchangeLineByIdAsync(long exchangeLineId);
        Task<ApiResponse<TempQuotattionExchangeLineGetDto>> CreateExchangeLineAsync(TempQuotattionExchangeLineCreateDto dto);
        Task<ApiResponse<TempQuotattionExchangeLineGetDto>> UpdateExchangeLineAsync(long exchangeLineId, TempQuotattionExchangeLineUpdateDto dto);
        Task<ApiResponse<object>> DeleteExchangeLineAsync(long exchangeLineId);
    }
}
