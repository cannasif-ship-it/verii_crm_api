using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace cms_webapi.Services
{
    public class TitleService : ITitleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public TitleService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<TitleDto>>> GetAllTitlesAsync(PagedRequest request)
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

                var query = _unitOfWork.Titles
                    .Query()
                    .Include(t => t.Contacts)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? "Id";
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var titles = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var items = titles.Select(x => _mapper.Map<TitleDto>(x)).ToList();

                var pagedResponse = new PagedResponse<TitleDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<TitleDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("TitleService.TitlesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<TitleDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("TitleService.InternalServerError"),
                    _localizationService.GetLocalizedString("TitleService.GetAllTitlesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TitleDto>> GetTitleByIdAsync(long id)
        {
            try
            {
                var title = await _unitOfWork.Titles.GetByIdAsync(id);
                if (title == null)
                {
                    return ApiResponse<TitleDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var titleWithNav = await _unitOfWork.Titles
                    .Query()
                    .Include(t => t.Contacts)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

                var titleDto = _mapper.Map<TitleDto>(titleWithNav ?? title);
                return ApiResponse<TitleDto>.SuccessResult(titleDto, _localizationService.GetLocalizedString("TitleService.TitleRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TitleDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TitleService.InternalServerError"),
                    _localizationService.GetLocalizedString("TitleService.GetTitleByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TitleDto>> CreateTitleAsync(CreateTitleDto  titleCreateDto)
        {
            try
            {
                var title = _mapper.Map<Title>(titleCreateDto);
                await _unitOfWork.Titles.AddAsync(title);
                await _unitOfWork.SaveChangesAsync();

                var titleWithNav = await _unitOfWork.Titles
                    .Query()
                    .Include(t => t.Contacts)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .FirstOrDefaultAsync(t => t.Id == title.Id && !t.IsDeleted);

                if (titleWithNav == null)
                {
                    return ApiResponse<TitleDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var titleDto = _mapper.Map<TitleDto>(titleWithNav);

                return ApiResponse<TitleDto>.SuccessResult(titleDto, _localizationService.GetLocalizedString("TitleService.TitleCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TitleDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TitleService.InternalServerError"),
                    _localizationService.GetLocalizedString("TitleService.CreateTitleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<TitleDto>> UpdateTitleAsync(long id, UpdateTitleDto  titleUpdateDto)
        {
            try
            {
                var title = await _unitOfWork.Titles.GetByIdForUpdateAsync(id);
                if (title == null)
                {
                    return ApiResponse<TitleDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(titleUpdateDto, title);
                await _unitOfWork.Titles.UpdateAsync(title);
                await _unitOfWork.SaveChangesAsync();

                var titleWithNav = await _unitOfWork.Titles
                    .Query()
                    .Include(t => t.Contacts)
                    .Include(t => t.CreatedByUser)
                    .Include(t => t.UpdatedByUser)
                    .Include(t => t.DeletedByUser)
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

                if (titleWithNav == null)
                {
                    return ApiResponse<TitleDto>.ErrorResult(
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var titleDto = _mapper.Map<TitleDto>(titleWithNav);

                return ApiResponse<TitleDto>.SuccessResult(titleDto, _localizationService.GetLocalizedString("TitleService.TitleUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<TitleDto>.ErrorResult(
                    _localizationService.GetLocalizedString("TitleService.InternalServerError"),
                    _localizationService.GetLocalizedString("TitleService.UpdateTitleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteTitleAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.Titles.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        _localizationService.GetLocalizedString("TitleService.TitleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("TitleService.TitleDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("TitleService.InternalServerError"),
                    _localizationService.GetLocalizedString("TitleService.DeleteTitleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
