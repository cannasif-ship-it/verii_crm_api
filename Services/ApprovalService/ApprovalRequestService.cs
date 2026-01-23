using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace crm_api.Services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ApprovalRequestService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ApprovalRequestGetDto>>> GetAllApprovalRequestsAsync(PagedRequest request)
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

                var query = _unitOfWork.ApprovalRequests
                    .Query()
                    .Where(ar => !ar.IsDeleted)
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalFlow)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ApprovalRequest.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ApprovalRequestGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ApprovalRequestGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ApprovalRequestGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ApprovalRequestGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRequestService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRequestService.GetAllApprovalRequestsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRequestGetDto>> GetApprovalRequestByIdAsync(long id)
        {
            try
            {
                var approvalRequest = await _unitOfWork.ApprovalRequests.GetByIdAsync(id);
                if (approvalRequest == null)
                {
                    return ApiResponse<ApprovalRequestGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var approvalRequestWithNav = await _unitOfWork.ApprovalRequests
                    .Query()
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalFlow)
                    .FirstOrDefaultAsync(ar => ar.Id == id && !ar.IsDeleted);

                var approvalRequestDto = _mapper.Map<ApprovalRequestGetDto>(approvalRequestWithNav ?? approvalRequest);
                return ApiResponse<ApprovalRequestGetDto>.SuccessResult(approvalRequestDto, _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRequestGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRequestService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRequestService.GetApprovalRequestByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRequestGetDto>> CreateApprovalRequestAsync(ApprovalRequestCreateDto approvalRequestCreateDto)
        {
            try
            {
                var approvalRequest = _mapper.Map<ApprovalRequest>(approvalRequestCreateDto);
                await _unitOfWork.ApprovalRequests.AddAsync(approvalRequest);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var approvalRequestWithNav = await _unitOfWork.ApprovalRequests
                    .Query()
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalFlow)
                    .FirstOrDefaultAsync(ar => ar.Id == approvalRequest.Id && !ar.IsDeleted);

                if (approvalRequestWithNav == null)
                {
                    return ApiResponse<ApprovalRequestGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalRequestDto = _mapper.Map<ApprovalRequestGetDto>(approvalRequestWithNav);

                return ApiResponse<ApprovalRequestGetDto>.SuccessResult(approvalRequestDto, _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRequestGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRequestService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRequestService.CreateApprovalRequestExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRequestGetDto>> UpdateApprovalRequestAsync(long id, ApprovalRequestUpdateDto approvalRequestUpdateDto)
        {
            try
            {
                // Get tracked entity for update
                var approvalRequest = await _unitOfWork.ApprovalRequests.GetByIdForUpdateAsync(id);
                if (approvalRequest == null)
                {
                    return ApiResponse<ApprovalRequestGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(approvalRequestUpdateDto, approvalRequest);
                await _unitOfWork.ApprovalRequests.UpdateAsync(approvalRequest);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping (read-only)
                var approvalRequestWithNav = await _unitOfWork.ApprovalRequests
                    .Query()
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalFlow)
                    .FirstOrDefaultAsync(ar => ar.Id == id);

                if (approvalRequestWithNav == null)
                {
                    return ApiResponse<ApprovalRequestGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalRequestDto = _mapper.Map<ApprovalRequestGetDto>(approvalRequestWithNav);

                return ApiResponse<ApprovalRequestGetDto>.SuccessResult(approvalRequestDto, _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRequestGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRequestService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRequestService.UpdateApprovalRequestExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteApprovalRequestAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.ApprovalRequests.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ApprovalRequestService.ApprovalRequestDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRequestService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRequestService.DeleteApprovalRequestExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
