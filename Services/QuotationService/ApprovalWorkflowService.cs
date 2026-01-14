using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Data;
using Microsoft.AspNetCore.Http;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace cms_webapi.Services
{
    public class ApprovalWorkflowService : IApprovalWorkflowService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _loc;
        private readonly CmsDbContext _context;

        public ApprovalWorkflowService(IUnitOfWork uow, IMapper mapper, ILocalizationService loc, CmsDbContext context)
        {
            _uow = uow; _mapper = mapper; _loc = loc; _context = context;
        }

        public async Task<ApiResponse<PagedResponse<ApprovalWorkflowDto>>> GetAllApprovalWorkflowsAsync(PagedRequest request)
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

                var query = _context.ApprovalWorkflows
                    .AsNoTracking()
                    .Where(aw => !aw.IsDeleted)
                    .Include(aw => aw.CustomerType)
                    .Include(aw => aw.CreatedByUser)
                    .Include(aw => aw.UpdatedByUser)
                    .Include(aw => aw.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ApprovalWorkflow.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ApprovalWorkflowDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ApprovalWorkflowDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ApprovalWorkflowDto>>.SuccessResult(pagedResponse, _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ApprovalWorkflowDto>>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.InternalServerError"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.GetAllExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalWorkflowDto>> GetApprovalWorkflowByIdAsync(long id)
        {
            try
            {
                var item = await _uow.ApprovalWorkflows.GetByIdAsync(id);
                if (item == null) return ApiResponse<ApprovalWorkflowDto>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowNotFound"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowNotFound"),
                    StatusCodes.Status404NotFound);

                // Reload with navigation properties for mapping
                var itemWithNav = await _context.ApprovalWorkflows
                    .AsNoTracking()
                    .Include(aw => aw.CustomerType)
                    .Include(aw => aw.CreatedByUser)
                    .Include(aw => aw.UpdatedByUser)
                    .Include(aw => aw.DeletedByUser)
                    .FirstOrDefaultAsync(aw => aw.Id == id && !aw.IsDeleted);

                var dto = _mapper.Map<ApprovalWorkflowDto>(itemWithNav ?? item);
                return ApiResponse<ApprovalWorkflowDto>.SuccessResult(dto, _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalWorkflowDto>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.InternalServerError"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.GetByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalWorkflowDto>> CreateApprovalWorkflowAsync(CreateApprovalWorkflowDto dto)
        {
            try
            {
                var entity = _mapper.Map<ApprovalWorkflow>(dto);
                entity.CreatedDate = DateTime.UtcNow;
                await _uow.ApprovalWorkflows.AddAsync(entity);
                await _uow.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var entityWithNav = await _context.ApprovalWorkflows
                    .AsNoTracking()
                    .Include(aw => aw.CustomerType)
                    .Include(aw => aw.CreatedByUser)
                    .Include(aw => aw.UpdatedByUser)
                    .Include(aw => aw.DeletedByUser)
                    .FirstOrDefaultAsync(aw => aw.Id == entity.Id && !aw.IsDeleted);

                var outDto = _mapper.Map<ApprovalWorkflowDto>(entityWithNav ?? entity);
                return ApiResponse<ApprovalWorkflowDto>.SuccessResult(outDto, _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalWorkflowDto>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.InternalServerError"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.CreateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ApprovalWorkflowDto>> UpdateApprovalWorkflowAsync(long id, UpdateApprovalWorkflowDto dto)
        {
            try
            {
                var entity = await _uow.ApprovalWorkflows.GetByIdAsync(id);
                if (entity == null) return ApiResponse<ApprovalWorkflowDto>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowNotFound"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowNotFound"),
                    StatusCodes.Status404NotFound);
                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                await _uow.ApprovalWorkflows.UpdateAsync(entity);
                await _uow.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var entityWithNav = await _context.ApprovalWorkflows
                    .AsNoTracking()
                    .Include(aw => aw.CustomerType)
                    .Include(aw => aw.CreatedByUser)
                    .Include(aw => aw.UpdatedByUser)
                    .Include(aw => aw.DeletedByUser)
                    .FirstOrDefaultAsync(aw => aw.Id == entity.Id && !aw.IsDeleted);

                var outDto = _mapper.Map<ApprovalWorkflowDto>(entityWithNav ?? entity);
                return ApiResponse<ApprovalWorkflowDto>.SuccessResult(outDto, _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ApprovalWorkflowDto>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.InternalServerError"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.UpdateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteApprovalWorkflowAsync(long id)
        {
            try
            {
                var entity = await _uow.ApprovalWorkflows.GetByIdAsync(id);
                if (entity == null) return ApiResponse<object>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowNotFound"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowNotFound"),
                    StatusCodes.Status404NotFound);
                await _uow.ApprovalWorkflows.SoftDeleteAsync(id);
                await _uow.SaveChangesAsync();
                return ApiResponse<object>.SuccessResult(null, _loc.GetLocalizedString("ApprovalWorkflowService.ApprovalWorkflowDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _loc.GetLocalizedString("ApprovalWorkflowService.InternalServerError"),
                    _loc.GetLocalizedString("ApprovalWorkflowService.DeleteExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}