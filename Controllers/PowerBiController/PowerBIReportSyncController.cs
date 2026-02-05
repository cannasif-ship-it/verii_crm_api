using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [Route("api/powerbi/reports")]
    [ApiController]
    [Authorize]
    public class PowerBIReportSyncController : ControllerBase
    {
        private readonly IPowerBIReportSyncService _syncService;

        public PowerBIReportSyncController(IPowerBIReportSyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync([FromQuery] Guid? workspaceId)
        {
            var result = await _syncService.SyncAsync(workspaceId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
