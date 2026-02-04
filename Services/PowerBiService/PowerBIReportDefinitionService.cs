using AutoMapper;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Interfaces;
using crm_api.Models.PowerBi;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using crm_api.DTOs.PowerBi;

namespace crm_api.Services
{
    public class PowerBIReportDefinitionService : IPowerBIReportDefinitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PowerBIReportDefinitionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<PowerBIReportDefinitionGetDto>>> GetAllAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var query = _unitOfWork.PowerBIReportDefinitions
                    .Query()
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(PowerBIReportDefinition.Id);
                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<PowerBIReportDefinitionGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<PowerBIReportDefinitionGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<PowerBIReportDefinitionGetDto>>.SuccessResult(
                    pagedResponse,
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PowerBIReportDefinitionGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.InternalServerError"),
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.GetAllExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PowerBIReportDefinitionGetDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PowerBIReportDefinitions.GetByIdAsync(id);
                if (entity == null)
                {
                    return ApiResponse<PowerBIReportDefinitionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionNotFound"),
                        _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var entityWithNav = await _unitOfWork.PowerBIReportDefinitions
                    .Query()
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id);

                var dto = _mapper.Map<PowerBIReportDefinitionGetDto>(entityWithNav ?? entity);

                return ApiResponse<PowerBIReportDefinitionGetDto>.SuccessResult(
                    dto,
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PowerBIReportDefinitionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.InternalServerError"),
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.GetByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PowerBIReportDefinitionGetDto>> CreateAsync(CreatePowerBIReportDefinitionDto dto)
        {
            try
            {
                var entity = _mapper.Map<PowerBIReportDefinition>(dto);

                await _unitOfWork.PowerBIReportDefinitions.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                // reload (audit nav için)
                var createdEntity = await _unitOfWork.PowerBIReportDefinitions
                    .Query()
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);

                var resultDto = _mapper.Map<PowerBIReportDefinitionGetDto>(createdEntity ?? entity);

                return ApiResponse<PowerBIReportDefinitionGetDto>.SuccessResult(
                    resultDto,
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PowerBIReportDefinitionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.InternalServerError"),
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.CreateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PowerBIReportDefinitionGetDto>> UpdateAsync(long id, UpdatePowerBIReportDefinitionDto dto)
        {
            try
            {
                // tracked entity lazım
                var entity = await _unitOfWork.PowerBIReportDefinitions.GetByIdForUpdateAsync(id);
                if (entity == null)
                {
                    return ApiResponse<PowerBIReportDefinitionGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionNotFound"),
                        _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(dto, entity);

                await _unitOfWork.PowerBIReportDefinitions.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var updatedEntity = await _unitOfWork.PowerBIReportDefinitions
                    .Query()
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id);

                var resultDto = _mapper.Map<PowerBIReportDefinitionGetDto>(updatedEntity ?? entity);

                return ApiResponse<PowerBIReportDefinitionGetDto>.SuccessResult(
                    resultDto,
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PowerBIReportDefinitionGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.InternalServerError"),
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.UpdateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PowerBIReportDefinitions.GetByIdAsync(id);
                if (entity == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionNotFound"),
                        _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.PowerBIReportDefinitions.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.ReportDefinitionDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.InternalServerError"),
                    _localizationService.GetLocalizedString("PowerBIReportDefinitionService.DeleteExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
