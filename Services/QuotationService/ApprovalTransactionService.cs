using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace cms_webapi.Services
{
    public class ApprovalTransactionService : IApprovalTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ApprovalTransactionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ApprovalTransactionGetDto>>> GetAllApprovalTransactionsAsync(PagedRequest request)
        {
            try
            {
                if (request == null)
                {
                    request = new PagedRequest();
                }

                if (request.Filters == null)
                {
                    request.Filters = new List<Filter>();
                }

                var query = _unitOfWork.ApprovalTransactions
                    .Query()
                    .Where(t => !t.IsDeleted)
                    .Include(t => t.Quotation)
                    .Include(t => t.QuotationLine)
                    .Include(t => t.ApprovedByUser)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ApprovalTransaction.Id);
                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ApprovalTransactionGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ApprovalTransactionGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ApprovalTransactionGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ApprovalTransactionGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalTransactionService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalTransactionService.GetAllTransactionsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalTransactionGetDto>> GetApprovalTransactionByIdAsync(long id)
        {
            try
            {
                var transaction = await _unitOfWork.ApprovalTransactions
                    .Query()
                    .Include(t => t.Quotation)
                    .Include(t => t.QuotationLine)
                    .Include(t => t.ApprovedByUser)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

                if (transaction == null)
                {
                    return ApiResponse<ApprovalTransactionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var transactionDto = _mapper.Map<ApprovalTransactionGetDto>(transaction);
                return ApiResponse<ApprovalTransactionGetDto>.SuccessResult(transactionDto, _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalTransactionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalTransactionService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalTransactionService.GetTransactionByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalTransactionGetDto>> CreateApprovalTransactionAsync(ApprovalTransactionCreateDto createDto)
        {
            try
            {
                var transaction = _mapper.Map<ApprovalTransaction>(createDto);
                await _unitOfWork.ApprovalTransactions.AddAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var transactionWithNav = await _unitOfWork.ApprovalTransactions
                    .Query()
                    .Include(t => t.Quotation)
                    .Include(t => t.QuotationLine)
                    .Include(t => t.ApprovedByUser)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .FirstOrDefaultAsync(t => t.Id == transaction.Id && !t.IsDeleted);

                if (transactionWithNav == null)
                {
                    return ApiResponse<ApprovalTransactionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var transactionDto = _mapper.Map<ApprovalTransactionGetDto>(transactionWithNav);
                return ApiResponse<ApprovalTransactionGetDto>.SuccessResult(transactionDto, _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalTransactionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalTransactionService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalTransactionService.CreateTransactionExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalTransactionGetDto>> UpdateApprovalTransactionAsync(long id, ApprovalTransactionUpdateDto updateDto)
        {
            try
            {
                var transaction = await _unitOfWork.ApprovalTransactions.GetByIdForUpdateAsync(id);
                if (transaction == null)
                {
                    return ApiResponse<ApprovalTransactionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateDto, transaction);
                await _unitOfWork.ApprovalTransactions.UpdateAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var transactionWithNav = await _unitOfWork.ApprovalTransactions
                    .Query()
                    .Include(t => t.Quotation)
                    .Include(t => t.QuotationLine)
                    .Include(t => t.ApprovedByUser)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .FirstOrDefaultAsync(t => t.Id == transaction.Id && !t.IsDeleted);

                if (transactionWithNav == null)
                {
                    return ApiResponse<ApprovalTransactionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var transactionDto = _mapper.Map<ApprovalTransactionGetDto>(transactionWithNav);
                return ApiResponse<ApprovalTransactionGetDto>.SuccessResult(transactionDto, _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalTransactionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalTransactionService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalTransactionService.UpdateTransactionExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteApprovalTransactionAsync(long id)
        {
            try
            {
                var transaction = await _unitOfWork.ApprovalTransactions.GetByIdAsync(id);
                if (transaction == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.ApprovalTransactions.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalTransactionService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalTransactionService.DeleteTransactionExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<ApprovalTransactionGetDto>>> GetApprovalTransactionsByDocumentIdAsync(long documentId)
        {
            try
            {
                var transactions = await _unitOfWork.ApprovalTransactions
                    .Query()
                    .Where(t => t.DocumentId == documentId && !t.IsDeleted)
                    .Include(t => t.Quotation)
                    .Include(t => t.QuotationLine)
                    .Include(t => t.ApprovedByUser)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .ToListAsync();

                var dtos = transactions.Select(x => _mapper.Map<ApprovalTransactionGetDto>(x)).ToList();
                return ApiResponse<List<ApprovalTransactionGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ApprovalTransactionGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalTransactionService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalTransactionService.GetTransactionsByDocumentIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<ApprovalTransactionGetDto>>> GetApprovalTransactionsByLineIdAsync(long lineId)
        {
            try
            {
                var transactions = await _unitOfWork.ApprovalTransactions
                    .Query()
                    .Where(t => t.LineId == lineId && !t.IsDeleted)
                    .Include(t => t.Quotation)
                    .Include(t => t.QuotationLine)
                    .Include(t => t.ApprovedByUser)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .ToListAsync();

                var dtos = transactions.Select(x => _mapper.Map<ApprovalTransactionGetDto>(x)).ToList();
                return ApiResponse<List<ApprovalTransactionGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ApprovalTransactionService.TransactionsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ApprovalTransactionGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalTransactionService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalTransactionService.GetTransactionsByLineIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
