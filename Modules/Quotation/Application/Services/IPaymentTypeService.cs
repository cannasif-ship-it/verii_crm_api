
namespace crm_api.Modules.Quotation.Application.Services
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
