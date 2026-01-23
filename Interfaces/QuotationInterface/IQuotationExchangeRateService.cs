using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IQuotationExchangeRateService
    {
        Task<ApiResponse<PagedResponse<QuotationExchangeRateGetDto>>> GetAllQuotationExchangeRatesAsync(PagedRequest request);
        Task<ApiResponse<QuotationExchangeRateGetDto>> GetQuotationExchangeRateByIdAsync(long id);
        Task<ApiResponse<QuotationExchangeRateGetDto>> CreateQuotationExchangeRateAsync(QuotationExchangeRateCreateDto createDto);
        Task<ApiResponse<QuotationExchangeRateGetDto>> UpdateQuotationExchangeRateAsync(long id, QuotationExchangeRateUpdateDto updateDto);
        Task<ApiResponse<object>> DeleteQuotationExchangeRateAsync(long id);
        Task<ApiResponse<List<QuotationExchangeRateGetDto>>> GetQuotationExchangeRatesByQuotationIdAsync(long quotationId);
    }
}
