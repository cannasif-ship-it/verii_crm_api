using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

var apiProjectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
var scriptPath = Path.Combine(apiProjectDir, "docs", "manual-migrations", "20260324_AddSalesRepDailyUsageReport.sql");
var appsettingsPath = Path.Combine(apiProjectDir, "appsettings.json");

if (!File.Exists(scriptPath))
{
    Console.Error.WriteLine($"SQL script not found: {scriptPath}");
    return 1;
}

if (!File.Exists(appsettingsPath))
{
    Console.Error.WriteLine($"appsettings.json not found: {appsettingsPath}");
    return 1;
}

var config = new ConfigurationBuilder()
    .AddJsonFile(appsettingsPath, optional: false)
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("DefaultConnection was not found.");
    return 1;
}

var sql = await File.ReadAllTextAsync(scriptPath);

await using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

if (args.Contains("--verify", StringComparer.OrdinalIgnoreCase))
{
    const string verifySql = """
SELECT TOP (5)
    report_date,
    user_id,
    user_name,
    user_email,
    scanned_card_count,
    created_contact_count,
    created_company_count
FROM dbo.RII_FN_USER_DAILY_CARD_PERFORMANCE(NULL, NULL, NULL, NULL)
ORDER BY report_date DESC, user_id ASC;

SELECT TOP (1)
    Id,
    Name,
    ConnectionKey,
    DataSourceType,
    DataSourceName
FROM RII_REPORT_DEFINITIONS
WHERE IsDeleted = 0
  AND Name = N'Günlük Kullanıcı Kart Performans Raporu'
ORDER BY Id DESC;

SELECT TOP (5)
    ra.ReportDefinitionId,
    ra.UserId,
    u.Email,
    ra.IsDeleted
FROM RII_REPORT_ASSIGNMENTS ra
INNER JOIN RII_USERS u ON u.Id = ra.UserId
WHERE ra.IsDeleted = 0
  AND ra.ReportDefinitionId = 5;
""";

    await using var verifyCommand = connection.CreateCommand();
    verifyCommand.CommandText = verifySql;
    verifyCommand.CommandTimeout = 180;
    await using var verifyReader = await verifyCommand.ExecuteReaderAsync();
    do
    {
        while (await verifyReader.ReadAsync())
        {
            for (var i = 0; i < verifyReader.FieldCount; i++)
            {
                var value = verifyReader.IsDBNull(i) ? "NULL" : verifyReader.GetValue(i)?.ToString();
                Console.WriteLine($"{verifyReader.GetName(i)}={value}");
            }
            Console.WriteLine("---");
        }
    } while (await verifyReader.NextResultAsync());

    Console.WriteLine("Verification completed.");
    return 0;
}

await using var command = connection.CreateCommand();
command.CommandText = sql;
command.CommandTimeout = 180;

await using var reader = await command.ExecuteReaderAsync();
do
{
    while (await reader.ReadAsync())
    {
        for (var i = 0; i < reader.FieldCount; i++)
        {
            var value = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i)?.ToString();
            Console.WriteLine($"{reader.GetName(i)}={value}");
        }
    }
} while (await reader.NextResultAsync());

Console.WriteLine("SQL script applied successfully.");
return 0;
