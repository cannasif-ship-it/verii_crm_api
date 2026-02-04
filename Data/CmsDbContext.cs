using Microsoft.EntityFrameworkCore;
using crm_api.Models;
using crm_api.Models.Notification;
using crm_api.Models.ReportBuilder;
using crm_api.Data.Configurations;
using depoWebAPI.Models;

namespace crm_api.Data
{
    public class CmsDbContext : DbContext
    {
        public CmsDbContext(DbContextOptions<CmsDbContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<ProductPricing> ProductPricings { get; set; }
        public DbSet<ProductPricingGroupBy> ProductPricingGroupBys { get; set; }
        public DbSet<UserDiscountLimit> UserDiscountLimits { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationLine> QuotationLines { get; set; }
        public DbSet<QuotationExchangeRate> QuotationExchangeRates { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<DemandLine> DemandLines { get; set; }
        public DbSet<DemandExchangeRate> DemandExchangeRates { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderExchangeRate> OrderExchangeRates { get; set; }
        public DbSet<UserAuthority> UserAuthorities { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
        public DbSet<PricingRuleHeader> PricingRuleHeaders { get; set; }
        public DbSet<PricingRuleLine> PricingRuleLines { get; set; }
        public DbSet<PricingRuleSalesman> PricingRuleSalesmen { get; set; }
        public DbSet<DocumentSerialType> DocumentSerialTypes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockDetail> StockDetails { get; set; }
        public DbSet<StockImage> StockImages { get; set; }
        public DbSet<StockRelation> StockRelations { get; set; }
        public DbSet<ApprovalAction> ApprovalActions { get; set; }
        public DbSet<ApprovalFlow> ApprovalFlows { get; set; }
        public DbSet<ApprovalFlowStep> ApprovalFlowSteps { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<ApprovalRoleGroup> ApprovalRoleGroups { get; set; }
        public DbSet<ApprovalRole> ApprovalRoles { get; set; }
        public DbSet<ApprovalUserRole> ApprovalUserRoles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ReportTemplate> ReportTemplates { get; set; }
        public DbSet<ReportDefinition> ReportDefinitions { get; set; }
        public DbSet<SmtpSetting> SmtpSettings { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kur function yapılandırması - Key yok
            modelBuilder.Entity<RII_FN_KUR>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("__EFMigrationsHistory_FN_KUR", t => t.ExcludeFromMigrations());
                entity.ToFunction("RII_FN_KUR");
            });

            // 2SHIPPING function yapılandırması - Key yok
            modelBuilder.Entity<RII_FN_2SHIPPING>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("__EFMigrationsHistory_FN_2SHIPPING", t => t.ExcludeFromMigrations());
                entity.ToFunction("RII_FN_2SHIPPING");
            });

            // Stok group function yapılandırması - Key yok
            modelBuilder.Entity<RII_STGROUP>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("__EFMigrationsHistory_STGROUP", t => t.ExcludeFromMigrations());
                entity.ToFunction("RII_STGROUP");
            });

            // Stok function yapılandırması - Key yok
            modelBuilder.Entity<RII_FN_STOK>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("__EFMigrationsHistory_FN_STOK", t => t.ExcludeFromMigrations());
                entity.ToFunction("RII_FN_STOK");

            });

            // Apply all configurations from the Configurations folder
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CmsDbContext).Assembly);
        }
    }
}
