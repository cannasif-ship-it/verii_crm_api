using crm_api.DTOs;
using crm_api.Models;

namespace crm_api.Interfaces
{
    public interface IDemandService
    {
        Task<ApiResponse<PagedResponse<DemandGetDto>>> GetAllDemandsAsync(PagedRequest request);
        Task<ApiResponse<DemandGetDto>> GetDemandByIdAsync(long id);
        Task<ApiResponse<DemandDto>> CreateDemandAsync(CreateDemandDto createDemandDto);
        Task<ApiResponse<DemandDto>> UpdateDemandAsync(long id, UpdateDemandDto updateDemandDto);
        Task<ApiResponse<object>> DeleteDemandAsync(long id);
        Task<ApiResponse<List<DemandGetDto>>> GetDemandsByPotentialCustomerIdAsync(long potentialCustomerId);
        Task<ApiResponse<List<DemandGetDto>>> GetDemandsByRepresentativeIdAsync(long representativeId);
        Task<ApiResponse<List<DemandGetDto>>> GetDemandsByStatusAsync(int status);
        Task<ApiResponse<bool>> DemandExistsAsync(long id);
        Task<ApiResponse<DemandGetDto>> CreateDemandBulkAsync(DemandBulkCreateDto bulkDto);
        Task<ApiResponse<DemandGetDto>> CreateRevisionOfDemandAsync(long demandId);
        Task<ApiResponse<List<PricingRuleLineGetDto>>> GetPriceRuleOfDemandAsync(string customerCode,long salesmenId,DateTime demandDate);
        Task<ApiResponse<List<PriceOfProductDto>>> GetPriceOfProductAsync(List<PriceOfProductRequestDto> request);
        Task<ApiResponse<bool>> StartApprovalFlowAsync(StartApprovalFlowDto request);
        Task<ApiResponse<List<ApprovalActionGetDto>>> GetWaitingApprovalsAsync();
        Task<ApiResponse<bool>> ApproveAsync(ApproveActionDto request);
        Task<ApiResponse<bool>> RejectAsync(RejectActionDto request);
        Task<ApiResponse<PagedResponse<DemandGetDto>>> GetRelatedDemands(PagedRequest request);
        Task<ApiResponse<List<ApprovalScopeUserDto>>> GetDemandRelatedUsersAsync(long userId);
        Task<ApiResponse<long>> ConvertToQuotationAsync(long demandId);
    }
}
