using crm_api.DTOs;
using Microsoft.AspNetCore.Http;

namespace crm_api.Interfaces
{
    public interface ICustomerImageService
    {
        Task<ApiResponse<List<CustomerImageDto>>> UploadImagesAsync(long customerId, List<IFormFile> files, List<string>? imageDescriptions = null);
        Task<ApiResponse<List<CustomerImageDto>>> GetByCustomerIdAsync(long customerId);
        Task<ApiResponse<object>> DeleteAsync(long id);
    }
}
