using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
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
