using depoWebAPI.Models;
using cms_webapi.DTOs;
using cms_webapi.DTOs.ErpDto;
using cms_webapi.Data;

namespace cms_webapi.Interfaces
{
    public interface IErpService
    {
        Task<ApiResponse<short>> GetBranchCodeFromContext();
        Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu);
        Task<ApiResponse<List<CariDto>>> GetCarisByCodesAsync(IEnumerable<string> cariKodlari);
        Task<ApiResponse<List<StokFunctionDto>>> GetStoksAsync(string? stokKodu);
        Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null);
        Task<ApiResponse<List<KurDto>>> GetExchangeRateAsync(DateTime tarih, int fiyatTipi);
        Task<ApiResponse<List<ErpShippingAddressDto>>> GetErpShippingAddressAsync(string customerCode);
        Task<ApiResponse<List<StokGroupDto>>> GetStokGroupAsync(string? grupKodu);

        // Health Check
        Task<ApiResponse<object>> HealthCheckAsync();
    }
}
