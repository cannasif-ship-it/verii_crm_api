using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IApprovalRoleGroupService
    {
        Task<ApiResponse<PagedResponse<ApprovalRoleGroupGetDto>>> GetAllApprovalRoleGroupsAsync(PagedRequest request);
        Task<ApiResponse<ApprovalRoleGroupGetDto>> GetApprovalRoleGroupByIdAsync(long id);
        Task<ApiResponse<ApprovalRoleGroupGetDto>> CreateApprovalRoleGroupAsync(ApprovalRoleGroupCreateDto approvalRoleGroupCreateDto);
        Task<ApiResponse<ApprovalRoleGroupGetDto>> UpdateApprovalRoleGroupAsync(long id, ApprovalRoleGroupUpdateDto approvalRoleGroupUpdateDto);
        Task<ApiResponse<object>> DeleteApprovalRoleGroupAsync(long id);
    }
}
