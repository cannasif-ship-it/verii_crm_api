using crm_api.Helpers;
using crm_api.Modules.System.Application.Services;
using crm_api.Modules.Integrations.Infrastructure.Security;
using crm_api.Repositories;
using crm_api.Shared.Common.Application;
using crm_api.Shared.Common.Application.Common;
using crm_api.Shared.Infrastructure.Abstractions;
using crm_api.Shared.Infrastructure.Services.Localization;
using crm_api.UnitOfWork;
using Infrastructure.BackgroundJobs.Interfaces;

namespace crm_api.Shared.Host.WebApi.Extensions;

public static class ModuleServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructureModule(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, crm_api.UnitOfWork.UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IGeocodingService, GeocodingService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IEncryptionService, AesGcmEncryptionService>();

        foreach (var resourceType in typeof(AssemblyMarker).Assembly
                     .GetTypes()
                     .Where(type => typeof(ILocalizationResource).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false }))
        {
            services.AddSingleton(typeof(ILocalizationResource), resourceType);
        }

        services.AddSingleton<LocalizationRegistry>();
        services.AddScoped<ILocalizationService, PragmaticLocalizationService>();

        return services;
    }

    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IUserSessionCacheService, UserSessionCacheService>();
        services.AddScoped<IUserDetailService, UserDetailService>();

        return services;
    }

    public static IServiceCollection AddAccessControlModule(this IServiceCollection services)
    {
        services.AddScoped<IUserAuthorityService, UserAuthorityService>();
        services.AddScoped<IPermissionAccessService, PermissionAccessService>();
        services.AddScoped<IPermissionDefinitionService, PermissionDefinitionService>();
        services.AddScoped<IPermissionGroupService, PermissionGroupService>();
        services.AddScoped<IUserPermissionGroupService, UserPermissionGroupService>();

        return services;
    }

    public static IServiceCollection AddCustomerModule(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerImageService, CustomerImageService>();
        services.AddScoped<ICustomerTypeService, CustomerTypeService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IDistrictService, DistrictService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IShippingAddressService, ShippingAddressService>();
        services.AddScoped<ICustomer360Service, Customer360Service>();
        services.AddScoped<ICustomerSyncJob, global::Infrastructure.BackgroundJobs.CustomerSyncJob>();

        return services;
    }

    public static IServiceCollection AddQuotationModule(this IServiceCollection services)
    {
        services.AddScoped<IQuotationService, QuotationService>();
        services.AddScoped<ITempQuotattionService, TempQuotattionService>();
        services.AddScoped<IQuotationLineService, QuotationLineService>();
        services.AddScoped<IQuotationExchangeRateService, QuotationExchangeRateService>();
        services.AddScoped<IQuotationNotesService, QuotationNotesService>();
        services.AddScoped<IProductPricingService, ProductPricingService>();
        services.AddScoped<IProductPricingGroupByService, ProductPricingGroupByService>();
        services.AddScoped<IUserDiscountLimitService, UserDiscountLimitService>();
        services.AddScoped<IPaymentTypeService, PaymentTypeService>();

        return services;
    }

    public static IServiceCollection AddDemandModule(this IServiceCollection services)
    {
        services.AddScoped<IDemandService, DemandService>();
        services.AddScoped<IDemandLineService, DemandLineService>();
        services.AddScoped<IDemandExchangeRateService, DemandExchangeRateService>();
        services.AddScoped<IDemandNotesService, DemandNotesService>();

        return services;
    }

    public static IServiceCollection AddOrderModule(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderLineService, OrderLineService>();
        services.AddScoped<IOrderExchangeRateService, OrderExchangeRateService>();
        services.AddScoped<IOrderNotesService, OrderNotesService>();

        return services;
    }

    public static IServiceCollection AddAnalyticsModule(this IServiceCollection services)
    {
        services.AddScoped<ISalesmen360Service, Salesmen360Service>();
        services.AddScoped<IRevenueQualityService, RevenueQualityService>();
        services.AddScoped<INextBestActionService, NextBestActionService>();

        return services;
    }

    public static IServiceCollection AddActivityModule(this IServiceCollection services)
    {
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IActivityImageService, ActivityImageService>();
        services.AddScoped<IActivityTypeService, ActivityTypeService>();
        services.AddScoped<IActivityMeetingTypeService, ActivityMeetingTypeService>();
        services.AddScoped<IActivityTopicPurposeService, ActivityTopicPurposeService>();
        services.AddScoped<IActivityShippingService, ActivityShippingService>();

        return services;
    }

    public static IServiceCollection AddDefinitionsModule(this IServiceCollection services)
    {
        services.AddScoped<ISalesTypeService, SalesTypeService>();
        services.AddScoped<ISalesRepCodeService, SalesRepCodeService>();
        services.AddScoped<ISalesRepCodeUserMatchService, SalesRepCodeUserMatchService>();
        services.AddScoped<global::Infrastructure.BackgroundJobs.Interfaces.ISalesRepCodeSyncJob, global::Infrastructure.BackgroundJobs.SalesRepCodeSyncJob>();
        services.AddScoped<ITitleService, TitleService>();
        services.AddScoped<IDocumentSerialTypeService, DocumentSerialTypeService>();

        return services;
    }

    public static IServiceCollection AddPricingModule(this IServiceCollection services)
    {
        services.AddScoped<IPricingRuleHeaderService, PricingRuleHeaderService>();
        services.AddScoped<IPricingRuleLineService, PricingRuleLineService>();
        services.AddScoped<IPricingRuleSalesmanService, PricingRuleSalesmanService>();

        return services;
    }

    public static IServiceCollection AddStockModule(this IServiceCollection services)
    {
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IStockDetailService, StockDetailService>();
        services.AddScoped<IStockImageService, StockImageService>();
        services.AddScoped<IStockRelationService, StockRelationService>();
        services.AddScoped<IStockSyncJob, global::crm_api.Modules.Stock.Infrastructure.Jobs.StockSyncJob>();
        services.AddScoped<IStockImageBulkImportJob, global::crm_api.Modules.Stock.Infrastructure.Jobs.StockImageBulkImportJob>();

        return services;
    }

    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        services.AddScoped<IProductCatalogService, ProductCatalogService>();

        return services;
    }

    public static IServiceCollection AddApprovalModule(this IServiceCollection services)
    {
        services.AddScoped<IApprovalActionService, ApprovalActionService>();
        services.AddScoped<IApprovalFlowService, ApprovalFlowService>();
        services.AddScoped<IApprovalFlowStepService, ApprovalFlowStepService>();
        services.AddScoped<IApprovalRequestService, ApprovalRequestService>();
        services.AddScoped<IApprovalRoleGroupService, ApprovalRoleGroupService>();
        services.AddScoped<IApprovalRoleService, ApprovalRoleService>();
        services.AddScoped<IApprovalUserRoleService, ApprovalUserRoleService>();

        return services;
    }

    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services)
    {
        services.AddScoped<IErpService, ErpService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ITenantGoogleOAuthSettingsService, TenantGoogleOAuthSettingsService>();
        services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
        services.AddScoped<IGoogleTokenService, GoogleTokenService>();
        services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();
        services.AddScoped<IGoogleIntegrationLogService, GoogleIntegrationLogService>();
        services.AddScoped<IGoogleGmailApiService, GoogleGmailApiService>();
        services.AddScoped<IOutlookEntegrationService, OutlookEntegrationService>();
        services.AddScoped<ISmtpSettingsService, SmtpSettingsService>();
        services.AddScoped<IMailJob, global::Infrastructure.BackgroundJobs.MailJob>();

        return services;
    }

    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }

    public static IServiceCollection AddPdfBuilderModule(this IServiceCollection services)
    {
        services.AddScoped<IPdfReportTemplateValidator, PdfReportTemplateValidator>();
        services.AddScoped<IPdfReportDocumentGeneratorService, PdfReportDocumentGeneratorService>();
        services.AddScoped<IPdfReportTemplateService, PdfReportTemplateService>();
        services.AddScoped<IPdfTemplateAssetService, PdfTemplateAssetService>();
        services.AddScoped<IPdfTablePresetService, PdfTablePresetService>();
        services.AddScoped<IReportTemplateService, ReportTemplateService>();
        services.AddScoped<IReportPdfGeneratorService, ReportPdfGeneratorService>();

        return services;
    }

    public static IServiceCollection AddReportBuilderModule(this IServiceCollection services)
    {
        services.AddScoped<IReportingConnectionService, ReportingConnectionService>();
        services.AddScoped<IReportingCatalogService, ReportingCatalogService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReportPreviewService, ReportPreviewService>();

        return services;
    }

    public static IServiceCollection AddPowerBIModule(this IServiceCollection services)
    {
        services.AddScoped<IPowerBIGroupService, PowerBIGroupService>();
        services.AddScoped<IPowerBIReportDefinitionService, PowerBIReportDefinitionService>();
        services.AddScoped<IPowerBIGroupReportDefinitionService, PowerBIGroupReportDefinitionService>();
        services.AddScoped<IUserPowerBIGroupService, UserPowerBIGroupService>();
        services.AddScoped<IPowerBIConfigurationService, PowerBIConfigurationService>();
        services.AddScoped<IPowerBIEmbedService, PowerBIEmbedService>();
        services.AddScoped<IPowerBIReportSyncService, PowerBIReportSyncService>();
        services.AddScoped<IPowerBIReportRoleMappingService, PowerBIReportRoleMappingService>();

        return services;
    }

    public static IServiceCollection AddSystemModule(this IServiceCollection services)
    {
        services.AddScoped<IHangfireDeadLetterJob, global::Infrastructure.BackgroundJobs.HangfireDeadLetterJob>();
        services.AddScoped<ISystemSettingsService, SystemSettingsService>();

        return services;
    }
}
