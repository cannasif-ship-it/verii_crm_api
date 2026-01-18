using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
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
