using crm_api.Shared.Host.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuredCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?.Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Select(origin => origin.Trim().TrimEnd('/'))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray()
    ?? Array.Empty<string>();

builder.Services.AddCrmApiWebApi(builder.Configuration, builder.Environment, configuredCorsOrigins);

var app = builder.Build();
app.UseCrmApiWebApi(configuredCorsOrigins);

app.Run();

public partial class Program { }
