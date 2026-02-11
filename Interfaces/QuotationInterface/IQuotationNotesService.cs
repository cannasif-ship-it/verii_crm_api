using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IQuotationNotesService
    {
        Task<ApiResponse<PagedResponse<QuotationNotesGetDto>>> GetAllQuotationNotesAsync(PagedRequest request);
        Task<ApiResponse<QuotationNotesGetDto>> GetQuotationNotesByIdAsync(long id);
        Task<ApiResponse<QuotationNotesGetDto>> GetNotesByQuotationIdAsync(long quotationId);
        Task<ApiResponse<QuotationNotesGetDto>> GetByQuotationIdAsync(long quotationId);
        Task<ApiResponse<QuotationNotesDto>> CreateQuotationNotesAsync(CreateQuotationNotesDto createQuotationNotesDto);
        Task<ApiResponse<QuotationNotesDto>> UpdateQuotationNotesAsync(long id, UpdateQuotationNotesDto updateQuotationNotesDto);
        Task<ApiResponse<QuotationNotesGetDto>> UpdateNotesListByQuotationIdAsync(long quotationId, UpdateQuotationNotesListDto request);
        Task<ApiResponse<object>> DeleteQuotationNotesAsync(long id);
    }
}
