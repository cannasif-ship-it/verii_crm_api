using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IPaymentTypeService
    {
        Task<ApiResponse<PagedResponse<PaymentTypeGetDto>>> GetAllPaymentTypesAsync(PagedRequest request);
        Task<ApiResponse<PaymentTypeGetDto>> GetPaymentTypeByIdAsync(long id);
        Task<ApiResponse<PaymentTypeGetDto>> CreatePaymentTypeAsync(PaymentTypeCreateDto createPaymentTypeDto);
        Task<ApiResponse<PaymentTypeGetDto>> UpdatePaymentTypeAsync(long id, PaymentTypeUpdateDto updatePaymentTypeDto);
        Task<ApiResponse<object>> DeletePaymentTypeAsync(long id);
    }
}
