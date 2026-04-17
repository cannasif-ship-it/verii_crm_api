
namespace crm_api.Modules.Customer.Application.Services
{
    public interface ICustomerService
    {
        Task<ApiResponse<PagedResponse<CustomerGetDto>>> GetAllCustomersAsync(CustomerListQueryDto request);
        Task<ApiResponse<CustomerGetDto>> GetCustomerByIdAsync(long id);
        Task<ApiResponse<CustomerGetDto>> CreateCustomerAsync(CustomerCreateDto customerCreateDto);
        Task<ApiResponse<CustomerGetDto>> UpdateCustomerAsync(long id, CustomerUpdateDto customerUpdateDto);
        Task<ApiResponse<object>> DeleteCustomerAsync(long id);
        Task SyncCustomersFromErpAsync();
        Task<ApiResponse<List<CustomerDuplicateCandidateDto>>> GetDuplicateCandidatesAsync();
        Task<ApiResponse<CustomerGetDto>> MergeCustomersAsync(CustomerMergeRequestDto request);
        Task<ApiResponse<CustomerCreateFromMobileResultDto>> CreateCustomerFromMobileAsync(CustomerCreateFromMobileDto request);
        Task<ApiResponse<List<NearbyCustomerPinDto>>> GetNearbyCustomersAsync(CustomerNearbyQueryDto query);
    }
}
