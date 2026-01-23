using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IApprovalFlowStepService
    {
        Task<ApiResponse<PagedResponse<ApprovalFlowStepGetDto>>> GetAllApprovalFlowStepsAsync(PagedRequest request);
        Task<ApiResponse<ApprovalFlowStepGetDto>> GetApprovalFlowStepByIdAsync(long id);
        Task<ApiResponse<ApprovalFlowStepGetDto>> CreateApprovalFlowStepAsync(ApprovalFlowStepCreateDto approvalFlowStepCreateDto);
        Task<ApiResponse<ApprovalFlowStepGetDto>> UpdateApprovalFlowStepAsync(long id, ApprovalFlowStepUpdateDto approvalFlowStepUpdateDto);
        Task<ApiResponse<object>> DeleteApprovalFlowStepAsync(long id);
    }
}
