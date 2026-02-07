using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/hangfire")]
    [Authorize]
    public class HangfireController : ControllerBase
    {
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var stats = monitoringApi.GetStatistics();

            return Ok(new
            {
                stats.Enqueued,
                stats.Processing,
                stats.Scheduled,
                stats.Succeeded,
                stats.Failed,
                stats.Deleted,
                stats.Servers,
                stats.Queues,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
