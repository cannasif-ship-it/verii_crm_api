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
    public class ApprovalFlowStepService : IApprovalFlowStepService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ApprovalFlowStepService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ApprovalFlowStepGetDto>>> GetAllApprovalFlowStepsAsync(PagedRequest request)
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

                var query = _unitOfWork.ApprovalFlowSteps
                    .Query()
                    .Where(afs => !afs.IsDeleted)
                    .Include(afs => afs.CreatedByUser)
                    .Include(afs => afs.UpdatedByUser)
                    .Include(afs => afs.DeletedByUser)
                    .Include(afs => afs.ApprovalFlow)
                    .Include(afs => afs.ApprovalRoleGroup)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ApprovalFlowStep.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ApprovalFlowStepGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ApprovalFlowStepGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ApprovalFlowStepGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ApprovalFlowStepGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.GetAllApprovalFlowStepsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalFlowStepGetDto>> GetApprovalFlowStepByIdAsync(long id)
        {
            try
            {
                var approvalFlowStep = await _unitOfWork.ApprovalFlowSteps.GetByIdAsync(id);
                if (approvalFlowStep == null)
                {
                    return ApiResponse<ApprovalFlowStepGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var approvalFlowStepWithNav = await _unitOfWork.ApprovalFlowSteps
                    .Query()
                    .Include(afs => afs.CreatedByUser)
                    .Include(afs => afs.UpdatedByUser)
                    .Include(afs => afs.DeletedByUser)
                    .Include(afs => afs.ApprovalFlow)
                    .Include(afs => afs.ApprovalRoleGroup)
                    .FirstOrDefaultAsync(afs => afs.Id == id && !afs.IsDeleted);

                var approvalFlowStepDto = _mapper.Map<ApprovalFlowStepGetDto>(approvalFlowStepWithNav ?? approvalFlowStep);
                return ApiResponse<ApprovalFlowStepGetDto>.SuccessResult(approvalFlowStepDto, _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalFlowStepGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.GetApprovalFlowStepByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalFlowStepGetDto>> CreateApprovalFlowStepAsync(ApprovalFlowStepCreateDto approvalFlowStepCreateDto)
        {
            try
            {
                var approvalFlowStep = _mapper.Map<ApprovalFlowStep>(approvalFlowStepCreateDto);
                await _unitOfWork.ApprovalFlowSteps.AddAsync(approvalFlowStep);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var approvalFlowStepWithNav = await _unitOfWork.ApprovalFlowSteps
                    .Query()
                    .Include(afs => afs.CreatedByUser)
                    .Include(afs => afs.UpdatedByUser)
                    .Include(afs => afs.DeletedByUser)
                    .Include(afs => afs.ApprovalFlow)
                    .Include(afs => afs.ApprovalRoleGroup)
                    .FirstOrDefaultAsync(afs => afs.Id == approvalFlowStep.Id && !afs.IsDeleted);

                if (approvalFlowStepWithNav == null)
                {
                    return ApiResponse<ApprovalFlowStepGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalFlowStepDto = _mapper.Map<ApprovalFlowStepGetDto>(approvalFlowStepWithNav);

                return ApiResponse<ApprovalFlowStepGetDto>.SuccessResult(approvalFlowStepDto, _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalFlowStepGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.CreateApprovalFlowStepExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalFlowStepGetDto>> UpdateApprovalFlowStepAsync(long id, ApprovalFlowStepUpdateDto approvalFlowStepUpdateDto)
        {
            try
            {
                // Get tracked entity for update
                var approvalFlowStep = await _unitOfWork.ApprovalFlowSteps.GetByIdForUpdateAsync(id);
                if (approvalFlowStep == null)
                {
                    return ApiResponse<ApprovalFlowStepGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(approvalFlowStepUpdateDto, approvalFlowStep);
                await _unitOfWork.ApprovalFlowSteps.UpdateAsync(approvalFlowStep);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping (read-only)
                var approvalFlowStepWithNav = await _unitOfWork.ApprovalFlowSteps
                    .Query()
                    .Include(afs => afs.CreatedByUser)
                    .Include(afs => afs.UpdatedByUser)
                    .Include(afs => afs.DeletedByUser)
                    .Include(afs => afs.ApprovalFlow)
                    .Include(afs => afs.ApprovalRoleGroup)
                    .FirstOrDefaultAsync(afs => afs.Id == id);

                if (approvalFlowStepWithNav == null)
                {
                    return ApiResponse<ApprovalFlowStepGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalFlowStepDto = _mapper.Map<ApprovalFlowStepGetDto>(approvalFlowStepWithNav);

                return ApiResponse<ApprovalFlowStepGetDto>.SuccessResult(approvalFlowStepDto, _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalFlowStepGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.UpdateApprovalFlowStepExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteApprovalFlowStepAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.ApprovalFlowSteps.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ApprovalFlowStepService.ApprovalFlowStepDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalFlowStepService.DeleteApprovalFlowStepExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
