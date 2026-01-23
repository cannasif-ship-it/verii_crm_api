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
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<CityGetDto>>> GetAllCitiesAsync(PagedRequest request)
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

                var query = _unitOfWork.Cities
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .Include(c => c.Country)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(City.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<CityGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<CityGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<CityGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("CityService.CitiesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<CityGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CityService.InternalServerError"),
                    _localizationService.GetLocalizedString("CityService.GetAllCitiesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }        

        public async Task<ApiResponse<CityGetDto>> GetCityByIdAsync(long id)
        {
            try
            {
                var city = await _unitOfWork.Cities.GetByIdAsync(id);
                if (city == null)
                {
                    return ApiResponse<CityGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var cityWithNav = await _unitOfWork.Cities
                    .Query()
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .Include(c => c.Country)
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                var cityDto = _mapper.Map<CityGetDto>(cityWithNav ?? city);
                return ApiResponse<CityGetDto>.SuccessResult(cityDto, _localizationService.GetLocalizedString("CityService.CityRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CityGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CityService.InternalServerError"),
                    _localizationService.GetLocalizedString("CityService.GetCityByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CityGetDto>> CreateCityAsync(CityCreateDto cityCreateDto)
        {
            try
            {
                var city = _mapper.Map<City>(cityCreateDto);
                await _unitOfWork.Cities.AddAsync(city);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var cityWithNav = await _unitOfWork.Cities
                    .Query()
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .Include(c => c.Country)
                    .FirstOrDefaultAsync(c => c.Id == city.Id && !c.IsDeleted);

                if (cityWithNav == null)
                {
                    return ApiResponse<CityGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var cityDto = _mapper.Map<CityGetDto>(cityWithNav);

                return ApiResponse<CityGetDto>.SuccessResult(cityDto, _localizationService.GetLocalizedString("CityService.CityCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CityGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CityService.InternalServerError"),
                    _localizationService.GetLocalizedString("CityService.CreateCityExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CityGetDto>> UpdateCityAsync(long id, CityUpdateDto cityUpdateDto)
        {
            try
            {
                var city = await _unitOfWork.Cities.GetByIdAsync(id);
                if (city == null)
                {
                    return ApiResponse<CityGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(cityUpdateDto, city);
                await _unitOfWork.Cities.UpdateAsync(city);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var cityWithNav = await _unitOfWork.Cities
                    .Query()
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .Include(c => c.Country)
                    .FirstOrDefaultAsync(c => c.Id == city.Id && !c.IsDeleted);

                if (cityWithNav == null)
                {
                    return ApiResponse<CityGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var cityDto = _mapper.Map<CityGetDto>(cityWithNav);

                return ApiResponse<CityGetDto>.SuccessResult(cityDto, _localizationService.GetLocalizedString("CityService.CityUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CityGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CityService.InternalServerError"),
                    _localizationService.GetLocalizedString("CityService.UpdateCityExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteCityAsync(long id)
        {
            try
            {
                var city = await _unitOfWork.Cities.GetByIdAsync(id);
                if (city == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        _localizationService.GetLocalizedString("CityService.CityNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.Cities.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("CityService.CityDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("CityService.InternalServerError"),
                    _localizationService.GetLocalizedString("CityService.DeleteCityExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
