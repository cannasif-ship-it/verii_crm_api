using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IShippingAddressService
    {
        Task<ApiResponse<PagedResponse<ShippingAddressGetDto>>> GetAllShippingAddressesAsync(PagedRequest request);
        Task<ApiResponse<ShippingAddressGetDto>> GetShippingAddressByIdAsync(long id);
        Task<ApiResponse<List<ShippingAddressGetDto>>> GetShippingAddressesByCustomerIdAsync(long customerId);
        Task<ApiResponse<ShippingAddressGetDto>> CreateShippingAddressAsync(CreateShippingAddressDto createShippingAddressDto);
        Task<ApiResponse<ShippingAddressGetDto>> UpdateShippingAddressAsync(long id, UpdateShippingAddressDto updateShippingAddressDto);
        Task<ApiResponse<object>> DeleteShippingAddressAsync(long id);
    }
}
