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
    public class ApprovalRoleGroupService : IApprovalRoleGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ApprovalRoleGroupService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ApprovalRoleGroupGetDto>>> GetAllApprovalRoleGroupsAsync(PagedRequest request)
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

                var query = _unitOfWork.ApprovalRoleGroups
                    .Query()
                    .Where(arg => !arg.IsDeleted)
                    .Include(arg => arg.CreatedByUser)
                    .Include(arg => arg.UpdatedByUser)
                    .Include(arg => arg.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ApprovalRoleGroup.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ApprovalRoleGroupGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ApprovalRoleGroupGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ApprovalRoleGroupGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ApprovalRoleGroupGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.GetAllApprovalRoleGroupsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRoleGroupGetDto>> GetApprovalRoleGroupByIdAsync(long id)
        {
            try
            {
                var approvalRoleGroup = await _unitOfWork.ApprovalRoleGroups.GetByIdAsync(id);
                if (approvalRoleGroup == null)
                {
                    return ApiResponse<ApprovalRoleGroupGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var approvalRoleGroupWithNav = await _unitOfWork.ApprovalRoleGroups
                    .Query()
                    .Include(arg => arg.CreatedByUser)
                    .Include(arg => arg.UpdatedByUser)
                    .Include(arg => arg.DeletedByUser)
                    .FirstOrDefaultAsync(arg => arg.Id == id && !arg.IsDeleted);

                var approvalRoleGroupDto = _mapper.Map<ApprovalRoleGroupGetDto>(approvalRoleGroupWithNav ?? approvalRoleGroup);
                return ApiResponse<ApprovalRoleGroupGetDto>.SuccessResult(approvalRoleGroupDto, _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRoleGroupGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.GetApprovalRoleGroupByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRoleGroupGetDto>> CreateApprovalRoleGroupAsync(ApprovalRoleGroupCreateDto approvalRoleGroupCreateDto)
        {
            try
            {
                var approvalRoleGroup = _mapper.Map<ApprovalRoleGroup>(approvalRoleGroupCreateDto);
                await _unitOfWork.ApprovalRoleGroups.AddAsync(approvalRoleGroup);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var approvalRoleGroupWithNav = await _unitOfWork.ApprovalRoleGroups
                    .Query()
                    .Include(arg => arg.CreatedByUser)
                    .Include(arg => arg.UpdatedByUser)
                    .Include(arg => arg.DeletedByUser)
                    .FirstOrDefaultAsync(arg => arg.Id == approvalRoleGroup.Id && !arg.IsDeleted);

                if (approvalRoleGroupWithNav == null)
                {
                    return ApiResponse<ApprovalRoleGroupGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalRoleGroupDto = _mapper.Map<ApprovalRoleGroupGetDto>(approvalRoleGroupWithNav);

                return ApiResponse<ApprovalRoleGroupGetDto>.SuccessResult(approvalRoleGroupDto, _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRoleGroupGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.CreateApprovalRoleGroupExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRoleGroupGetDto>> UpdateApprovalRoleGroupAsync(long id, ApprovalRoleGroupUpdateDto approvalRoleGroupUpdateDto)
        {
            try
            {
                // Get tracked entity for update
                var approvalRoleGroup = await _unitOfWork.ApprovalRoleGroups.GetByIdForUpdateAsync(id);
                if (approvalRoleGroup == null)
                {
                    return ApiResponse<ApprovalRoleGroupGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(approvalRoleGroupUpdateDto, approvalRoleGroup);
                await _unitOfWork.ApprovalRoleGroups.UpdateAsync(approvalRoleGroup);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping (read-only)
                var approvalRoleGroupWithNav = await _unitOfWork.ApprovalRoleGroups
                    .Query()
                    .Include(arg => arg.CreatedByUser)
                    .Include(arg => arg.UpdatedByUser)
                    .Include(arg => arg.DeletedByUser)
                    .FirstOrDefaultAsync(arg => arg.Id == id);

                if (approvalRoleGroupWithNav == null)
                {
                    return ApiResponse<ApprovalRoleGroupGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalRoleGroupDto = _mapper.Map<ApprovalRoleGroupGetDto>(approvalRoleGroupWithNav);

                return ApiResponse<ApprovalRoleGroupGetDto>.SuccessResult(approvalRoleGroupDto, _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRoleGroupGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.UpdateApprovalRoleGroupExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteApprovalRoleGroupAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.ApprovalRoleGroups.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ApprovalRoleGroupService.ApprovalRoleGroupDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleGroupService.DeleteApprovalRoleGroupExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
