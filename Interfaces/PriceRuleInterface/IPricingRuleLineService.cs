using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IPricingRuleLineService
    {
        Task<ApiResponse<PagedResponse<PricingRuleLineGetDto>>> GetAllPricingRuleLinesAsync(PagedRequest request);
        Task<ApiResponse<PricingRuleLineGetDto>> GetPricingRuleLineByIdAsync(long id);
        Task<ApiResponse<PricingRuleLineGetDto>> CreatePricingRuleLineAsync(PricingRuleLineCreateDto createDto);
        Task<ApiResponse<PricingRuleLineGetDto>> UpdatePricingRuleLineAsync(long id, PricingRuleLineUpdateDto updateDto);
        Task<ApiResponse<object>> DeletePricingRuleLineAsync(long id);
        Task<ApiResponse<List<PricingRuleLineGetDto>>> GetPricingRuleLinesByHeaderIdAsync(long headerId);
    }
}
