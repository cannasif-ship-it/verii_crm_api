using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IApprovalActionService
    {
        Task<ApiResponse<PagedResponse<ApprovalActionGetDto>>> GetAllApprovalActionsAsync(PagedRequest request);
        Task<ApiResponse<ApprovalActionGetDto>> GetApprovalActionByIdAsync(long id);
        Task<ApiResponse<ApprovalActionGetDto>> CreateApprovalActionAsync(ApprovalActionCreateDto approvalActionCreateDto);
        Task<ApiResponse<ApprovalActionGetDto>> UpdateApprovalActionAsync(long id, ApprovalActionUpdateDto approvalActionUpdateDto);
        Task<ApiResponse<object>> DeleteApprovalActionAsync(long id);
    }
}
