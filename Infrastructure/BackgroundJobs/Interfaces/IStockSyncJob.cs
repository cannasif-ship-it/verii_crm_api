namespace Infrastructure.BackgroundJobs.Interfaces
{
    public interface IStockSyncJob
    {
        Task ExecuteAsync();
    }
}
