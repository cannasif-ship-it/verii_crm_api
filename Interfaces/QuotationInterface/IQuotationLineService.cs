using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IQuotationLineService
    {
        Task<ApiResponse<PagedResponse<QuotationLineGetDto>>> GetAllQuotationLinesAsync(PagedRequest request);
        Task<ApiResponse<QuotationLineGetDto>> GetQuotationLineByIdAsync(long id);
        Task<ApiResponse<QuotationLineDto>> CreateQuotationLineAsync(CreateQuotationLineDto createQuotationLineDto);
        Task<ApiResponse<List<QuotationLineDto>>> CreateQuotationLinesAsync(List<CreateQuotationLineDto> createQuotationLineDtos);
        Task<ApiResponse<List<QuotationLineDto>>> UpdateQuotationLinesAsync(List<QuotationLineDto> quotationLineDtos);
        Task<ApiResponse<QuotationLineDto>> UpdateQuotationLineAsync(long id, UpdateQuotationLineDto updateQuotationLineDto);
        Task<ApiResponse<object>> DeleteQuotationLineAsync(long id);
        Task<ApiResponse<List<QuotationLineGetDto>>> GetQuotationLinesByQuotationIdAsync(long quotationId);
    }
}
