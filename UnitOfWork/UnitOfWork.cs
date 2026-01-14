using cms_webapi.Data;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace cms_webapi.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation for managing transactions and repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CmsDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        // Lazy initialization of repositories
        private IGenericRepository<User>? _users;
        private IGenericRepository<Country>? _countries;
        private IGenericRepository<City>? _cities;
        private IGenericRepository<District>? _districts;
        private IGenericRepository<CustomerType>? _customerTypes;
        private IGenericRepository<Customer>? _customers;
        private IGenericRepository<Title>? _titles;
        private IGenericRepository<UserAuthority>? _userAuthorities;
        private IGenericRepository<Contact>? _contacts;
        private IGenericRepository<Activity>? _activities;
        private IGenericRepository<ActivityType>? _activityTypes;
        private IGenericRepository<PaymentType>? _paymentTypes;
        private IGenericRepository<ProductPricing>? _productPricings;
        private IGenericRepository<ProductPricingGroupBy>? _productPricingGroupBys;
        private IGenericRepository<UserDiscountLimit>? _userDiscountLimits;
        private IGenericRepository<ShippingAddress>? _shippingAddresses;
        private IGenericRepository<Quotation>? _quotations;
        private IGenericRepository<QuotationLine>? _quotationLines;
        private IGenericRepository<UserSession>? _userSessions;
        private IGenericRepository<ApprovalWorkflow>? _approvalWorkflows;
        private IGenericRepository<UserHierarchy>? _userHierarchies;
        private IGenericRepository<ApprovalAuthority>? _approvalAuthorities;
        private IGenericRepository<ApprovalRule>? _approvalRules;
        private IGenericRepository<ApprovalQueue>? _approvalQueues;
        private IGenericRepository<QuotationExchangeRate>? _quotationExchangeRates;
        private IGenericRepository<ApprovalTransaction>? _approvalTransactions;
        private IGenericRepository<UserDetail>? _userDetails;
        private IGenericRepository<PricingRuleHeader>? _pricingRuleHeaders;
        private IGenericRepository<PricingRuleLine>? _pricingRuleLines;
        private IGenericRepository<PricingRuleSalesman>? _pricingRuleSalesmen;
        private IGenericRepository<DocumentSerialType>? _documentSerialTypes;
        private IGenericRepository<Stock>? _stocks;
        private IGenericRepository<StockDetail>? _stockDetails;

        public UnitOfWork(CmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _repositories = new ConcurrentDictionary<Type, object>();
        }

        /// <summary>
        /// repository property
        /// </summary>
        public IGenericRepository<User> Users{get{_users ??= new GenericRepository<User>(_context, _httpContextAccessor);return _users;}}
        public IGenericRepository<Country> Countries{get{_countries ??= new GenericRepository<Country>(_context, _httpContextAccessor);return _countries;}}
        public IGenericRepository<City> Cities{get{_cities ??= new GenericRepository<City>(_context, _httpContextAccessor);return _cities;}}
        public IGenericRepository<District> Districts{get{_districts ??= new GenericRepository<District>(_context, _httpContextAccessor);return _districts;}}
        public IGenericRepository<CustomerType> CustomerTypes{get{_customerTypes ??= new GenericRepository<CustomerType>(_context, _httpContextAccessor);return _customerTypes;}} 
        public IGenericRepository<Customer> Customers{get{_customers ??= new GenericRepository<Customer>(_context, _httpContextAccessor);return _customers;}}
        public IGenericRepository<Title> Titles{get{_titles ??= new GenericRepository<Title>(_context, _httpContextAccessor);return _titles;}}
        public IGenericRepository<UserAuthority> UserAuthorities{get{_userAuthorities ??= new GenericRepository<UserAuthority>(_context, _httpContextAccessor);return _userAuthorities;}}
        public IGenericRepository<Contact> Contacts{get{_contacts ??= new GenericRepository<Contact>(_context, _httpContextAccessor);return _contacts;}}
        public IGenericRepository<Activity> Activities{get{_activities ??= new GenericRepository<Activity>(_context, _httpContextAccessor);return _activities;}}
        public IGenericRepository<ActivityType> ActivityTypes{get{_activityTypes ??= new GenericRepository<ActivityType>(_context, _httpContextAccessor);return _activityTypes;}}
        public IGenericRepository<PaymentType> PaymentTypes{get{_paymentTypes ??= new GenericRepository<PaymentType>(_context, _httpContextAccessor);return _paymentTypes;}}

        public IGenericRepository<ProductPricing> ProductPricings{get{_productPricings ??= new GenericRepository<ProductPricing>(_context, _httpContextAccessor);return _productPricings;}}
        public IGenericRepository<ProductPricingGroupBy> ProductPricingGroupBys{get{_productPricingGroupBys ??= new GenericRepository<ProductPricingGroupBy>(_context, _httpContextAccessor);return _productPricingGroupBys;}}
        public IGenericRepository<UserDiscountLimit> UserDiscountLimits{get{_userDiscountLimits ??= new GenericRepository<UserDiscountLimit>(_context, _httpContextAccessor);return _userDiscountLimits;}}
        public IGenericRepository<ShippingAddress> ShippingAddresses{get{_shippingAddresses ??= new GenericRepository<ShippingAddress>(_context, _httpContextAccessor);return _shippingAddresses;}}
        public IGenericRepository<Quotation> Quotations{get{_quotations ??= new GenericRepository<Quotation>(_context, _httpContextAccessor);return _quotations;}}
        public IGenericRepository<QuotationLine> QuotationLines{get{_quotationLines ??= new GenericRepository<QuotationLine>(_context, _httpContextAccessor);return _quotationLines;}}
        public IGenericRepository<UserSession> UserSessions{get{_userSessions ??= new GenericRepository<UserSession>(_context, _httpContextAccessor);return _userSessions;}}
        public IGenericRepository<ApprovalWorkflow> ApprovalWorkflows{get{_approvalWorkflows ??= new GenericRepository<ApprovalWorkflow>(_context, _httpContextAccessor);return _approvalWorkflows;}}
        public IGenericRepository<UserHierarchy> UserHierarchies{get{_userHierarchies ??= new GenericRepository<UserHierarchy>(_context, _httpContextAccessor);return _userHierarchies;}}
        public IGenericRepository<ApprovalAuthority> ApprovalAuthorities{get{_approvalAuthorities ??= new GenericRepository<ApprovalAuthority>(_context, _httpContextAccessor);return _approvalAuthorities;}}
        public IGenericRepository<ApprovalRule> ApprovalRules{get{_approvalRules ??= new GenericRepository<ApprovalRule>(_context, _httpContextAccessor);return _approvalRules;}}
        public IGenericRepository<ApprovalQueue> ApprovalQueues{get{_approvalQueues ??= new GenericRepository<ApprovalQueue>(_context, _httpContextAccessor);return _approvalQueues;}}
        public IGenericRepository<QuotationExchangeRate> QuotationExchangeRates{get{_quotationExchangeRates ??= new GenericRepository<QuotationExchangeRate>(_context, _httpContextAccessor);return _quotationExchangeRates;}}
        public IGenericRepository<ApprovalTransaction> ApprovalTransactions{get{_approvalTransactions ??= new GenericRepository<ApprovalTransaction>(_context, _httpContextAccessor);return _approvalTransactions;}}
        public IGenericRepository<UserDetail> UserDetails{get{_userDetails ??= new GenericRepository<UserDetail>(_context, _httpContextAccessor);return _userDetails;}}
        public IGenericRepository<PricingRuleHeader> PricingRuleHeaders{get{_pricingRuleHeaders ??= new GenericRepository<PricingRuleHeader>(_context, _httpContextAccessor);return _pricingRuleHeaders;}}
        public IGenericRepository<PricingRuleLine> PricingRuleLines{get{_pricingRuleLines ??= new GenericRepository<PricingRuleLine>(_context, _httpContextAccessor);return _pricingRuleLines;}}
        public IGenericRepository<PricingRuleSalesman> PricingRuleSalesmen{get{_pricingRuleSalesmen ??= new GenericRepository<PricingRuleSalesman>(_context, _httpContextAccessor);return _pricingRuleSalesmen;}}
        public IGenericRepository<DocumentSerialType> DocumentSerialTypes{get{_documentSerialTypes ??= new GenericRepository<DocumentSerialType>(_context, _httpContextAccessor);return _documentSerialTypes;}}
        public IGenericRepository<Stock> Stocks{get{_stocks ??= new GenericRepository<Stock>(_context, _httpContextAccessor);return _stocks;}}
        public IGenericRepository<StockDetail> StockDetails{get{_stockDetails ??= new GenericRepository<StockDetail>(_context, _httpContextAccessor);return _stockDetails;}}


        /// <summary>
        /// Get repository for any entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Generic repository for the entity</returns>
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            return (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), 
                _ => new GenericRepository<T>(_context, _httpContextAccessor));
        }

        /// <summary>
        /// Save all changes to the database
        /// </summary>
        /// <returns>Number of affected rows</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // If we have an active transaction, rollback
                if (_transaction != null)
                {
                    await RollbackTransactionAsync();
                }
                throw;
            }
        }

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }

            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                return;
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method
        /// </summary>
        /// <param name="disposing">Whether disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context?.Dispose();
                _disposed = true;
            }
        }
    }
}
