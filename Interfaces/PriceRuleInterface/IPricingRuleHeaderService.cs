using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IPricingRuleHeaderService
    {
        Task<ApiResponse<PagedResponse<PricingRuleHeaderGetDto>>> GetAllPricingRuleHeadersAsync(PagedRequest request);
        Task<ApiResponse<PricingRuleHeaderGetDto>> GetPricingRuleHeaderByIdAsync(long id);
        Task<ApiResponse<PricingRuleHeaderGetDto>> CreatePricingRuleHeaderAsync(PricingRuleHeaderCreateDto createDto);
        Task<ApiResponse<PricingRuleHeaderGetDto>> UpdatePricingRuleHeaderAsync(long id, PricingRuleHeaderUpdateDto updateDto);
        Task<ApiResponse<object>> DeletePricingRuleHeaderAsync(long id);
    }
}
