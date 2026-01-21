using cms_webapi.DTOs;
using cms_webapi.Models;

namespace cms_webapi.Interfaces
{
    public interface IDocumentSerialTypeService
    {
        Task<ApiResponse<PagedResponse<DocumentSerialTypeGetDto>>> GetAllDocumentSerialTypesAsync(PagedRequest request);
        Task<ApiResponse<DocumentSerialTypeGetDto>> GetDocumentSerialTypeByIdAsync(long id);
        Task<ApiResponse<DocumentSerialTypeGetDto>> CreateDocumentSerialTypeAsync(DocumentSerialTypeCreateDto createDto);
        Task<ApiResponse<DocumentSerialTypeGetDto>> UpdateDocumentSerialTypeAsync(long id, DocumentSerialTypeUpdateDto updateDto);
        Task<ApiResponse<object>> DeleteDocumentSerialTypeAsync(long id);
        Task<ApiResponse<List<DocumentSerialTypeGetDto>>> GetAvaibleDocumentSerialTypesAsync(long customerTypeId, long salesRepId, PricingRuleType ruleType);
        Task<ApiResponse<string>> GenerateDocumentSerialAsync(long id, bool isNewDocument = true, string? oldDocumentSerial = null);
    }
}
