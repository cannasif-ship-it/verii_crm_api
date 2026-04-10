
namespace crm_api.Modules.Approval.Application.Services
{
    public interface IApprovalRoleService
    {
        Task<ApiResponse<PagedResponse<ApprovalRoleGetDto>>> GetAllApprovalRolesAsync(PagedRequest request);
        Task<ApiResponse<ApprovalRoleGetDto>> GetApprovalRoleByIdAsync(long id);
        Task<ApiResponse<ApprovalRoleGetDto>> CreateApprovalRoleAsync(ApprovalRoleCreateDto approvalRoleCreateDto);
        Task<ApiResponse<ApprovalRoleGetDto>> UpdateApprovalRoleAsync(long id, ApprovalRoleUpdateDto approvalRoleUpdateDto);
        Task<ApiResponse<object>> DeleteApprovalRoleAsync(long id);
    }
}
