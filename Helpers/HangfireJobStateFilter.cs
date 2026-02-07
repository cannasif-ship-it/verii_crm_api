using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;

namespace crm_api.Helpers
{
    public class HangfireJobStateFilter : IApplyStateFilter
    {
        private readonly ILogger<HangfireJobStateFilter> _logger;

        public HangfireJobStateFilter(ILogger<HangfireJobStateFilter> logger)
        {
            _logger = logger;
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            var jobId = context.BackgroundJob?.Id ?? "unknown";
            var job = context.BackgroundJob?.Job;
            var jobName = job == null ? "unknown" : $"{job.Type.Name}.{job.Method.Name}";

            if (context.NewState is FailedState failedState)
            {
                var retryCount = context.GetJobParameter<int>("RetryCount");
                _logger.LogError(
                    failedState.Exception,
                    "Hangfire job failed. JobId: {JobId}, Job: {JobName}, RetryCount: {RetryCount}, Reason: {Reason}",
                    jobId,
                    jobName,
                    retryCount,
                    failedState.Reason);
            }
            else if (context.NewState is SucceededState succeededState)
            {
                _logger.LogInformation(
                    "Hangfire job succeeded. JobId: {JobId}, Job: {JobName}, Latency: {Latency}, Duration: {Duration}",
                    jobId,
                    jobName,
                    succeededState.Latency,
                    succeededState.PerformanceDuration);
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}
