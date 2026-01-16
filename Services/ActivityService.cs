using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Helpers;
using cms_webapi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace cms_webapi.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;

        public ActivityService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, CmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
        }

        public async Task<ApiResponse<PagedResponse<ActivityDto>>> GetAllActivitiesAsync(PagedRequest request)
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

                var query = _context.Activities
                    .AsNoTracking()
                    .Where(a => !a.IsDeleted)
                    .Include(a => a.ActivityType)
                    .Include(a => a.CreatedByUser)
                    .Include(a => a.UpdatedByUser)
                    .Include(a => a.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(Activity.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ActivityDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ActivityDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ActivityDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ActivityService.ActivitiesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ActivityDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityService.InternalServerError"),
                    _localizationService.GetLocalizedString("ActivityService.GetAllActivitiesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ActivityDto>> GetActivityByIdAsync(long id)
        {
            try
            {
                var activity = await _unitOfWork.Activities.GetByIdAsync(id);
                if (activity == null)
                {
                    return ApiResponse<ActivityDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityService.ActivityNotFound"),
                    _localizationService.GetLocalizedString("ActivityService.ActivityNotFound"),
                    StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var activityWithNav = await _context.Activities
                    .AsNoTracking()
                    .Include(a => a.ActivityType)
                    .Include(a => a.CreatedByUser)
                    .Include(a => a.UpdatedByUser)
                    .Include(a => a.DeletedByUser)
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

                var activityDto = _mapper.Map<ActivityDto>(activityWithNav ?? activity);
                return ApiResponse<ActivityDto>.SuccessResult(activityDto, _localizationService.GetLocalizedString("ActivityService.ActivityRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ActivityDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityService.InternalServerError"),
                    _localizationService.GetLocalizedString("ActivityService.GetActivityByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ActivityDto>> CreateActivityAsync(CreateActivityDto createActivityDto)
        {
            try
            {
                var activity = _mapper.Map<Activity>(createActivityDto);
                var createdActivity = await _unitOfWork.Activities.AddAsync(activity);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var activityWithNav = await _context.Activities
                    .AsNoTracking()
                    .Include(a => a.ActivityType)
                    .Include(a => a.CreatedByUser)
                    .Include(a => a.UpdatedByUser)
                    .Include(a => a.DeletedByUser)
                    .FirstOrDefaultAsync(a => a.Id == createdActivity.Id && !a.IsDeleted);

                var activityDto = _mapper.Map<ActivityDto>(activityWithNav ?? createdActivity);

                return ApiResponse<ActivityDto>.SuccessResult(activityDto, _localizationService.GetLocalizedString("ActivityService.ActivityCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ActivityDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityService.InternalServerError"),
                    _localizationService.GetLocalizedString("ActivityService.CreateActivityExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ActivityDto>> UpdateActivityAsync(long id, UpdateActivityDto updateActivityDto)
        {
            try
            {
                var activity = await _unitOfWork.Activities.GetByIdAsync(id);
                if (activity == null)
                {
                    return ApiResponse<ActivityDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityService.ActivityNotFound"),
                    _localizationService.GetLocalizedString("ActivityService.ActivityNotFound"),
                    StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateActivityDto, activity);
                var updatedActivity = await _unitOfWork.Activities.UpdateAsync(activity);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var activityWithNav = await _context.Activities
                    .AsNoTracking()
                    .Include(a => a.ActivityType)
                    .Include(a => a.CreatedByUser)
                    .Include(a => a.UpdatedByUser)
                    .Include(a => a.DeletedByUser)
                    .FirstOrDefaultAsync(a => a.Id == updatedActivity.Id && !a.IsDeleted);

                var activityDto = _mapper.Map<ActivityDto>(activityWithNav ?? updatedActivity);

                return ApiResponse<ActivityDto>.SuccessResult(activityDto, _localizationService.GetLocalizedString("ActivityService.ActivityUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ActivityDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityService.InternalServerError"),
                    _localizationService.GetLocalizedString("ActivityService.UpdateActivityExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteActivityAsync(long id)
        {
            try
            {
                var activity = await _unitOfWork.Activities.GetByIdAsync(id);
                if (activity == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ActivityService.ActivityNotFound"),
                        _localizationService.GetLocalizedString("ActivityService.ActivityNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.Activities.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(activity, _localizationService.GetLocalizedString("ActivityService.ActivityDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ActivityService.InternalServerError"),
                    _localizationService.GetLocalizedString("ActivityService.DeleteActivityExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
