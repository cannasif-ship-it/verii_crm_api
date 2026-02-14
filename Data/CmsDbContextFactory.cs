using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace crm_api.Data;

public sealed class CmsDbContextFactory : IDesignTimeDbContextFactory<CmsDbContext>
{
    public CmsDbContext CreateDbContext(string[] args)
    {
        var environmentName =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
            "Production";

        var projectRoot = ResolveProjectRoot();
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(projectRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);

        if (string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase))
        {
            configurationBuilder.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false);
        }

        var configuration = configurationBuilder
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Missing connection string: ConnectionStrings:DefaultConnection");
        }

        var optionsBuilder = new DbContextOptionsBuilder<CmsDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new CmsDbContext(optionsBuilder.Options);
    }

    private static string ResolveProjectRoot()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        if (File.Exists(Path.Combine(currentDirectory, "appsettings.json")))
        {
            return currentDirectory;
        }

        var candidateFromAssembly = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
        if (File.Exists(Path.Combine(candidateFromAssembly, "appsettings.json")))
        {
            return candidateFromAssembly;
        }

        throw new InvalidOperationException("Could not resolve project root containing appsettings.json.");
    }
}
