using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace cms_webapi.Services
{
    public class StockRelationService : IStockRelationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public StockRelationService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<StockRelationDto>> CreateAsync(StockRelationCreateDto relationDto)
        {
            try
            {
                var existingRelation = await _unitOfWork.Repository<StockRelation>()
                    .Query()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => 
                        x.StockId == relationDto.StockId && 
                        x.RelatedStockId == relationDto.RelatedStockId && 
                        !x.IsDeleted);

                if (existingRelation != null)
                {
                    return ApiResponse<StockRelationDto>.ErrorResult(
                        _localizationService.GetLocalizedString("StockRelationService.DuplicateRelation"),
                        _localizationService.GetLocalizedString("StockRelationService.DuplicateRelation"),
                        StatusCodes.Status400BadRequest);
                }

                await _unitOfWork.BeginTransactionAsync();

                var relation = _mapper.Map<StockRelation>(relationDto);
                await _unitOfWork.Repository<StockRelation>().AddAsync(relation);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var relationWithNav = await _unitOfWork.Repository<StockRelation>()
                    .Query()
                    .Include(x => x.Stock)
                    .Include(x => x.RelatedStock)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == relation.Id && !x.IsDeleted);

                var dto = _mapper.Map<StockRelationDto>(relationWithNav);

                return ApiResponse<StockRelationDto>.SuccessResult(
                    dto,
                    _localizationService.GetLocalizedString("StockRelationService.RelationAdded"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<StockRelationDto>.ErrorResult(
                    _localizationService.GetLocalizedString("StockRelationService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockRelationService.CreateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<StockRelationDto>>> GetByStockIdAsync(long stockId)
        {
            try
            {
                var relations = await _unitOfWork.Repository<StockRelation>()
                    .Query()
                    .Where(x => x.StockId == stockId && !x.IsDeleted)
                    .Include(x => x.Stock)
                    .Include(x => x.RelatedStock)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .AsNoTracking()
                    .ToListAsync();

                var dtos = relations.Select(x => _mapper.Map<StockRelationDto>(x)).ToList();

                return ApiResponse<List<StockRelationDto>>.SuccessResult(
                    dtos,
                    _localizationService.GetLocalizedString("StockRelationService.RelationsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StockRelationDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("StockRelationService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockRelationService.GetByStockIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteAsync(long id)
        {
            try
            {
                var relation = await _unitOfWork.Repository<StockRelation>().GetByIdAsync(id);
                if (relation == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("StockRelationService.RelationNotFound"),
                        _localizationService.GetLocalizedString("StockRelationService.RelationNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.Repository<StockRelation>().SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("StockRelationService.RelationRemoved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("StockRelationService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockRelationService.DeleteExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
