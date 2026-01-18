using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace cms_webapi.Services
{
    public class ApprovalRoleService : IApprovalRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ApprovalRoleService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ApprovalRoleGetDto>>> GetAllApprovalRolesAsync(PagedRequest request)
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

                var query = _unitOfWork.ApprovalRoles
                    .Query()
                    .Where(ar => !ar.IsDeleted)
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalRoleGroup)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ApprovalRole.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ApprovalRoleGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ApprovalRoleGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ApprovalRoleGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRolesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ApprovalRoleGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleService.GetAllApprovalRolesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRoleGetDto>> GetApprovalRoleByIdAsync(long id)
        {
            try
            {
                var approvalRole = await _unitOfWork.ApprovalRoles.GetByIdAsync(id);
                if (approvalRole == null)
                {
                    return ApiResponse<ApprovalRoleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var approvalRoleWithNav = await _unitOfWork.ApprovalRoles
                    .Query()
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalRoleGroup)
                    .FirstOrDefaultAsync(ar => ar.Id == id && !ar.IsDeleted);

                var approvalRoleDto = _mapper.Map<ApprovalRoleGetDto>(approvalRoleWithNav ?? approvalRole);
                return ApiResponse<ApprovalRoleGetDto>.SuccessResult(approvalRoleDto, _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRoleGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleService.GetApprovalRoleByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRoleGetDto>> CreateApprovalRoleAsync(ApprovalRoleCreateDto approvalRoleCreateDto)
        {
            try
            {
                var approvalRole = _mapper.Map<ApprovalRole>(approvalRoleCreateDto);
                await _unitOfWork.ApprovalRoles.AddAsync(approvalRole);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var approvalRoleWithNav = await _unitOfWork.ApprovalRoles
                    .Query()
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalRoleGroup)
                    .FirstOrDefaultAsync(ar => ar.Id == approvalRole.Id && !ar.IsDeleted);

                if (approvalRoleWithNav == null)
                {
                    return ApiResponse<ApprovalRoleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalRoleDto = _mapper.Map<ApprovalRoleGetDto>(approvalRoleWithNav);

                return ApiResponse<ApprovalRoleGetDto>.SuccessResult(approvalRoleDto, _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRoleGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleService.CreateApprovalRoleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalRoleGetDto>> UpdateApprovalRoleAsync(long id, ApprovalRoleUpdateDto approvalRoleUpdateDto)
        {
            try
            {
                // Get tracked entity for update
                var approvalRole = await _unitOfWork.ApprovalRoles.GetByIdForUpdateAsync(id);
                if (approvalRole == null)
                {
                    return ApiResponse<ApprovalRoleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(approvalRoleUpdateDto, approvalRole);
                await _unitOfWork.ApprovalRoles.UpdateAsync(approvalRole);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping (read-only)
                var approvalRoleWithNav = await _unitOfWork.ApprovalRoles
                    .Query()
                    .Include(ar => ar.CreatedByUser)
                    .Include(ar => ar.UpdatedByUser)
                    .Include(ar => ar.DeletedByUser)
                    .Include(ar => ar.ApprovalRoleGroup)
                    .FirstOrDefaultAsync(ar => ar.Id == id);

                if (approvalRoleWithNav == null)
                {
                    return ApiResponse<ApprovalRoleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var approvalRoleDto = _mapper.Map<ApprovalRoleGetDto>(approvalRoleWithNav);

                return ApiResponse<ApprovalRoleGetDto>.SuccessResult(approvalRoleDto, _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalRoleGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleService.UpdateApprovalRoleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteApprovalRoleAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.ApprovalRoles.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ApprovalRoleService.ApprovalRoleDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ApprovalRoleService.InternalServerError"),
                    _localizationService.GetLocalizedString("ApprovalRoleService.DeleteApprovalRoleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
