using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using crm_api.Data;
using crm_api.Interfaces;
using crm_api.Mappings;
using crm_api.Repositories;
using crm_api.Services;
using crm_api.UnitOfWork;
using crm_api.Hubs;
using crm_api.Helpers;
using System.Security.Claims;
using Microsoft.Extensions.FileProviders;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.BackgroundJobs.Interfaces;
using Microsoft.Extensions.Caching.Memory;              // ✅ SMTP için (IMemoryCache)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Centralized validation response
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var localization = context.HttpContext.RequestServices.GetRequiredService<crm_api.Interfaces.ILocalizationService>();
        var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        var response = crm_api.DTOs.ApiResponse<object>.ErrorResult(
            localization.GetLocalizedString("General.ValidationError"),
            localization.GetLocalizedString("General.ValidationError"),
            StatusCodes.Status400BadRequest);
        response.Errors = errors;
        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
    };
});


// ✅ SMTP için: MemoryCache + DataProtection
builder.Services.AddMemoryCache();
builder.Services.AddDataProtection();

// SignalR Configuration
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Entity Framework Configuration - Using SQL Server
builder.Services.AddDbContext<CmsDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(60);
    });
});

// Hangfire Configuration
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

// ERP Database Configuration - Using SQL Server
builder.Services.AddDbContext<ErpCmsDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ErpConnection");
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(60);
    });
});

// AutoMapper Configuration - Automatically discover all mapping profiles in the assembly
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register Core Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register Authentication & Authorization Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserAuthorityService, UserAuthorityService>();

// Register Localization Services
builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Register ERP Services
builder.Services.AddScoped<IErpService, ErpService>();

// Register Customer Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IShippingAddressService, ShippingAddressService>();

// Register Quotation Services
builder.Services.AddScoped<IQuotationService, QuotationService>();
builder.Services.AddScoped<IQuotationLineService, QuotationLineService>();
builder.Services.AddScoped<IQuotationExchangeRateService, QuotationExchangeRateService>();

// Register Demand Services
builder.Services.AddScoped<IDemandService, DemandService>();
builder.Services.AddScoped<IDemandLineService, DemandLineService>();
builder.Services.AddScoped<IDemandExchangeRateService, DemandExchangeRateService>();

// Register Order Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderLineService, OrderLineService>();
builder.Services.AddScoped<IOrderExchangeRateService, OrderExchangeRateService>();

// Register Product Services
builder.Services.AddScoped<IProductPricingService, ProductPricingService>();
builder.Services.AddScoped<IProductPricingGroupByService, ProductPricingGroupByService>();

// Register User Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IUserDiscountLimitService, UserDiscountLimitService>();
builder.Services.AddScoped<IUserDetailService, UserDetailService>();

// Register Activity Services
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IActivityTypeService, ActivityTypeService>();

// Register Payment Services
builder.Services.AddScoped<IPaymentTypeService, PaymentTypeService>();

// Register Title Services
builder.Services.AddScoped<ITitleService, TitleService>();

// Register Pricing Rule Services
builder.Services.AddScoped<IPricingRuleHeaderService, PricingRuleHeaderService>();
builder.Services.AddScoped<IPricingRuleLineService, PricingRuleLineService>();
builder.Services.AddScoped<IPricingRuleSalesmanService, PricingRuleSalesmanService>();

// Register Document Serial Type Services
builder.Services.AddScoped<IDocumentSerialTypeService, DocumentSerialTypeService>();

// Register Stock Services
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockDetailService, StockDetailService>();
builder.Services.AddScoped<IStockImageService, StockImageService>();
builder.Services.AddScoped<IStockRelationService, StockRelationService>();

// Register Approval Services
builder.Services.AddScoped<IApprovalActionService, ApprovalActionService>();
builder.Services.AddScoped<IApprovalFlowService, ApprovalFlowService>();
builder.Services.AddScoped<IApprovalFlowStepService, ApprovalFlowStepService>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestService>();
builder.Services.AddScoped<IApprovalRoleGroupService, ApprovalRoleGroupService>();
builder.Services.AddScoped<IApprovalRoleService, ApprovalRoleService>();
builder.Services.AddScoped<IApprovalUserRoleService, ApprovalUserRoleService>();

// Register Mail Services
builder.Services.AddScoped<IMailService, MailService>();

// ✅ SMTP Settings Service kaydı
builder.Services.AddScoped<ISmtpSettingsService, SmtpSettingsService>();

// Register Background Jobs
builder.Services.AddScoped<Infrastructure.BackgroundJobs.Interfaces.IStockSyncJob, Infrastructure.BackgroundJobs.StockSyncJob>();
builder.Services.AddScoped<Infrastructure.BackgroundJobs.Interfaces.IMailJob, Infrastructure.BackgroundJobs.MailJob>();

// Register File Upload Services
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// Register Report Template Services
builder.Services.AddScoped<IReportTemplateService, ReportTemplateService>();
builder.Services.AddScoped<IReportPdfGeneratorService, ReportPdfGeneratorService>();

// Report Builder (no allowlist; connection + datasource check + preview + CRUD)
builder.Services.AddScoped<IReportingConnectionService, crm_api.Services.ReportBuilderService.ReportingConnectionService>();
builder.Services.AddScoped<IReportingCatalogService, crm_api.Services.ReportBuilderService.ReportingCatalogService>();
builder.Services.AddScoped<IReportService, crm_api.Services.ReportBuilderService.ReportService>();
builder.Services.AddScoped<IReportPreviewService, crm_api.Services.ReportBuilderService.ReportPreviewService>();

// PowerBi CRUD Services
builder.Services.AddScoped<IPowerBIGroupService, PowerBIGroupService>();
builder.Services.AddScoped<IPowerBIReportDefinitionService, PowerBIReportDefinitionService>();
builder.Services.AddScoped<IPowerBIGroupReportDefinitionService, PowerBIGroupReportDefinitionService>();
builder.Services.AddScoped<IUserPowerBIGroupService, UserPowerBIGroupService>();
builder.Services.AddScoped<IPowerBIConfigurationService, PowerBIConfigurationService>();
builder.Services.AddScoped<IPowerBIEmbedService, PowerBIEmbedService>();
builder.Services.AddScoped<IPowerBIReportSyncService, PowerBIReportSyncService>();
builder.Services.AddScoped<IPowerBIReportRoleMappingService, PowerBIReportRoleMappingService>();

// PowerBi / Azure AD options (embed token)
builder.Services.Configure<crm_api.Infrastructure.AzureAdSettings>(
    builder.Configuration.GetSection(crm_api.Infrastructure.AzureAdSettings.SectionName));
builder.Services.Configure<crm_api.Infrastructure.PowerBISettings>(
    builder.Configuration.GetSection(crm_api.Infrastructure.PowerBISettings.SectionName));

// Add HttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();

// Add HttpClient for external requests (e.g., image loading in PDF generation)
builder.Services.AddHttpClient();

// Localization Configuration
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Request Localization Configuration
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("tr-TR"),
        new CultureInfo("de-DE"),
        new CultureInfo("fr-FR"),
        new CultureInfo("es-ES"),
        new CultureInfo("it-IT")
    };

    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Add custom request culture provider for x-language header
    options.RequestCultureProviders.Insert(0, new CustomHeaderRequestCultureProvider());
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "https://crm.v3rii.com"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "CmsWebApi",
        ValidAudience = builder.Configuration["JwtSettings:Audience"] ?? "CmsWebApiUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JwtSettings:SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!")),
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
            var db = context.HttpContext.RequestServices.GetRequiredService<CmsDbContext>();
            var claims = context.Principal?.Claims;
            var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                context.Fail("Token geçersiz: eksik kullanıcı ID");
                return;
            }

            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var accessToken = context.HttpContext.Request.Query["access_token"].FirstOrDefault();

            string? rawToken = null;
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                rawToken = authHeader.Substring("Bearer ".Length).Trim();
            }
            else if (!string.IsNullOrEmpty(accessToken))
            {
                rawToken = accessToken;
            }

            string? tokenHash = null;
            if (!string.IsNullOrEmpty(rawToken))
            {
                using var sha256Hash = System.Security.Cryptography.SHA256.Create();
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
                var builderStr = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builderStr.Append(bytes[i].ToString("x2"));
                }
                tokenHash = builderStr.ToString();
            }

            try
            {
                var session = await db.UserSessions
                    .AsNoTracking()
                    .Where(s => s.UserId.ToString() == userId
                        && s.RevokedAt == null
                        && (tokenHash != null && s.Token == tokenHash))
                    .FirstOrDefaultAsync(context.HttpContext.RequestAborted);

                if (session == null)
                {
                    context.Fail("Token geçersiz veya oturum kapandı");
                }
            }
            catch (Exception ex)
            {
                context.Fail($"Session kontrolü sırasında hata: {ex.Message}");
            }
        }
    };
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
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

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityDefinition("Language", new OpenApiSecurityScheme
    {
        Description = "Language header for localization. Use 'tr' for Turkish or 'en' for English. Example: \"x-language: tr\"",
        Name = "x-language",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    c.CustomSchemaIds(type => type.FullName);

    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    c.ParameterFilter<FileUploadParameterFilter>();
    c.OperationFilter<FileUploadOperationFilter>();

    c.CustomOperationIds(apiDesc => apiDesc.ActionDescriptor.RouteValues["action"]);

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Migrations are intentionally run out-of-band (e.g., dotnet ef database update)

// Configure the HTTP request pipeline.
app.UseCors("DevCors");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Web API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseRouting();

// Static files for uploaded images - wwwroot folder (default)
app.UseStaticFiles();

// Static files for uploads folder (project root/uploads)
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "uploads");
if (Directory.Exists(uploadsPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadsPath),
        RequestPath = "/uploads"
    });
}

// Add Request Localization Middleware
app.UseRequestLocalization();

// Add BranchCode Middleware
app.UseMiddleware<BranchCodeMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Endpoint mapping
app.MapHub<AuthHub>("/authHub");
app.MapHub<crm_api.Hubs.NotificationHub>("/notificationHub");
app.MapControllers();

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

// Register Recurring Jobs
RecurringJob.AddOrUpdate<IStockSyncJob>(
    "erp-stock-sync-job",
    job => job.ExecuteAsync(),
    Cron.MinuteInterval(30));

BackgroundJob.Enqueue<IStockSyncJob>(job => job.ExecuteAsync());

app.Run();
