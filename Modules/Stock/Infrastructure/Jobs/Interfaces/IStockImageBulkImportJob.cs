namespace Infrastructure.BackgroundJobs.Interfaces
{
    public interface IStockImageBulkImportJob
    {
        Task ExecuteAsync(string archivePath, string originalFileName);
    }
}
