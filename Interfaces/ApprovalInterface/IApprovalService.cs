using cms_webapi.DTOs;
using cms_webapi.Models;

namespace cms_webapi.Interfaces
{
    public interface IApprovalService
    {
        // Teklif kaydedildiğinde çağrılır
        Task ProcessQuotationApproval(long quotationId);
        
        // Satır bazlı onay kontrolü
        Task ProcessLineApproval(long quotationLineId);
        
        // Onay işlemi (Yetki kontrolü ile)
        Task<ApiResponse<bool>> ApproveQuotation(long approvalQueueId, long userId, string? note);
        
        // Red işlemi (Yetki kontrolü ile)
        Task<ApiResponse<bool>> RejectQuotation(long approvalQueueId, long userId, string? note);
        
        // Kullanıcının bekleyen onaylarını getir
        Task<ApiResponse<List<ApprovalQueueGetDto>>> GetPendingApprovals(long userId);
        
        // Teklifin onay durumunu kontrol et
        Task<ApiResponse<ApprovalStatus>> GetQuotationApprovalStatus(long quotationId);
        
        // Kullanıcının bu teklifi onaylama yetkisi var mı?
        Task<bool> CanUserApproveQuotation(long quotationId, long userId);
        
        // Kullanıcının bu teklifi düzenleme yetkisi var mı?
        Task<bool> CanUserEditQuotation(long quotationId, long userId);
        
        // Kullanıcının bu teklifi silme yetkisi var mı?
        Task<bool> CanUserDeleteQuotation(long quotationId, long userId);
        
        // Teklifin onay akışında mı?
        Task<bool> IsQuotationInApprovalProcess(long quotationId);
        
        // Finance onaycısı var mı?
        Task<User?> GetFinanceManager();
        
        // Teklifin mevcut onaycısını getir
        Task<ApprovalQueue?> GetCurrentApprover(long quotationId);
    }
}
