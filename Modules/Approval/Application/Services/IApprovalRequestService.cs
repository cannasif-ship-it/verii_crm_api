
namespace crm_api.Modules.Approval.Application.Services
{
    public interface IApprovalRequestService
    {
        Task<ApiResponse<PagedResponse<ApprovalRequestGetDto>>> GetAllApprovalRequestsAsync(PagedRequest request);
        Task<ApiResponse<ApprovalRequestGetDto>> GetApprovalRequestByIdAsync(long id);
        Task<ApiResponse<ApprovalRequestGetDto>> CreateApprovalRequestAsync(ApprovalRequestCreateDto approvalRequestCreateDto);
        Task<ApiResponse<ApprovalRequestGetDto>> UpdateApprovalRequestAsync(long id, ApprovalRequestUpdateDto approvalRequestUpdateDto);
        Task<ApiResponse<object>> DeleteApprovalRequestAsync(long id);
    }
}
