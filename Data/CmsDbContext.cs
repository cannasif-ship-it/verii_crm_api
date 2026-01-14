using Microsoft.EntityFrameworkCore;
using cms_webapi.Models;
using cms_webapi.Data.Configurations;
using depoWebAPI.Models;

namespace cms_webapi.Data
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
        public DbSet<ApprovalWorkflow> ApprovalWorkflows { get; set; }
        public DbSet<UserHierarchy> UserHierarchies { get; set; }
        public DbSet<ApprovalAuthority> ApprovalAuthorities { get; set; }
        public DbSet<ApprovalRule> ApprovalRules { get; set; }
        public DbSet<ApprovalQueue> ApprovalQueues { get; set; }
        public DbSet<QuotationExchangeRate> QuotationExchangeRates { get; set; }
        public DbSet<ApprovalTransaction> ApprovalTransactions { get; set; }
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





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kur function yapılandırması - Key yok
            modelBuilder.Entity<RII_FN_KUR>(entity =>
            {
                entity.HasNoKey();
                entity.ToFunction("RII_FN_KUR");
                entity.Metadata.SetIsTableExcludedFromMigrations(true);
            });

            // 2SHIPPING function yapılandırması - Key yok
            modelBuilder.Entity<RII_FN_2SHIPPING>(entity =>
            {
                entity.HasNoKey();
                entity.ToFunction("RII_FN_2SHIPPING");
                entity.Metadata.SetIsTableExcludedFromMigrations(true);
            });

            // Stok group function yapılandırması - Key yok
            modelBuilder.Entity<RII_STGROUP>(entity =>
            {
                entity.HasNoKey();
                entity.ToFunction("RII_STGROUP");
                entity.Metadata.SetIsTableExcludedFromMigrations(true);
            });

            // Stok function yapılandırması - Key yok
            modelBuilder.Entity<RII_FN_STOK>(entity =>
            {
                entity.HasNoKey();
                entity.ToFunction("RII_FN_STOK");
                entity.Property(e => e.STOK_KODU).HasMaxLength(35);
                entity.Property(e => e.URETICI_KODU).HasMaxLength(35);
                entity.Property(e => e.STOK_ADI).HasMaxLength(200);
                entity.Property(e => e.GRUP_KODU).HasMaxLength(8);
                entity.Property(e => e.GRUP_ISIM).HasMaxLength(30);
                entity.Property(e => e.KOD_1).HasMaxLength(8);
                entity.Property(e => e.KOD1_ADI).HasMaxLength(30);
                entity.Property(e => e.KOD_2).HasMaxLength(8);
                entity.Property(e => e.KOD2_ADI).HasMaxLength(30);
                entity.Property(e => e.KOD_3).HasMaxLength(8);
                entity.Property(e => e.KOD3_ADI).HasMaxLength(30);
                entity.Property(e => e.KOD_4).HasMaxLength(8);
                entity.Property(e => e.KOD4_ADI).HasMaxLength(30);
                entity.Property(e => e.KOD_5).HasMaxLength(8);
                entity.Property(e => e.KOD5_ADI).HasMaxLength(30);
                entity.Property(e => e.INGISIM).HasMaxLength(100);
                entity.Metadata.SetIsTableExcludedFromMigrations(true);
            });

            // Apply all configurations from the Configurations folder
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CmsDbContext).Assembly);
        }
    }
}
