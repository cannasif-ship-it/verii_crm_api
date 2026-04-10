using crm_api.Modules.Integrations.Domain.ReadModels;
using crm_api.Modules.Integrations.Application.Dtos.Erp;
using crm_api.Data;

namespace crm_api.Modules.Integrations.Application.Services
{
    public interface IErpService
    {
        Task<ApiResponse<short>> GetBranchCodeFromContext();
        Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu);
        Task<ApiResponse<List<CariDto>>> GetCarisByCodesAsync(IEnumerable<string> cariKodlari);
        Task<ApiResponse<List<StokFunctionDto>>> GetStoksAsync(string? stokKodu);
        Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null);
        Task<ApiResponse<List<KurDto>>> GetExchangeRateAsync(DateTime tarih, int fiyatTipi);
        Task<ApiResponse<List<ErpCariMovementDto>>> GetCariMovementsAsync(string customerCode);
        Task<ApiResponse<List<ErpCariBalanceDto>>> GetCariBalancesAsync(string customerCode);
        Task<ApiResponse<List<ErpShippingAddressDto>>> GetErpShippingAddressAsync(string customerCode);
        Task<ApiResponse<List<StokGroupDto>>> GetStokGroupAsync(string? grupKodu);
        Task<ApiResponse<List<ProjeDto>>> GetProjectCodesAsync();
        Task<ApiResponse<List<EsnYapMasDto>>> GetEsnYapMasAsync();

        // Health Check
        Task<ApiResponse<object>> HealthCheckAsync();
    }
}
