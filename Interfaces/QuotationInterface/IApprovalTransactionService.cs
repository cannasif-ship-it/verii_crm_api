using cms_webapi.DTOs;

namespace cms_webapi.Interfaces
{
    public interface IApprovalTransactionService
    {
        Task<ApiResponse<PagedResponse<ApprovalTransactionGetDto>>> GetAllApprovalTransactionsAsync(PagedRequest request);
        Task<ApiResponse<ApprovalTransactionGetDto>> GetApprovalTransactionByIdAsync(long id);
        Task<ApiResponse<ApprovalTransactionGetDto>> CreateApprovalTransactionAsync(ApprovalTransactionCreateDto createDto);
        Task<ApiResponse<ApprovalTransactionGetDto>> UpdateApprovalTransactionAsync(long id, ApprovalTransactionUpdateDto updateDto);
        Task<ApiResponse<object>> DeleteApprovalTransactionAsync(long id);
        Task<ApiResponse<List<ApprovalTransactionGetDto>>> GetApprovalTransactionsByDocumentIdAsync(long documentId);
        Task<ApiResponse<List<ApprovalTransactionGetDto>>> GetApprovalTransactionsByLineIdAsync(long lineId);
    }
}
