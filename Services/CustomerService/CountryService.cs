using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace crm_api.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<CountryGetDto>>> GetAllCountriesAsync(PagedRequest request)
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

                var query = _unitOfWork.Countries
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .ApplyFilters(request.Filters, request.FilterLogic);

                var sortBy = request.SortBy ?? nameof(Country.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<CountryGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<CountryGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<CountryGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("CountryService.CountriesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<CountryGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CountryService.InternalServerError"),
                    _localizationService.GetLocalizedString("CountryService.GetAllCountriesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CountryGetDto>> GetCountryByIdAsync(long id)
        {
            try
            {
                // Get entity with audit navigation properties
                var country = await _unitOfWork.Countries.GetByIdAsync(id);

                if (country == null)
                {
                    return ApiResponse<CountryGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                        _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var countryDto = _mapper.Map<CountryGetDto>(country);
                return ApiResponse<CountryGetDto>.SuccessResult(countryDto, _localizationService.GetLocalizedString("CountryService.CountryRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CountryGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CountryService.InternalServerError"),
                    _localizationService.GetLocalizedString("CountryService.GetCountryByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CountryGetDto>> CreateCountryAsync(CountryCreateDto countryCreateDto)
        {
            var country = _mapper.Map<Country>(countryCreateDto);
            await _unitOfWork.Countries.AddAsync(country);
            await _unitOfWork.SaveChangesAsync();

            var countryDto = _mapper.Map<CountryGetDto>(country);
            return ApiResponse<CountryGetDto>.SuccessResult(countryDto, _localizationService.GetLocalizedString("CountryService.CountryCreated"));
        }

        public async Task<ApiResponse<CountryGetDto>> UpdateCountryAsync(long id, CountryUpdateDto countryUpdateDto)
        {
            // Get tracked entity for update
            var country = await _unitOfWork.Countries.GetByIdForUpdateAsync(id);
            if (country == null)
            {
                return ApiResponse<CountryGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                    _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                    StatusCodes.Status404NotFound);
            }

            _mapper.Map(countryUpdateDto, country);
            await _unitOfWork.Countries.UpdateAsync(country);
            await _unitOfWork.SaveChangesAsync();

            // Reload with audit navigation properties for mapping (read-only)
            var countryWithNav = await _unitOfWork.Countries.GetByIdAsync(id);

            if (countryWithNav == null)
            {
                return ApiResponse<CountryGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                    _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                    StatusCodes.Status404NotFound);
            }

            var countryDto = _mapper.Map<CountryGetDto>(countryWithNav);
            return ApiResponse<CountryGetDto>.SuccessResult(countryDto, _localizationService.GetLocalizedString("CountryService.CountryUpdated"));
        }

        public async Task<ApiResponse<object>> DeleteCountryAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.Countries.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                        _localizationService.GetLocalizedString("CountryService.CountryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("CountryService.CountryDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("CountryService.InternalServerError"),
                    _localizationService.GetLocalizedString("CountryService.DeleteCountryExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
