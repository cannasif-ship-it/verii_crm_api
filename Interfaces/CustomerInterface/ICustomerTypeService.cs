using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface ICustomerTypeService
    {
        Task<ApiResponse<PagedResponse<CustomerTypeGetDto>>> GetAllCustomerTypesAsync(PagedRequest request);
        Task<ApiResponse<CustomerTypeGetDto>> GetCustomerTypeByIdAsync(long id);
        Task<ApiResponse<CustomerTypeGetDto>> CreateCustomerTypeAsync(CustomerTypeCreateDto customerTypeCreateDto);
        Task<ApiResponse<CustomerTypeGetDto>> UpdateCustomerTypeAsync(long id, CustomerTypeUpdateDto customerTypeUpdateDto);
        Task<ApiResponse<object>> DeleteCustomerTypeAsync(long id);
    }
}
