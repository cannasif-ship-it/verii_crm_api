using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
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
