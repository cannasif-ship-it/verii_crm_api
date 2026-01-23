// fixed wrong copy-paste namespace
using crm_api.DTOs;
using crm_api.Data;

namespace crm_api.Interfaces
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
