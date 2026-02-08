namespace Infrastructure.BackgroundJobs.Interfaces
{
    public interface IHangfireDeadLetterJob
    {
        Task ProcessAsync(HangfireDeadLetterPayload payload);
    }
}
