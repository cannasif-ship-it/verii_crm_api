using System.Globalization;
using System.Security.Claims;
using System.Text;
using crm_api.Data;
using crm_api.Helpers;
using crm_api.Hubs;
using crm_api.Infrastructure;
using crm_api.Infrastructure.Filters;
using crm_api.Infrastructure.Startup;
using crm_api.Repositories;
using crm_api.Shared.Common.Application;
using crm_api.Shared.Common.Application.Common;
using crm_api.Shared.Infrastructure.Abstractions;
using crm_api.Shared.Infrastructure.Services.Localization;
using crm_api.Modules.PowerBI.Infrastructure.Options;
using crm_api.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.BackgroundJobs.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace crm_api.Shared.Host.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrmApiWebApi(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment,
        string[] configuredCorsOrigins)
    {
        if (configuredCorsOrigins.Length == 0)
        {
            throw new InvalidOperationException("Cors:AllowedOrigins ayari bos birakilamaz.");
        }

        services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilterAttribute>();
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
        services.AddScoped<ValidationFilterAttribute>();
        services.AddMemoryCache();

        var dataProtectionKeyPath =
            configuration["DataProtection:KeyPath"] ??
            Path.Combine(environment.ContentRootPath, "DataProtectionKeys");
        Directory.CreateDirectory(dataProtectionKeyPath);

        services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeyPath))
            .SetApplicationName("V3RII_CRM");

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });

        services.AddDbContext<CmsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(60);
            });
        });

        services.AddDbContext<ErpCmsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("ErpConnection");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(60);
            });
        });

        services.AddHangfire(hangfire => hangfire
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.Configure<HangfireMonitoringOptions>(
            configuration.GetSection(HangfireMonitoringOptions.SectionName));
        services.Configure<GeocodingOptions>(
            configuration.GetSection(GeocodingOptions.SectionName));
        services.Configure<GoogleOptions>(
            configuration.GetSection(GoogleOptions.SectionName));

        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
        {
            Attempts = 3,
            DelaysInSeconds = new[] { 60, 300, 900 },
            LogEvents = true,
            OnAttemptsExceeded = AttemptsExceededAction.Fail
        });

        services.AddHangfireServer(options =>
        {
            options.Queues = new[] { "default", "dead-letter" };
        });

        services.AddHostedService<AdminBootstrapHostedService>();
        services.AddHostedService<SystemSettingsBootstrapHostedService>();
        services.AddAutoMapper(typeof(AssemblyMarker).Assembly);
        services.AddSharedInfrastructureModule();
        services.AddSystemModule();
        services.AddIdentityModule();
        services.AddAccessControlModule();
        services.AddCustomerModule();
        services.AddQuotationModule();
        services.AddDemandModule();
        services.AddOrderModule();
        services.AddAnalyticsModule();
        services.AddActivityModule();
        services.AddDefinitionsModule();
        services.AddPricingModule();
        services.AddStockModule();
        services.AddCatalogModule();
        services.AddApprovalModule();
        services.AddIntegrationsModule();
        services.AddNotificationModule();
        services.AddPdfBuilderModule();
        services.AddReportBuilderModule();
        services.AddPowerBIModule();

        services.Configure<PdfBuilderOptions>(
            configuration.GetSection(PdfBuilderOptions.SectionName));
        services.PostConfigure<PdfBuilderOptions>(options =>
        {
            if (string.IsNullOrWhiteSpace(options.LocalImageBasePath))
            {
                options.LocalImageBasePath = environment.ContentRootPath;
            }
        });
        services.Configure<crm_api.Infrastructure.AzureAdSettings>(
            configuration.GetSection(crm_api.Infrastructure.AzureAdSettings.SectionName));
        services.Configure<PowerBISettings>(
            configuration.GetSection(PowerBISettings.SectionName));

        services.AddHttpContextAccessor();
        services.AddHttpClient();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("tr-TR"),
                new CultureInfo("de-DE"),
                new CultureInfo("fr-FR"),
                new CultureInfo("es-ES"),
                new CultureInfo("it-IT"),
                new CultureInfo("ar-SA")
            };

            options.DefaultRequestCulture = new RequestCulture("tr-TR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0, new CustomHeaderRequestCultureProvider());
        });

        services.AddCors(options =>
        {
            options.AddPolicy("DevCors", policy =>
            {
                policy.WithOrigins(configuredCorsOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = !environment.IsDevelopment();
            options.SaveToken = true;

            var jwtSecret = configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrWhiteSpace(jwtSecret))
            {
                throw new InvalidOperationException("JwtSettings:SecretKey zorunludur.");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"] ?? "CmsWebApi",
                ValidAudience = configuration["JwtSettings:Audience"] ?? "CmsWebApiUsers",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && (
                        path.StartsWithSegments("/api/authHub") ||
                        path.StartsWithSegments("/authHub") ||
                        path.StartsWithSegments("/api/notificationHub") ||
                        path.StartsWithSegments("/notificationHub")))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    var localizationService = context.HttpContext.RequestServices.GetRequiredService<ILocalizationService>();
                    var claims = context.Principal?.Claims;
                    var userIdClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    if (!long.TryParse(userIdClaim, out var userId))
                    {
                        context.Fail(localizationService.GetLocalizedString("Auth.TokenInvalidMissingUserId"));
                        return;
                    }

                    var sessionClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value
                        ?? claims?.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)?.Value;
                    if (!Guid.TryParse(sessionClaim, out var sessionId))
                    {
                        context.Fail(localizationService.GetLocalizedString("Auth.TokenInvalidMissingSessionId"));
                        return;
                    }

                    try
                    {
                        var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                        var sessionCacheService = context.HttpContext.RequestServices.GetRequiredService<IUserSessionCacheService>();
                        var cacheKey = sessionCacheService.GetCacheKey(sessionId);

                        if (cache.TryGetValue<long>(cacheKey, out var cachedUserId))
                        {
                            if (cachedUserId != userId)
                            {
                                context.Fail(localizationService.GetLocalizedString("Auth.SessionExpiredOrInvalid"));
                            }

                            return;
                        }

                        var restored = await sessionCacheService.RestoreSessionAsync(
                            sessionId,
                            userId,
                            context.HttpContext.RequestAborted);
                        if (!restored)
                        {
                            context.Fail(localizationService.GetLocalizedString("Auth.SessionExpiredOrInvalid"));
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Fail(localizationService.GetLocalizedString("Auth.SessionValidationFailed", ex.Message));
                    }
                }
            };
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CRM Web API",
                Version = "v1",
                Description = "A comprehensive CRM Web API with JWT Authentication",
                Contact = new OpenApiContact
                {
                    Name = "CRM API Team",
                    Email = "support@crmapi.com"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityDefinition("Language", new OpenApiSecurityScheme
            {
                Description = "Language header for localization. Use 'tr' for Turkish or 'en' for English. Example: \"x-language: tr\"",
                Name = "x-language",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKey"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Language"
                        }
                    },
                    new List<string>()
                }
            });

            options.CustomSchemaIds(type => type.FullName);
            options.MapType<IFormFile>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });
            options.ParameterFilter<FileUploadParameterFilter>();
            options.OperationFilter<FileUploadOperationFilter>();
            options.CustomOperationIds(apiDesc => apiDesc.ActionDescriptor.RouteValues["action"]);

            var xmlFile = $"{typeof(ServiceCollectionExtensions).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}
