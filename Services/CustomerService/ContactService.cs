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
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ContactService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ContactDto>>> GetAllContactsAsync(PagedRequest request)
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

                var query = _unitOfWork.Contacts
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(Contact.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ContactDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ContactDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ContactDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ContactService.ContactsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ContactDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.GetAllContactsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ContactDto>> GetContactByIdAsync(long id)
        {
            try
            {
                var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
                if (contact == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var contactWithNav = await _unitOfWork.Contacts
                    .Query()
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                var contactDto = _mapper.Map<ContactDto>(contactWithNav ?? contact);
                return ApiResponse<ContactDto>.SuccessResult(contactDto, _localizationService.GetLocalizedString("ContactService.ContactRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.GetContactByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ContactDto>> CreateContactAsync(CreateContactDto createContactDto)
        {
            try
            {
                var contact = _mapper.Map<Contact>(createContactDto);
                await _unitOfWork.Contacts.AddAsync(contact);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var contactWithNav = await _unitOfWork.Contacts
                    .Query()
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == contact.Id && !c.IsDeleted);

                if (contactWithNav == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var contactDto = _mapper.Map<ContactDto>(contactWithNav);

                return ApiResponse<ContactDto>.SuccessResult(contactDto, _localizationService.GetLocalizedString("ContactService.ContactCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.CreateContactExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ContactDto>> UpdateContactAsync(long id, UpdateContactDto updateContactDto)
        {
            try
            {
                // Get tracked entity for update
                var contact = await _unitOfWork.Contacts.GetByIdForUpdateAsync(id);
                if (contact == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateContactDto, contact);
                await _unitOfWork.Contacts.UpdateAsync(contact);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping (read-only)
                var contactWithNav = await _unitOfWork.Contacts
                    .Query()
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (contactWithNav == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var contactDto = _mapper.Map<ContactDto>(contactWithNav);

                return ApiResponse<ContactDto>.SuccessResult(contactDto, _localizationService.GetLocalizedString("ContactService.ContactUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.UpdateContactExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteContactAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.Contacts.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ContactService.ContactDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.DeleteContactExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
