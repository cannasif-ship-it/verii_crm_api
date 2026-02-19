using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services
{
    public class CustomerImageService : ICustomerImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;
        private readonly IFileUploadService _fileUploadService;

        public CustomerImageService(
            IUnitOfWork unitOfWork,
            ILocalizationService localizationService,
            IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _fileUploadService = fileUploadService;
        }

        public async Task<ApiResponse<List<CustomerImageDto>>> UploadImagesAsync(long customerId, List<IFormFile> files, List<string>? imageDescriptions = null)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return ApiResponse<List<CustomerImageDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("FileUploadService.FileRequired"),
                        _localizationService.GetLocalizedString("FileUploadService.FileRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                {
                    return ApiResponse<List<CustomerImageDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.BeginTransactionAsync();

                var entities = new List<CustomerImage>();

                for (var i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var description = imageDescriptions != null && i < imageDescriptions.Count
                        ? imageDescriptions[i]
                        : null;

                    var uploadResult = await _fileUploadService.UploadCustomerImageAsync(file, customerId);
                    if (!uploadResult.Success || string.IsNullOrWhiteSpace(uploadResult.Data))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<List<CustomerImageDto>>.ErrorResult(
                            uploadResult.Message ?? _localizationService.GetLocalizedString("FileUploadService.FileUploadError"),
                            uploadResult.ExceptionMessage,
                            uploadResult.StatusCode);
                    }

                    var entity = new CustomerImage
                    {
                        CustomerId = customerId,
                        ImageUrl = uploadResult.Data,
                        ImageDescription = description
                    };

                    await _unitOfWork.CustomerImages.AddAsync(entity);

                    entities.Add(entity);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var uploadedImages = entities.Select(entity => new CustomerImageDto
                {
                    Id = entity.Id,
                    CreatedDate = entity.CreatedDate,
                    UpdatedDate = entity.UpdatedDate,
                    DeletedDate = entity.DeletedDate,
                    IsDeleted = entity.IsDeleted,
                    CustomerId = customerId,
                    CustomerName = customer.CustomerName,
                    ImageUrl = entity.ImageUrl,
                    ImageDescription = entity.ImageDescription
                }).ToList();

                return ApiResponse<List<CustomerImageDto>>.SuccessResult(uploadedImages, "Customer images uploaded.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<List<CustomerImageDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<CustomerImageDto>>> GetByCustomerIdAsync(long customerId)
        {
            try
            {
                var customerExists = await _unitOfWork.Customers.Query(tracking: false)
                    .AnyAsync(x => x.Id == customerId && !x.IsDeleted);

                if (!customerExists)
                {
                    return ApiResponse<List<CustomerImageDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var items = await _unitOfWork.CustomerImages.Query()
                    .Where(x => x.CustomerId == customerId && !x.IsDeleted)
                    .Include(x => x.Customer)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToListAsync();

                var response = items.Select(x => new CustomerImageDto
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate,
                    DeletedDate = x.DeletedDate,
                    IsDeleted = x.IsDeleted,
                    CreatedByFullUser = x.CreatedByUser != null ? $"{x.CreatedByUser.FirstName} {x.CreatedByUser.LastName}".Trim() : null,
                    UpdatedByFullUser = x.UpdatedByUser != null ? $"{x.UpdatedByUser.FirstName} {x.UpdatedByUser.LastName}".Trim() : null,
                    DeletedByFullUser = x.DeletedByUser != null ? $"{x.DeletedByUser.FirstName} {x.DeletedByUser.LastName}".Trim() : null,
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer?.CustomerName,
                    ImageUrl = x.ImageUrl,
                    ImageDescription = x.ImageDescription
                }).ToList();

                return ApiResponse<List<CustomerImageDto>>.SuccessResult(response, "Customer images retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CustomerImageDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.CustomerImages.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        "Customer image not found.",
                        "Customer image not found.",
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.CustomerImages.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(entity.ImageUrl))
                {
                    await _fileUploadService.DeleteCustomerImageAsync(entity.ImageUrl);
                }

                return ApiResponse<object>.SuccessResult(null, "Customer image deleted.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
