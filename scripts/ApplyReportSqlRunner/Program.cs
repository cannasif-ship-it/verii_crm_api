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
    tarih,
    plasiyer,
    user_id,
    user_email,
    okutulan_kart_sayisi,
    olusturulan_kisi_sayisi,
    olusturulan_cari_sayisi
FROM dbo.RII_FN_USER_DAILY_CARD_PERFORMANCE(NULL, NULL, NULL, NULL)
ORDER BY tarih DESC, user_id ASC;

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
