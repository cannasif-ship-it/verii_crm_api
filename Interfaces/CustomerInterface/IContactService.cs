// fixed wrong copy-paste namespace
using cms_webapi.DTOs;
using cms_webapi.Data;

namespace cms_webapi.Interfaces
{
    public interface IContactService
    {
        Task<ApiResponse<PagedResponse<ContactDto>>> GetAllContactsAsync(PagedRequest request);
        Task<ApiResponse<ContactDto>> GetContactByIdAsync(long id);
        Task<ApiResponse<ContactDto>> CreateContactAsync(CreateContactDto createContactDto);
        Task<ApiResponse<ContactDto>> UpdateContactAsync(long id, UpdateContactDto updateContactDto);
        Task<ApiResponse<object>> DeleteContactAsync(long id);
    }
}
