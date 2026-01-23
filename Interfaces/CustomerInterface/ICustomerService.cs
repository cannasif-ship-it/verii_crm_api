using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface ICustomerService
    {
        Task<ApiResponse<PagedResponse<CustomerGetDto>>> GetAllCustomersAsync(PagedRequest request);
        Task<ApiResponse<CustomerGetDto>> GetCustomerByIdAsync(long id);
        Task<ApiResponse<CustomerGetDto>> CreateCustomerAsync(CustomerCreateDto customerCreateDto);
        Task<ApiResponse<CustomerGetDto>> UpdateCustomerAsync(long id, CustomerUpdateDto customerUpdateDto);
        Task<ApiResponse<object>> DeleteCustomerAsync(long id);
    }
}
