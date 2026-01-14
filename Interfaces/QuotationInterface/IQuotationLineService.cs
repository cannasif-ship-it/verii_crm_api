using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface IQuotationLineService
    {
        Task<ApiResponse<PagedResponse<QuotationLineGetDto>>> GetAllQuotationLinesAsync(PagedRequest request);
        Task<ApiResponse<QuotationLineGetDto>> GetQuotationLineByIdAsync(long id);
        Task<ApiResponse<QuotationLineDto>> CreateQuotationLineAsync(CreateQuotationLineDto createQuotationLineDto);
        Task<ApiResponse<QuotationLineDto>> UpdateQuotationLineAsync(long id, UpdateQuotationLineDto updateQuotationLineDto);
        Task<ApiResponse<object>> DeleteQuotationLineAsync(long id);
        Task<ApiResponse<List<QuotationLineGetDto>>> GetQuotationLinesByQuotationIdAsync(long quotationId);
    }
}