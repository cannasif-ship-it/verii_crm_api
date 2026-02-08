using Hangfire.Storage.Monitoring;
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
        private readonly IMonitoringApi _monitoringApi;

        public HangfireController()
        {
            _monitoringApi = JobStorage.Current.GetMonitoringApi();
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var stats = _monitoringApi.GetStatistics();

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

        [HttpGet("failed")]
        public IActionResult GetFailed([FromQuery] int from = 0, [FromQuery] int count = 20)
        {
            if (from < 0) from = 0;
            if (count <= 0) count = 20;
            if (count > 100) count = 100;

            var failed = _monitoringApi.FailedJobs(from, count)
                .Select(MapJob)
                .ToList();

            return Ok(new
            {
                Items = failed,
                Total = _monitoringApi.FailedCount(),
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("dead-letter")]
        public IActionResult GetDeadLetter([FromQuery] int from = 0, [FromQuery] int count = 20)
        {
            if (from < 0) from = 0;
            if (count <= 0) count = 20;
            if (count > 100) count = 100;

            var queue = _monitoringApi.Queues().FirstOrDefault(x => x.Name == "dead-letter");
            var jobs = queue == null
                ? new List<object>()
                : _monitoringApi.EnqueuedJobs("dead-letter", from, count)
                    .Select((KeyValuePair<string, EnqueuedJobDto> item) => MapEnqueuedJob(item))
                    .Cast<object>()
                    .ToList();

            return Ok(new
            {
                Queue = "dead-letter",
                Enqueued = queue?.Length ?? 0,
                Items = jobs,
                Timestamp = DateTime.UtcNow
            });
        }

        private static object MapJob(KeyValuePair<string, FailedJobDto> kvp)
        {
            var details = kvp.Value;
            var job = details.Job;

            return new
            {
                JobId = kvp.Key,
                JobName = job == null ? "unknown" : $"{job.Type.Name}.{job.Method.Name}",
                FailedAt = details.FailedAt,
                State = "Failed",
                Reason = details.ExceptionMessage
            };
        }

        private static object MapEnqueuedJob(KeyValuePair<string, EnqueuedJobDto> kvp)
        {
            var details = kvp.Value;
            var job = details?.Job;

            return new
            {
                JobId = kvp.Key,
                JobName = job == null ? "unknown" : $"{job.Type.Name}.{job.Method.Name}",
                EnqueuedAt = details?.EnqueuedAt,
                State = "Enqueued",
                Reason = "dead-letter"
            };
        }
    }
}
