using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IApprovalFlowService
    {
        Task<ApiResponse<PagedResponse<ApprovalFlowGetDto>>> GetAllApprovalFlowsAsync(PagedRequest request);
        Task<ApiResponse<ApprovalFlowGetDto>> GetApprovalFlowByIdAsync(long id);
        Task<ApiResponse<ApprovalFlowGetDto>> CreateApprovalFlowAsync(ApprovalFlowCreateDto approvalFlowCreateDto);
        Task<ApiResponse<ApprovalFlowGetDto>> UpdateApprovalFlowAsync(long id, ApprovalFlowUpdateDto approvalFlowUpdateDto);
        Task<ApiResponse<object>> DeleteApprovalFlowAsync(long id);
    }
}
