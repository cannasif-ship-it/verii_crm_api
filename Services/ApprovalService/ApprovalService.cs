using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;

namespace cms_webapi.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ApprovalService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task ProcessQuotationApproval(long quotationId)
        {
            var quotation = await _unitOfWork.Quotations
                .Query()
                .Include(q => q.Representative)
                .Include(q => q.Lines)
                .FirstOrDefaultAsync(q => q.Id == quotationId && !q.IsDeleted);

            if (quotation == null) return;

            var salesperson = quotation.Representative;
            if (salesperson == null) return;

            // 1. Hiyerarşiyi bul
            var hierarchy = await _unitOfWork.UserHierarchies
                .Query()
                .Include(h => h.Manager)
                .Include(h => h.GeneralManager)
                .FirstOrDefaultAsync(h => h.SalespersonId == salesperson.Id && h.IsActive && !h.IsDeleted);

            if (hierarchy == null) return;

            var manager = hierarchy.Manager;
            var generalManager = hierarchy.GeneralManager;

            if (manager == null || generalManager == null) return;

            // 2. Manager'ın yetkisini kontrol et
            var managerAuthority = await _unitOfWork.ApprovalAuthorities
                .Query()
                .FirstOrDefaultAsync(a => a.UserId == hierarchy.ManagerId && a.IsActive && !a.IsDeleted);

            int sequenceOrder = 1;

            if (managerAuthority != null &&
                hierarchy.ManagerId.HasValue &&
                quotation.GrandTotal <= managerAuthority.MaxApprovalAmount)
            {
                // Yetki yeterli - Manager'a gönder
                await CreateApprovalQueue(quotationId, null, hierarchy.ManagerId.Value,
                    ApprovalLevel.SalesManager, sequenceOrder++);

                // Üst yönetime gitme kuralını kontrol et
                var rule = await _unitOfWork.ApprovalRules
                    .Query()
                    .FirstOrDefaultAsync(r => r.ApprovalAuthorityId == managerAuthority.Id && r.IsActive && !r.IsDeleted);

                if (rule != null && rule.ForwardToUpperManagement)
                {
                    // Finance seviyesi kontrolü
                    if (rule.ForwardToLevel == ApprovalLevel.Finance)
                    {
                        var financeManager = await GetFinanceManager();

                        if (financeManager != null)
                        {
                            // Finance onaycısı var - Finance'e gönder
                            await CreateApprovalQueue(quotationId, null, financeManager.Id,
                                ApprovalLevel.Finance, sequenceOrder++);

                            // Sonra GeneralManager'a gönder
                            if (hierarchy.GeneralManagerId.HasValue)
                            {
                                await CreateApprovalQueue(quotationId, null, hierarchy.GeneralManagerId.Value,
                                    ApprovalLevel.GeneralManager, sequenceOrder++);
                            }
                        }
                        else if (!rule.RequireFinanceApproval && hierarchy.GeneralManagerId.HasValue)
                        {
                            // Finance onaycısı yok ama zorunlu değil - Direkt GeneralManager'a
                            await CreateApprovalQueue(quotationId, null, hierarchy.GeneralManagerId.Value,
                                ApprovalLevel.GeneralManager, sequenceOrder++);
                        }
                        // Eğer RequireFinanceApproval = true ve financeManager = null ise
                        // Hata fırlatılmalı veya log kaydedilmeli
                    }
                    else if ((rule.ForwardToLevel == ApprovalLevel.GeneralManager ||
                             rule.ForwardToLevel == null) && hierarchy.GeneralManagerId.HasValue)
                    {
                        // Direkt GeneralManager'a
                        await CreateApprovalQueue(quotationId, null, hierarchy.GeneralManagerId.Value,
                            ApprovalLevel.GeneralManager, sequenceOrder++);
                    }
                }
                // else: Sadece Manager'a gönderildi, bitir
            }
            else
            {
                // Yetki yetmez - Direkt GeneralManager'a gönder
                if (hierarchy.GeneralManagerId.HasValue)
                {
                    await CreateApprovalQueue(quotationId, null, hierarchy.GeneralManagerId.Value,
                        ApprovalLevel.GeneralManager, sequenceOrder);
                }
            }

            // 3. Satır bazlı onay kontrolü
            foreach (var line in quotation.Lines.Where(l => !l.IsDeleted))
            {
                await ProcessLineApproval(line.Id);
            }
        }

        public async Task ProcessLineApproval(long quotationLineId)
        {
            var line = await _unitOfWork.QuotationLines
                .Query()
                .Include(l => l.Quotation)
                .FirstOrDefaultAsync(l => l.Id == quotationLineId && !l.IsDeleted);

            if (line == null || line.Quotation == null) return;

            // İndirim toplamı kontrolü
            var totalDiscount = line.DiscountRate1 + line.DiscountRate2 + line.DiscountRate3;

            // Satışçının indirim limitini kontrol et
            var discountLimit = await _unitOfWork.UserDiscountLimits
                .Query()
                .FirstOrDefaultAsync(udl => udl.SalespersonId == line.Quotation.RepresentativeId &&
                                           udl.ErpProductGroupCode == line.ProductCode &&
                                           !udl.IsDeleted);

            if (discountLimit != null && totalDiscount > discountLimit.MaxDiscount1)
            {
                // İndirim limiti aşıldı, onay gerekli
                line.ApprovalStatus = ApprovalStatus.Waiting;

                // Hiyerarşiye göre onay kuyruğu oluştur
                var hierarchy = await _unitOfWork.UserHierarchies
                    .Query()
                    .Include(h => h.Manager)
                    .FirstOrDefaultAsync(h => h.SalespersonId == line.Quotation.RepresentativeId &&
                                            h.IsActive && !h.IsDeleted);

                if (hierarchy != null && hierarchy.ManagerId.HasValue)
                {
                    await CreateApprovalQueue(line.QuotationId, line.Id, hierarchy.ManagerId.Value,
                        ApprovalLevel.SalesManager, 1);
                }
            }
        }

        private async Task CreateApprovalQueue(long quotationId, long? lineId,
            long assignedToUserId, ApprovalLevel level, int sequenceOrder)
        {
            var queue = new ApprovalQueue
            {
                QuotationId = quotationId,
                QuotationLineId = lineId,
                AssignedToUserId = assignedToUserId,
                ApprovalLevel = level,
                Status = ApprovalStatus.Waiting,
                SequenceOrder = sequenceOrder,
                IsCurrent = sequenceOrder == 1, // İlk onay adımı aktif
                AssignedAt = DateTime.UtcNow
            };

            await _unitOfWork.ApprovalQueues.AddAsync(queue);
            await _unitOfWork.SaveChangesAsync();

            // Bildirim gönder (şimdilik boş, sonra SignalR ile eklenebilir)
            // await NotifyApprover(assignedToUserId, quotationId);
        }

        public async Task<ApiResponse<bool>> ApproveQuotation(long approvalQueueId, long userId, string? note)
        {
            try
            {
                // 1. Yetki kontrolü
                var queue = await _unitOfWork.ApprovalQueues
                    .Query()
                    .Include(q => q.Quotation)
                    .FirstOrDefaultAsync(q => q.Id == approvalQueueId && !q.IsDeleted);

                if (queue == null)
                    return ApiResponse<bool>.ErrorResult("Onay kuyruğu bulunamadı.", "Approval queue not found", 404);

                if (!queue.IsCurrent)
                    return ApiResponse<bool>.ErrorResult("Bu onay adımı aktif değil.", "This approval step is not current", 400);

                if (queue.AssignedToUserId != userId)
                    return ApiResponse<bool>.ErrorResult("Bu onay size atanmamış.", "This approval is not assigned to you", 403);

                if (queue.Status != ApprovalStatus.Waiting)
                    return ApiResponse<bool>.ErrorResult("Bu onay zaten işlenmiş.", "This approval has already been processed", 400);

                // 2. ApprovalTransaction oluştur
                var transaction = new ApprovalTransaction
                {
                    DocumentId = queue.QuotationId,
                    LineId = queue.QuotationLineId,
                    ApprovalLevel = queue.ApprovalLevel,
                    Status = ApprovalStatus.Approved,
                    ApprovedByUserId = userId,
                    RequestedAt = queue.AssignedAt,
                    ActionDate = DateTime.UtcNow,
                    Note = note
                };
                await _unitOfWork.ApprovalTransactions.AddAsync(transaction);

                // 3. Kuyruğu güncelle
                queue.Status = ApprovalStatus.Approved;
                queue.CompletedAt = DateTime.UtcNow;
                queue.IsCurrent = false;
                queue.Note = note;
                await _unitOfWork.ApprovalQueues.UpdateAsync(queue);

                // 4. Sıradaki onay var mı?
                var nextQueue = await _unitOfWork.ApprovalQueues
                    .Query()
                    .FirstOrDefaultAsync(q =>
                        q.QuotationId == queue.QuotationId &&
                        q.SequenceOrder == queue.SequenceOrder + 1 &&
                        !q.IsDeleted);

                if (nextQueue != null)
                {
                    nextQueue.IsCurrent = true;
                    await _unitOfWork.ApprovalQueues.UpdateAsync(nextQueue);
                    // await NotifyApprover(nextQueue.AssignedToUserId, queue.QuotationId);
                }
                else
                {
                    // Tüm onaylar tamamlandı
                    await FinalizeQuotationApproval(queue.QuotationId);
                }

                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, "Onay işlemi başarılı");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Onay işlemi sırasında hata oluştu.", ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> RejectQuotation(long approvalQueueId, long userId, string? note)
        {
            try
            {
                // 1. Yetki kontrolü
                var queue = await _unitOfWork.ApprovalQueues
                    .Query()
                    .Include(q => q.Quotation)
                    .FirstOrDefaultAsync(q => q.Id == approvalQueueId && !q.IsDeleted);

                if (queue == null)
                    return ApiResponse<bool>.ErrorResult("Onay kuyruğu bulunamadı.", "Approval queue not found", 404);

                if (!queue.IsCurrent)
                    return ApiResponse<bool>.ErrorResult("Bu onay adımı aktif değil.", "This approval step is not current", 400);

                if (queue.AssignedToUserId != userId)
                    return ApiResponse<bool>.ErrorResult("Bu onay size atanmamış.", "This approval is not assigned to you", 403);

                if (queue.Status != ApprovalStatus.Waiting)
                    return ApiResponse<bool>.ErrorResult("Bu onay zaten işlenmiş.", "This approval has already been processed", 400);

                // 2. ApprovalTransaction oluştur
                var transaction = new ApprovalTransaction
                {
                    DocumentId = queue.QuotationId,
                    LineId = queue.QuotationLineId,
                    ApprovalLevel = queue.ApprovalLevel,
                    Status = ApprovalStatus.Rejected,
                    ApprovedByUserId = userId,
                    RequestedAt = queue.AssignedAt,
                    ActionDate = DateTime.UtcNow,
                    Note = note
                };
                await _unitOfWork.ApprovalTransactions.AddAsync(transaction);

                // 3. Kuyruğu güncelle
                queue.Status = ApprovalStatus.Rejected;
                queue.CompletedAt = DateTime.UtcNow;
                queue.IsCurrent = false;
                queue.Note = note;
                await _unitOfWork.ApprovalQueues.UpdateAsync(queue);

                // 4. Teklifi reddedildi olarak işaretle
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(queue.QuotationId);
                if (quotation != null)
                {
                    quotation.ApprovalStatus = false;
                    await _unitOfWork.Quotations.UpdateAsync(quotation);
                }

                // 5. Tüm bekleyen onayları iptal et
                var pendingQueues = await _unitOfWork.ApprovalQueues
                    .Query()
                    .Where(q => q.QuotationId == queue.QuotationId &&
                               q.Status == ApprovalStatus.Waiting &&
                               !q.IsDeleted)
                    .ToListAsync();

                foreach (var pendingQueue in pendingQueues)
                {
                    pendingQueue.Status = ApprovalStatus.Rejected;
                    pendingQueue.IsCurrent = false;
                    pendingQueue.CompletedAt = DateTime.UtcNow;
                    await _unitOfWork.ApprovalQueues.UpdateAsync(pendingQueue);
                }

                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, "Red işlemi başarılı");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Red işlemi sırasında hata oluştu.", ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<ApprovalQueueGetDto>>> GetPendingApprovals(long userId)
        {
            try
            {
                var queues = await _unitOfWork.ApprovalQueues
                    .Query()
                    .Include(q => q.Quotation)
                    .Include(q => q.QuotationLine)
                    .Include(q => q.AssignedToUser)
                    .Where(q => q.AssignedToUserId == userId &&
                               q.Status == ApprovalStatus.Waiting &&
                               q.IsCurrent &&
                               !q.IsDeleted)
                    .OrderBy(q => q.AssignedAt)
                    .ToListAsync();

                var dtos = queues.Select(q => _mapper.Map<ApprovalQueueGetDto>(q)).ToList();

                return ApiResponse<List<ApprovalQueueGetDto>>.SuccessResult(dtos, "Bekleyen onaylar getirildi");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ApprovalQueueGetDto>>.ErrorResult("Bekleyen onaylar getirilirken hata oluştu.", ex.Message, 500);
            }
        }

        public async Task<ApiResponse<ApprovalStatus>> GetQuotationApprovalStatus(long quotationId)
        {
            try
            {
                var pendingQueue = await _unitOfWork.ApprovalQueues
                    .Query()
                    .FirstOrDefaultAsync(q =>
                        q.QuotationId == quotationId &&
                        q.Status == ApprovalStatus.Waiting &&
                        !q.IsDeleted);

                if (pendingQueue != null)
                {
                    return ApiResponse<ApprovalStatus>.SuccessResult(ApprovalStatus.Waiting, "Onay bekleniyor");
                }

                var quotation = await _unitOfWork.Quotations.GetByIdAsync(quotationId);
                if (quotation != null && quotation.ApprovalStatus == true)
                {
                    return ApiResponse<ApprovalStatus>.SuccessResult(ApprovalStatus.Approved, "Onaylandı");
                }

                return ApiResponse<ApprovalStatus>.SuccessResult(ApprovalStatus.NotRequired, "Onay gerekmiyor");
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalStatus>.ErrorResult("Onay durumu kontrol edilirken hata oluştu.", ex.Message, 500);
            }
        }

        public async Task<bool> CanUserApproveQuotation(long quotationId, long userId)
        {
            var currentQueue = await _unitOfWork.ApprovalQueues
                .Query()
                .FirstOrDefaultAsync(q =>
                    q.QuotationId == quotationId &&
                    q.IsCurrent &&
                    q.Status == ApprovalStatus.Waiting &&
                    !q.IsDeleted);

            return currentQueue != null && currentQueue.AssignedToUserId == userId;
        }

        public async Task<bool> CanUserEditQuotation(long quotationId, long userId)
        {
            // Onay akışında mı kontrol et
            var isInApproval = await IsQuotationInApprovalProcess(quotationId);
            if (isInApproval)
            {
                // Onay akışında ise sadece oluşturan kullanıcı düzenleyebilir
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(quotationId);
                if (quotation == null) return false;

                // Sadece oluşturan kullanıcı düzenleyebilir (onay akışında)
                return quotation.CreatedBy == userId;
            }

            // Onay akışında değilse, oluşturan kullanıcı düzenleyebilir
            var quotation2 = await _unitOfWork.Quotations.GetByIdAsync(quotationId);
            if (quotation2 == null) return false;

            return quotation2.CreatedBy == userId;
        }

        public async Task<bool> CanUserDeleteQuotation(long quotationId, long userId)
        {
            // Onay akışında mı kontrol et
            var isInApproval = await IsQuotationInApprovalProcess(quotationId);
            if (isInApproval)
            {
                // Onay akışında ise silinemez
                return false;
            }

            // Onay akışında değilse, sadece oluşturan kullanıcı silebilir
            var quotation = await _unitOfWork.Quotations.GetByIdAsync(quotationId);
            if (quotation == null) return false;

            return quotation.CreatedBy == userId;
        }

        public async Task<bool> IsQuotationInApprovalProcess(long quotationId)
        {
            var pendingQueue = await _unitOfWork.ApprovalQueues
                .Query()
                .FirstOrDefaultAsync(q =>
                    q.QuotationId == quotationId &&
                    q.Status == ApprovalStatus.Waiting &&
                    !q.IsDeleted);

            return pendingQueue != null;
        }

        public async Task<ApprovalQueue?> GetCurrentApprover(long quotationId)
        {
            return await _unitOfWork.ApprovalQueues
                .Query()
                .Include(q => q.AssignedToUser)
                .FirstOrDefaultAsync(q =>
                    q.QuotationId == quotationId &&
                    q.IsCurrent &&
                    q.Status == ApprovalStatus.Waiting &&
                    !q.IsDeleted);
        }

        public async Task<User?> GetFinanceManager()
        {
            // Finance seviyesinde onay yetkisi olan aktif kullanıcıyı bul
            var financeAuthority = await _unitOfWork.ApprovalAuthorities
                .Query()
                .Include(a => a.User)
                .Where(a => a.ApprovalLevel == ApprovalLevel.Finance &&
                           a.IsActive &&
                           !a.IsDeleted &&
                           a.User != null &&
                           !a.User.IsDeleted)
                .FirstOrDefaultAsync();

            return financeAuthority?.User;
        }

        private async Task FinalizeQuotationApproval(long quotationId)
        {
            var quotation = await _unitOfWork.Quotations.GetByIdAsync(quotationId);
            if (quotation != null)
            {
                quotation.ApprovalStatus = true; // BaseHeaderEntity'den
                quotation.ApprovalDate = DateTime.UtcNow;
                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
