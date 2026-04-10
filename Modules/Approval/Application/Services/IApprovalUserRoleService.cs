
namespace crm_api.Modules.Approval.Application.Services
{
    public interface IApprovalUserRoleService
    {
        Task<ApiResponse<PagedResponse<ApprovalUserRoleGetDto>>> GetAllApprovalUserRolesAsync(PagedRequest request);
        Task<ApiResponse<ApprovalUserRoleGetDto>> GetApprovalUserRoleByIdAsync(long id);
        Task<ApiResponse<ApprovalUserRoleGetDto>> CreateApprovalUserRoleAsync(ApprovalUserRoleCreateDto approvalUserRoleCreateDto);
        Task<ApiResponse<ApprovalUserRoleGetDto>> UpdateApprovalUserRoleAsync(long id, ApprovalUserRoleUpdateDto approvalUserRoleUpdateDto);
        Task<ApiResponse<object>> DeleteApprovalUserRoleAsync(long id);
    }
}
