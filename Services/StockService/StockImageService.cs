using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace crm_api.Services
{
    public class StockImageService : IStockImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IFileUploadService _fileUploadService;

        public StockImageService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _fileUploadService = fileUploadService;
        }

        public async Task<ApiResponse<List<StockImageDto>>> AddImagesAsync(List<StockImageCreateDto> imageDtos)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var images = new List<StockImage>();
                foreach (var imageDto in imageDtos)
                {
                    var image = _mapper.Map<StockImage>(imageDto);
                    images.Add(image);
                    await _unitOfWork.Repository<StockImage>().AddAsync(image);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var dtos = images.Select(x => _mapper.Map<StockImageDto>(x)).ToList();

                return ApiResponse<List<StockImageDto>>.SuccessResult(
                    dtos,
                    _localizationService.GetLocalizedString("StockImageService.ImageAdded"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<List<StockImageDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("StockImageService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockImageService.AddImagesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<StockImageDto>>> UploadImagesAsync(long stockId, List<IFormFile> files, List<string>? altTexts = null)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return ApiResponse<List<StockImageDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("FileUploadService.FileRequired"),
                        _localizationService.GetLocalizedString("FileUploadService.FileRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var stock = await _unitOfWork.Stocks.GetByIdAsync(stockId);
                if (stock == null)
                {
                    return ApiResponse<List<StockImageDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("StockService.StockNotFound"),
                        _localizationService.GetLocalizedString("StockService.StockNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.BeginTransactionAsync();

                var uploadedImages = new List<StockImageDto>();
                var maxSortOrder = await _unitOfWork.Repository<StockImage>()
                    .Query()
                    .Where(x => x.StockId == stockId && !x.IsDeleted)
                    .OrderByDescending(x => x.SortOrder)
                    .Select(x => x.SortOrder)
                    .FirstOrDefaultAsync();

                var currentSortOrder = maxSortOrder + 1;

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var altText = altTexts != null && i < altTexts.Count ? altTexts[i] : null;

                    var uploadResult = await _fileUploadService.UploadStockImageAsync(file, stockId);
                    if (!uploadResult.Success || string.IsNullOrEmpty(uploadResult.Data))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<List<StockImageDto>>.ErrorResult(
                            uploadResult.Message ?? _localizationService.GetLocalizedString("FileUploadService.FileUploadError"),
                            uploadResult.ExceptionMessage,
                            uploadResult.StatusCode);
                    }

                    var image = new StockImage
                    {
                        StockId = stockId,
                        FilePath = uploadResult.Data!,
                        AltText = altText,
                        SortOrder = currentSortOrder++,
                        IsPrimary = false
                    };

                    await _unitOfWork.Repository<StockImage>().AddAsync(image);
                    uploadedImages.Add(_mapper.Map<StockImageDto>(image));
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<List<StockImageDto>>.SuccessResult(
                    uploadedImages,
                    _localizationService.GetLocalizedString("StockImageService.ImageAdded"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<List<StockImageDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("StockImageService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockImageService.UploadImagesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<StockImageDto>>> GetByStockIdAsync(long stockId)
        {
            try
            {
                var images = await _unitOfWork.Repository<StockImage>()
                    .Query()
                    .Where(x => x.StockId == stockId && !x.IsDeleted)
                    .Include(x => x.Stock)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .AsNoTracking()
                    .OrderBy(x => x.SortOrder)
                    .ToListAsync();

                var dtos = images.Select(x => _mapper.Map<StockImageDto>(x)).ToList();

                return ApiResponse<List<StockImageDto>>.SuccessResult(
                    dtos,
                    _localizationService.GetLocalizedString("StockImageService.ImagesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StockImageDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("StockImageService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockImageService.GetByStockIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteAsync(long id)
        {
            try
            {
                var image = await _unitOfWork.Repository<StockImage>().GetByIdAsync(id);
                if (image == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("StockImageService.ImageNotFound"),
                        _localizationService.GetLocalizedString("StockImageService.ImageNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.Repository<StockImage>().SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("StockImageService.ImageDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("StockImageService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockImageService.DeleteExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<StockImageDto>> SetPrimaryImageAsync(long imageId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var image = await _unitOfWork.Repository<StockImage>().GetByIdForUpdateAsync(imageId);
                if (image == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<StockImageDto>.ErrorResult(
                        _localizationService.GetLocalizedString("StockImageService.ImageNotFound"),
                        _localizationService.GetLocalizedString("StockImageService.ImageNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var otherPrimaryImages = await _unitOfWork.Repository<StockImage>()
                    .Query()
                    .Where(x => x.StockId == image.StockId && x.Id != imageId && x.IsPrimary && !x.IsDeleted)
                    .ToListAsync();

                foreach (var otherImage in otherPrimaryImages)
                {
                    otherImage.IsPrimary = false;
                    await _unitOfWork.Repository<StockImage>().UpdateAsync(otherImage);
                }

                image.IsPrimary = true;
                await _unitOfWork.Repository<StockImage>().UpdateAsync(image);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var imageWithNav = await _unitOfWork.Repository<StockImage>()
                    .Query()
                    .Include(x => x.Stock)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == imageId && !x.IsDeleted);

                var dto = _mapper.Map<StockImageDto>(imageWithNav);

                return ApiResponse<StockImageDto>.SuccessResult(
                    dto,
                    _localizationService.GetLocalizedString("StockImageService.PrimaryImageSet"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<StockImageDto>.ErrorResult(
                    _localizationService.GetLocalizedString("StockImageService.InternalServerError"),
                    _localizationService.GetLocalizedString("StockImageService.SetPrimaryExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
