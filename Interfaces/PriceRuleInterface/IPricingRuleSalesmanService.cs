using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IPricingRuleSalesmanService
    {
        Task<ApiResponse<PagedResponse<PricingRuleSalesmanGetDto>>> GetAllPricingRuleSalesmenAsync(PagedRequest request);
        Task<ApiResponse<PricingRuleSalesmanGetDto>> GetPricingRuleSalesmanByIdAsync(long id);
        Task<ApiResponse<PricingRuleSalesmanGetDto>> CreatePricingRuleSalesmanAsync(PricingRuleSalesmanCreateDto createDto);
        Task<ApiResponse<PricingRuleSalesmanGetDto>> UpdatePricingRuleSalesmanAsync(long id, PricingRuleSalesmanUpdateDto updateDto);
        Task<ApiResponse<object>> DeletePricingRuleSalesmanAsync(long id);
        Task<ApiResponse<List<PricingRuleSalesmanGetDto>>> GetPricingRuleSalesmenByHeaderIdAsync(long headerId);
    }
}
