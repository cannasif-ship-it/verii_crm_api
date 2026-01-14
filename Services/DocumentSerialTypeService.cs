using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace cms_webapi.Services
{
    public class DocumentSerialTypeService : IDocumentSerialTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public DocumentSerialTypeService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<DocumentSerialTypeGetDto>>> GetAllDocumentSerialTypesAsync(PagedRequest request)
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

                var query = _unitOfWork.DocumentSerialTypes
                    .Query()
                    .Where(d => !d.IsDeleted)
                    .Include(d => d.CustomerType)
                    .Include(d => d.SalesRep)
                    .Include(d => d.CreatedByUser)
                    .Include(d => d.UpdatedByUser)
                    .Include(d => d.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(DocumentSerialType.Id);
                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<DocumentSerialTypeGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<DocumentSerialTypeGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<DocumentSerialTypeGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("DocumentSerialTypeService.TypesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<DocumentSerialTypeGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.GetAllTypesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DocumentSerialTypeGetDto>> GetDocumentSerialTypeByIdAsync(long id)
        {
            try
            {
                var documentSerialType = await _unitOfWork.DocumentSerialTypes
                    .Query()
                    .Include(d => d.CustomerType)
                    .Include(d => d.SalesRep)
                    .Include(d => d.CreatedByUser)
                    .Include(d => d.UpdatedByUser)
                    .Include(d => d.DeletedByUser)
                    .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

                if (documentSerialType == null)
                {
                    return ApiResponse<DocumentSerialTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var documentSerialTypeDto = _mapper.Map<DocumentSerialTypeGetDto>(documentSerialType);
                return ApiResponse<DocumentSerialTypeGetDto>.SuccessResult(documentSerialTypeDto, _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<DocumentSerialTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.GetTypeByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DocumentSerialTypeGetDto>> CreateDocumentSerialTypeAsync(DocumentSerialTypeCreateDto createDto)
        {
            try
            {
                var documentSerialType = _mapper.Map<DocumentSerialType>(createDto);
                await _unitOfWork.DocumentSerialTypes.AddAsync(documentSerialType);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var documentSerialTypeWithNav = await _unitOfWork.DocumentSerialTypes
                    .Query()
                    .Include(d => d.CustomerType)
                    .Include(d => d.SalesRep)
                    .Include(d => d.CreatedByUser)
                    .Include(d => d.UpdatedByUser)
                    .Include(d => d.DeletedByUser)
                    .FirstOrDefaultAsync(d => d.Id == documentSerialType.Id && !d.IsDeleted);

                if (documentSerialTypeWithNav == null)
                {
                    return ApiResponse<DocumentSerialTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var documentSerialTypeDto = _mapper.Map<DocumentSerialTypeGetDto>(documentSerialTypeWithNav);
                return ApiResponse<DocumentSerialTypeGetDto>.SuccessResult(documentSerialTypeDto, _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<DocumentSerialTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.CreateTypeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DocumentSerialTypeGetDto>> UpdateDocumentSerialTypeAsync(long id, DocumentSerialTypeUpdateDto updateDto)
        {
            try
            {
                var documentSerialType = await _unitOfWork.DocumentSerialTypes.GetByIdForUpdateAsync(id);
                if (documentSerialType == null)
                {
                    return ApiResponse<DocumentSerialTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateDto, documentSerialType);
                await _unitOfWork.DocumentSerialTypes.UpdateAsync(documentSerialType);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var documentSerialTypeWithNav = await _unitOfWork.DocumentSerialTypes
                    .Query()
                    .Include(d => d.CustomerType)
                    .Include(d => d.SalesRep)
                    .Include(d => d.CreatedByUser)
                    .Include(d => d.UpdatedByUser)
                    .Include(d => d.DeletedByUser)
                    .FirstOrDefaultAsync(d => d.Id == documentSerialType.Id && !d.IsDeleted);

                if (documentSerialTypeWithNav == null)
                {
                    return ApiResponse<DocumentSerialTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var documentSerialTypeDto = _mapper.Map<DocumentSerialTypeGetDto>(documentSerialTypeWithNav);
                return ApiResponse<DocumentSerialTypeGetDto>.SuccessResult(documentSerialTypeDto, _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<DocumentSerialTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.UpdateTypeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteDocumentSerialTypeAsync(long id)
        {
            try
            {
                var documentSerialType = await _unitOfWork.DocumentSerialTypes.GetByIdAsync(id);
                if (documentSerialType == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.DocumentSerialTypes.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("DocumentSerialTypeService.TypeDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("DocumentSerialTypeService.DeleteTypeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
