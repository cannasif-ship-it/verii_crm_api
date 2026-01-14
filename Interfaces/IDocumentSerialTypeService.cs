using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface IDocumentSerialTypeService
    {
        Task<ApiResponse<PagedResponse<DocumentSerialTypeGetDto>>> GetAllDocumentSerialTypesAsync(PagedRequest request);
        Task<ApiResponse<DocumentSerialTypeGetDto>> GetDocumentSerialTypeByIdAsync(long id);
        Task<ApiResponse<DocumentSerialTypeGetDto>> CreateDocumentSerialTypeAsync(DocumentSerialTypeCreateDto createDto);
        Task<ApiResponse<DocumentSerialTypeGetDto>> UpdateDocumentSerialTypeAsync(long id, DocumentSerialTypeUpdateDto updateDto);
        Task<ApiResponse<object>> DeleteDocumentSerialTypeAsync(long id);
    }
}
