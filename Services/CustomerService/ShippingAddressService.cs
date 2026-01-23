using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using crm_api.Helpers;
using System;
using System.Collections.Generic;

namespace crm_api.Services
{
    public class ShippingAddressService : IShippingAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShippingAddressService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ShippingAddressGetDto>>> GetAllShippingAddressesAsync(PagedRequest request)
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

                var query = _unitOfWork.ShippingAddresses
                    .Query()
                    .Where(sa => !sa.IsDeleted)
                    .Include(sa => sa.CreatedByUser)
                    .Include(sa => sa.UpdatedByUser)
                    .Include(sa => sa.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ShippingAddress.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ShippingAddressGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ShippingAddressGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ShippingAddressGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ShippingAddressGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ShippingAddressService.InternalServerError"),
                    _localizationService.GetLocalizedString("ShippingAddressService.GetAllShippingAddressesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ShippingAddressGetDto>> GetShippingAddressByIdAsync(long id)
        {
            try
            {
                var shippingAddress = await _unitOfWork.ShippingAddresses.GetByIdAsync(id);

                if (shippingAddress == null)
                {
                    return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var shippingAddressWithNav = await _unitOfWork.ShippingAddresses
                    .Query()
                    .Include(sa => sa.CreatedByUser)
                    .Include(sa => sa.UpdatedByUser)
                    .Include(sa => sa.DeletedByUser)
                    .FirstOrDefaultAsync(sa => sa.Id == id && !sa.IsDeleted);

                if (shippingAddressWithNav == null)
                {
                    return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var shippingAddressDto = _mapper.Map<ShippingAddressGetDto>(shippingAddressWithNav);

                return ApiResponse<ShippingAddressGetDto>.SuccessResult(shippingAddressDto, _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ShippingAddressService.InternalServerError"),
                    _localizationService.GetLocalizedString("ShippingAddressService.GetShippingAddressByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<ShippingAddressGetDto>>> GetShippingAddressesByCustomerIdAsync(long customerId)
        {
            try
            {
                var shippingAddresses = await _unitOfWork.ShippingAddresses.FindAsync(sa => sa.CustomerId == customerId);

                var shippingAddressDtos = _mapper.Map<List<ShippingAddressGetDto>>(shippingAddresses);

                return ApiResponse<List<ShippingAddressGetDto>>.SuccessResult(shippingAddressDtos, _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressesByCustomerRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ShippingAddressGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ShippingAddressService.InternalServerError"),
                    _localizationService.GetLocalizedString("ShippingAddressService.GetShippingAddressesByCustomerIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ShippingAddressGetDto>> CreateShippingAddressAsync(CreateShippingAddressDto createShippingAddressDto)
        {
            try
            {
                var shippingAddress = _mapper.Map<ShippingAddress>(createShippingAddressDto);

                await _unitOfWork.ShippingAddresses.AddAsync(shippingAddress);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var shippingAddressWithNav = await _unitOfWork.ShippingAddresses
                    .Query()
                    .Include(sa => sa.CreatedByUser)
                    .Include(sa => sa.UpdatedByUser)
                    .Include(sa => sa.DeletedByUser)
                    .FirstOrDefaultAsync(sa => sa.Id == shippingAddress.Id && !sa.IsDeleted);

                if (shippingAddressWithNav == null)
                {
                    return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var shippingAddressDto = _mapper.Map<ShippingAddressGetDto>(shippingAddressWithNav);

                return ApiResponse<ShippingAddressGetDto>.SuccessResult(shippingAddressDto, _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ShippingAddressService.InternalServerError"),
                    _localizationService.GetLocalizedString("ShippingAddressService.CreateShippingAddressExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ShippingAddressGetDto>> UpdateShippingAddressAsync(long id, UpdateShippingAddressDto updateShippingAddressDto)
        {
            try
            {
                var existingShippingAddress = await _unitOfWork.ShippingAddresses.GetByIdAsync(id);

                if (existingShippingAddress == null)
                {
                    return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateShippingAddressDto, existingShippingAddress);
                existingShippingAddress.UpdatedDate = DateTime.UtcNow;

               await _unitOfWork.ShippingAddresses.UpdateAsync(existingShippingAddress);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var shippingAddressWithNav = await _unitOfWork.ShippingAddresses
                    .Query()
                    .Include(sa => sa.CreatedByUser)
                    .Include(sa => sa.UpdatedByUser)
                    .Include(sa => sa.DeletedByUser)
                    .FirstOrDefaultAsync(sa => sa.Id == id && !sa.IsDeleted);

                if (shippingAddressWithNav == null)
                {
                    return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var shippingAddressDto = _mapper.Map<ShippingAddressGetDto>(shippingAddressWithNav);

                return ApiResponse<ShippingAddressGetDto>.SuccessResult(shippingAddressDto, _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShippingAddressGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ShippingAddressService.InternalServerError"),
                    _localizationService.GetLocalizedString("ShippingAddressService.UpdateShippingAddressExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteShippingAddressAsync(long id)
        {
            try
            {
                var shippingAddress = await _unitOfWork.ShippingAddresses.GetByIdAsync(id);

                if (shippingAddress == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressNotFound"),
                        StatusCodes.Status404NotFound);
                }

                shippingAddress.DeletedDate = DateTime.UtcNow;

                await _unitOfWork.ShippingAddresses.UpdateAsync(shippingAddress);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ShippingAddressService.ShippingAddressDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ShippingAddressService.InternalServerError"),
                    _localizationService.GetLocalizedString("ShippingAddressService.DeleteShippingAddressExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
