using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IPdfTemplateAssetService
    {
        Task<ApiResponse<PdfTemplateAssetDto>> UploadAsync(IFormFile file, long userId, long? templateId = null);
    }
}
