using Microsoft.AspNetCore.Mvc;
using cms_webapi.Interfaces;
using cms_webapi.DTOs;
using cms_webapi.DTOs.ErpDto;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErpController : ControllerBase
    {
        private readonly IErpService _IErpService;

        public ErpController(IErpService erpService)
        {
            _IErpService = erpService;
        }

        [HttpGet("getAllCustomers")]
        public async Task<ActionResult<ApiResponse<List<CariDto>>>> GetCaris([FromQuery] string? cariKodu = null)
        {
            var result = await _IErpService.GetCarisAsync(cariKodu);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAllProducts")]
        public async Task<ActionResult<ApiResponse<List<StokDto>>>> GetStoks([FromQuery] string? stokKodu = null)
        {
            var result = await _IErpService.GetStoksAsync(stokKodu);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getBranches")]
        public async Task<ActionResult<ApiResponse<List<BranchDto>>>> GetBranches([FromQuery] int? branchNo = null)
        {
            var result = await _IErpService.GetBranchesAsync(branchNo);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getExchangeRate")]
        public async Task<ActionResult<ApiResponse<List<KurDto>>>> GetExchangeRate(
            [FromQuery] DateTime tarih,
            [FromQuery] int fiyatTipi)
        {
            var result = await _IErpService.GetExchangeRateAsync(tarih, fiyatTipi);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getStokGroup")]
        public async Task<ActionResult<ApiResponse<List<StokGroupDto>>>> GetStokGroup([FromQuery] string? grupKodu)
        {
            var result = await _IErpService.GetStokGroupAsync(grupKodu);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getErpShippingAddress")]
        public async Task<ActionResult<ApiResponse<List<ErpShippingAddressDto>>>> GetErpShippingAddress([FromQuery] string customerCode)
        {
            var result = await _IErpService.GetErpShippingAddressAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("health-check")]
        [AllowAnonymous]
        public IActionResult HealthCheckPublic()
        {
            var healthResponse = new { Status = "Healthy", Timestamp = DateTime.UtcNow };
            return StatusCode(200, healthResponse);
        }
    }
}
