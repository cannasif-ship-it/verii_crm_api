using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crm_api.Data;
using Hangfire;
using Hangfire.Storage;

namespace crm_api.Modules.System.Api
{
    [ApiController]
    [Route("api/hangfire")]
    [Authorize]
    public class HangfireController : ControllerBase
    {
        private readonly CmsDbContext _db;

        public HangfireController(CmsDbContext db)
        {
            _db = db;
        }

        [HttpGet("recurring-jobs")]
        public IActionResult GetRecurringJobs()
        {
            using var connection = JobStorage.Current.GetConnection();
            var jobs = connection.GetRecurringJobs()
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    JobName = x.Job?.Type?.Name ?? x.Id,
                    Method = x.Job?.Method?.Name,
                    Cron = x.Cron,
                    Queue = x.Queue,
                    NextExecution = x.NextExecution?.ToString("o"),
                    LastExecution = x.LastExecution?.ToString("o"),
                    LastJobId = x.LastJobId,
                    Error = x.Error,
                })
                .ToList();

            return Ok(new
            {
                Items = jobs,
                Total = jobs.Count,
                Timestamp = DateTime.UtcNow,
            });
        }

        [HttpPost("recurring-jobs/{jobId}/trigger")]
        public IActionResult TriggerRecurringJob([FromRoute] string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return BadRequest(new { Message = "Job id is required." });
            }

            using var connection = JobStorage.Current.GetConnection();
            var exists = connection.GetRecurringJobs()
                .Any(x => string.Equals(x.Id, jobId, StringComparison.OrdinalIgnoreCase));

            if (!exists)
            {
                return NotFound(new { Message = "Recurring job not found." });
            }

            RecurringJob.TriggerJob(jobId);

            return Ok(new
            {
                JobId = jobId,
                TriggeredAt = DateTime.UtcNow,
                Message = "Recurring job triggered successfully.",
            });
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var failed = await _db.JobFailureLogs
                .AsNoTracking()
                .CountAsync();

            return Ok(new
            {
                Enqueued = 0,
                Processing = 0,
                Scheduled = 0,
                Succeeded = 0,
                Failed = failed,
                Deleted = 0,
                Servers = 0,
                Queues = 1,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("failed")]
        public Task<IActionResult> GetFailed([FromQuery] int from = 0, [FromQuery] int count = 20)
        {
            // Backward-compatible endpoint: artık sadece RII_JOB_FAILURE_LOG'dan okunur.
            return GetFailuresFromDb(from, count);
        }

        [HttpGet("failures-from-db")]
        public async Task<IActionResult> GetFailuresFromDb([FromQuery] int from = 0, [FromQuery] int count = 50)
        {
            if (from < 0) from = 0;
            if (count <= 0) count = 50;
            if (count > 200) count = 200;

            var items = await _db.JobFailureLogs
                .AsNoTracking()
                .OrderByDescending(x => x.FailedAt)
                .Skip(from)
                .Take(count)
                .Select(x => new
                {
                    x.JobId,
                    x.JobName,
                    FailedAt = x.FailedAt.ToString("o"),
                    State = "Failed",
                    Reason = x.ExceptionMessage ?? x.Reason,
                    x.ExceptionType,
                    x.RetryCount,
                    x.Queue
                })
                .ToListAsync();

            var total = await _db.JobFailureLogs.CountAsync();

            return Ok(new
            {
                Items = items,
                Total = total,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("dead-letter")]
        public async Task<IActionResult> GetDeadLetter([FromQuery] int from = 0, [FromQuery] int count = 20)
        {
            if (from < 0) from = 0;
            if (count <= 0) count = 20;
            if (count > 200) count = 200;

            var deadLetterQuery = _db.JobFailureLogs
                .AsNoTracking()
                .Where(x => x.Queue == "dead-letter");

            var items = await deadLetterQuery
                .OrderByDescending(x => x.FailedAt)
                .Skip(from)
                .Take(count)
                .Select(x => new
                {
                    x.JobId,
                    x.JobName,
                    EnqueuedAt = x.FailedAt.ToString("o"),
                    State = "Enqueued",
                    Reason = x.ExceptionMessage ?? x.Reason
                })
                .ToListAsync();

            var total = await deadLetterQuery.CountAsync();

            return Ok(new
            {
                Queue = "dead-letter",
                Enqueued = total,
                Items = items,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
