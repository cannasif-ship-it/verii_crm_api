using cms_webapi.DTOs;
using cms_webapi.Models;

namespace cms_webapi.Interfaces
{
    public interface IQuotationService
    {
        Task<ApiResponse<PagedResponse<QuotationGetDto>>> GetAllQuotationsAsync(PagedRequest request);
        Task<ApiResponse<QuotationGetDto>> GetQuotationByIdAsync(long id);
        Task<ApiResponse<QuotationDto>> CreateQuotationAsync(CreateQuotationDto createQuotationDto);
        Task<ApiResponse<QuotationDto>> UpdateQuotationAsync(long id, UpdateQuotationDto updateQuotationDto);
        Task<ApiResponse<object>> DeleteQuotationAsync(long id);
        Task<ApiResponse<List<QuotationGetDto>>> GetQuotationsByPotentialCustomerIdAsync(long potentialCustomerId);
        Task<ApiResponse<List<QuotationGetDto>>> GetQuotationsByRepresentativeIdAsync(long representativeId);
        Task<ApiResponse<List<QuotationGetDto>>> GetQuotationsByStatusAsync(int status);
        Task<ApiResponse<bool>> QuotationExistsAsync(long id);
        Task<ApiResponse<CreateBulkQuotationResultDto>> CreateBulkQuotationAsync(CreateBulkQuotationDto dto);
        Task<ApiResponse<QuotationGetDto>> CreateQuotationBulkAsync(QuotationBulkCreateDto bulkDto);
        Task<ApiResponse<List<PriceOfProductDto>>> GetPriceOfProductAsync(List<PriceOfProductRequestDto> request);
    }
}
