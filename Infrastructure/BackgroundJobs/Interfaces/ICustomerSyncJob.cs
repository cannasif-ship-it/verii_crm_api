namespace Infrastructure.BackgroundJobs.Interfaces
{
    public interface ICustomerSyncJob
    {
        Task ExecuteAsync();
    }
}
