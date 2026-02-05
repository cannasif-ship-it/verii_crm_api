using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [Route("api/powerbi")]
    [ApiController]
    [Authorize]
    public class PowerBiEmbedController : ControllerBase
    {
        private readonly IPowerBIEmbedService _embedService;

        public PowerBiEmbedController(IPowerBIEmbedService embedService)
        {
            _embedService = embedService;
        }

        [HttpGet("reports/{id}/embed-config")]
        public async Task<IActionResult> GetEmbedConfig(long id)
        {
            var result = await _embedService.GetEmbedConfigAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
