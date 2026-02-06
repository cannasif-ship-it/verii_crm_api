using crm_api.DTOs;
using crm_api.DTOs.CustomerDto;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services
{
    public class Customer360Service : ICustomer360Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;

        public Customer360Service(IUnitOfWork unitOfWork, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<Customer360OverviewDto>> GetOverviewAsync(long customerId)
        {
            try
            {
                var customerQuery = _unitOfWork.Customers.Query(tracking: false).Where(c => c.Id == customerId && !c.IsDeleted);
                var customer = await customerQuery.FirstOrDefaultAsync();

                if (customer == null)
                {
                    return ApiResponse<Customer360OverviewDto>.ErrorResult(
                        _localizationService.GetLocalizedString("Customer360Service.CustomerNotFound"),
                        _localizationService.GetLocalizedString("Customer360Service.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var profile = MapProfile(customer);

                var contacts = await GetContactsAsync(customerId);
                var shippingAddresses = await GetShippingAddressesAsync(customerId);
                var recentDemands = await GetRecentDemandsAsync(customerId);
                var recentQuotations = await GetRecentQuotationsAsync(customerId);
                var recentOrders = await GetRecentOrdersAsync(customerId);
                var recentActivities = await GetRecentActivitiesAsync(customerId);

                var totalDemands = await _unitOfWork.Demands.Query(tracking: false)
                    .CountAsync(d => d.PotentialCustomerId == customerId && !d.IsDeleted);
                var totalQuotations = await _unitOfWork.Quotations.Query(tracking: false)
                    .CountAsync(q => q.PotentialCustomerId == customerId && !q.IsDeleted);
                var totalOrders = await _unitOfWork.Orders.Query(tracking: false)
                    .CountAsync(o => o.PotentialCustomerId == customerId && !o.IsDeleted);

                var openQuotations = await _unitOfWork.Quotations.Query(tracking: false)
                    .CountAsync(q => q.PotentialCustomerId == customerId && !q.IsDeleted &&
                        q.Status != ApprovalStatus.Approved && q.Status != ApprovalStatus.Rejected);
                var openOrders = await _unitOfWork.Orders.Query(tracking: false)
                    .CountAsync(o => o.PotentialCustomerId == customerId && !o.IsDeleted &&
                        o.Status != ApprovalStatus.Approved && o.Status != ApprovalStatus.Rejected);

                var lastActivityDate = await GetLastActivityDateAsync(customerId);

                var kpis = new Customer360KpiDto
                {
                    TotalDemands = totalDemands,
                    TotalQuotations = totalQuotations,
                    TotalOrders = totalOrders,
                    OpenQuotations = openQuotations,
                    OpenOrders = openOrders,
                    LastActivityDate = lastActivityDate
                };

                var timeline = await BuildTimelineAsync(customerId);

                var overview = new Customer360OverviewDto
                {
                    Profile = profile,
                    Kpis = kpis,
                    Contacts = contacts,
                    ShippingAddresses = shippingAddresses,
                    RecentDemands = recentDemands,
                    RecentQuotations = recentQuotations,
                    RecentOrders = recentOrders,
                    RecentActivities = recentActivities,
                    Timeline = timeline
                };

                return ApiResponse<Customer360OverviewDto>.SuccessResult(
                    overview,
                    _localizationService.GetLocalizedString("Customer360Service.OverviewRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<Customer360OverviewDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Customer360Service.InternalServerError"),
                    _localizationService.GetLocalizedString("Customer360Service.ExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<Customer360AnalyticsSummaryDto>> GetAnalyticsSummaryAsync(long customerId)
        {
            try
            {
                var customerExists = await _unitOfWork.Customers.Query(tracking: false)
                    .AnyAsync(c => c.Id == customerId && !c.IsDeleted);

                if (!customerExists)
                {
                    return ApiResponse<Customer360AnalyticsSummaryDto>.ErrorResult(
                        _localizationService.GetLocalizedString("Customer360Service.CustomerNotFound"),
                        _localizationService.GetLocalizedString("Customer360Service.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var sinceDate = DateTime.UtcNow.AddMonths(-12);

                var last12MonthsOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                    .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted &&
                        (o.OfferDate ?? o.CreatedDate) >= sinceDate)
                    .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m;

                var openQuotationAmount = await _unitOfWork.Quotations.Query(tracking: false)
                    .Where(q => q.PotentialCustomerId == customerId && !q.IsDeleted &&
                        q.Status != ApprovalStatus.Approved && q.Status != ApprovalStatus.Rejected)
                    .SumAsync(q => (decimal?)q.GrandTotal) ?? 0m;

                var openOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                    .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted &&
                        o.Status != ApprovalStatus.Approved && o.Status != ApprovalStatus.Rejected)
                    .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m;

                var activityCount = await _unitOfWork.Activities.Query(tracking: false)
                    .CountAsync(a => a.PotentialCustomerId == customerId && !a.IsDeleted);

                var lastActivityDate = await GetLastActivityDateAsync(customerId);

                var summary = new Customer360AnalyticsSummaryDto
                {
                    Last12MonthsOrderAmount = last12MonthsOrderAmount,
                    OpenQuotationAmount = openQuotationAmount,
                    OpenOrderAmount = openOrderAmount,
                    ActivityCount = activityCount,
                    LastActivityDate = lastActivityDate
                };

                return ApiResponse<Customer360AnalyticsSummaryDto>.SuccessResult(
                    summary,
                    _localizationService.GetLocalizedString("Customer360Service.OverviewRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<Customer360AnalyticsSummaryDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Customer360Service.InternalServerError"),
                    _localizationService.GetLocalizedString("Customer360Service.ExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<Customer360AnalyticsChartsDto>> GetAnalyticsChartsAsync(long customerId, int months = 12)
        {
            try
            {
                var customerExists = await _unitOfWork.Customers.Query(tracking: false)
                    .AnyAsync(c => c.Id == customerId && !c.IsDeleted);

                if (!customerExists)
                {
                    return ApiResponse<Customer360AnalyticsChartsDto>.ErrorResult(
                        _localizationService.GetLocalizedString("Customer360Service.CustomerNotFound"),
                        _localizationService.GetLocalizedString("Customer360Service.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var safeMonths = months <= 0 ? 12 : Math.Min(months, 36);
                var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                var startMonth = currentMonth.AddMonths(-(safeMonths - 1));
                var endExclusive = currentMonth.AddMonths(1);
                var monthLabels = Enumerable.Range(0, safeMonths)
                    .Select(i => startMonth.AddMonths(i).ToString("yyyy-MM"))
                    .ToList();

                var demandDates = await _unitOfWork.Demands.Query(tracking: false)
                    .Where(d => d.PotentialCustomerId == customerId && !d.IsDeleted)
                    .Select(d => d.OfferDate ?? d.CreatedDate)
                    .Where(d => d >= startMonth && d < endExclusive)
                    .ToListAsync();

                var quotationDates = await _unitOfWork.Quotations.Query(tracking: false)
                    .Where(q => q.PotentialCustomerId == customerId && !q.IsDeleted)
                    .Select(q => q.OfferDate ?? q.CreatedDate)
                    .Where(d => d >= startMonth && d < endExclusive)
                    .ToListAsync();

                var orderDates = await _unitOfWork.Orders.Query(tracking: false)
                    .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted)
                    .Select(o => o.OfferDate ?? o.CreatedDate)
                    .Where(d => d >= startMonth && d < endExclusive)
                    .ToListAsync();

                var demandByMonth = demandDates
                    .GroupBy(d => d.ToString("yyyy-MM"))
                    .ToDictionary(g => g.Key, g => g.Count());

                var quotationByMonth = quotationDates
                    .GroupBy(d => d.ToString("yyyy-MM"))
                    .ToDictionary(g => g.Key, g => g.Count());

                var orderByMonth = orderDates
                    .GroupBy(d => d.ToString("yyyy-MM"))
                    .ToDictionary(g => g.Key, g => g.Count());

                var monthlyTrend = monthLabels
                    .Select(label => new Customer360MonthlyTrendItemDto
                    {
                        Month = label,
                        DemandCount = demandByMonth.TryGetValue(label, out var dCount) ? dCount : 0,
                        QuotationCount = quotationByMonth.TryGetValue(label, out var qCount) ? qCount : 0,
                        OrderCount = orderByMonth.TryGetValue(label, out var oCount) ? oCount : 0
                    })
                    .ToList();

                var distribution = new Customer360DistributionDto
                {
                    DemandCount = await _unitOfWork.Demands.Query(tracking: false)
                        .CountAsync(d => d.PotentialCustomerId == customerId && !d.IsDeleted),
                    QuotationCount = await _unitOfWork.Quotations.Query(tracking: false)
                        .CountAsync(q => q.PotentialCustomerId == customerId && !q.IsDeleted),
                    OrderCount = await _unitOfWork.Orders.Query(tracking: false)
                        .CountAsync(o => o.PotentialCustomerId == customerId && !o.IsDeleted)
                };

                var last12MonthsStart = currentMonth.AddMonths(-11);

                var amountComparison = new Customer360AmountComparisonDto
                {
                    Last12MonthsOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                        .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted &&
                            (o.OfferDate ?? o.CreatedDate) >= last12MonthsStart &&
                            (o.OfferDate ?? o.CreatedDate) < endExclusive)
                        .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m,
                    OpenQuotationAmount = await _unitOfWork.Quotations.Query(tracking: false)
                        .Where(q => q.PotentialCustomerId == customerId && !q.IsDeleted &&
                            q.Status != ApprovalStatus.Approved && q.Status != ApprovalStatus.Rejected)
                        .SumAsync(q => (decimal?)q.GrandTotal) ?? 0m,
                    OpenOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                        .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted &&
                            o.Status != ApprovalStatus.Approved && o.Status != ApprovalStatus.Rejected)
                        .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m
                };

                var charts = new Customer360AnalyticsChartsDto
                {
                    MonthlyTrend = monthlyTrend,
                    Distribution = distribution,
                    AmountComparison = amountComparison
                };

                return ApiResponse<Customer360AnalyticsChartsDto>.SuccessResult(
                    charts,
                    _localizationService.GetLocalizedString("Customer360Service.OverviewRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<Customer360AnalyticsChartsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Customer360Service.InternalServerError"),
                    _localizationService.GetLocalizedString("Customer360Service.ExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        private static Customer360ProfileDto MapProfile(Customer c)
        {
            return new Customer360ProfileDto
            {
                Id = c.Id,
                CustomerCode = c.CustomerCode,
                Name = c.CustomerName,
                TaxNumber = c.TaxNumber,
                TaxOffice = c.TaxOffice,
                TcknNumber = c.TcknNumber,
                Email = c.Email,
                Phone = c.Phone1,
                Phone2 = c.Phone2,
                Website = c.Website,
                Address = c.Address,
                SalesRepCode = c.SalesRepCode,
                GroupCode = c.GroupCode,
                CreditLimit = c.CreditLimit,
                IsERPIntegrated = c.IsERPIntegrated,
                LastSyncDate = c.LastSyncDate
            };
        }

        private async Task<List<Customer360SimpleItemDto>> GetContactsAsync(long customerId)
        {
            return await _unitOfWork.Contacts.Query(tracking: false)
                .Where(c => c.CustomerId == customerId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedDate)
                .Take(10)
                .Select(c => new Customer360SimpleItemDto
                {
                    Id = c.Id,
                    Title = c.FullName,
                    Subtitle = c.Email ?? c.Phone,
                    Status = null,
                    Amount = null,
                    Date = c.CreatedDate
                })
                .ToListAsync();
        }

        private async Task<List<Customer360SimpleItemDto>> GetShippingAddressesAsync(long customerId)
        {
            return await _unitOfWork.ShippingAddresses.Query(tracking: false)
                .Where(s => s.CustomerId == customerId && !s.IsDeleted)
                .OrderByDescending(s => s.CreatedDate)
                .Take(10)
                .Select(s => new Customer360SimpleItemDto
                {
                    Id = s.Id,
                    Title = s.Address,
                    Subtitle = s.ContactPerson ?? s.Phone,
                    Status = null,
                    Amount = null,
                    Date = s.CreatedDate
                })
                .ToListAsync();
        }

        private async Task<List<Customer360SimpleItemDto>> GetRecentDemandsAsync(long customerId)
        {
            return await _unitOfWork.Demands.Query(tracking: false)
                .Where(d => d.PotentialCustomerId == customerId && !d.IsDeleted)
                .OrderByDescending(d => d.OfferDate ?? d.CreatedDate)
                .Take(10)
                .Select(d => new Customer360SimpleItemDto
                {
                    Id = d.Id,
                    Title = d.OfferNo ?? d.Id.ToString(),
                    Subtitle = d.Description,
                    Status = d.Status.HasValue ? d.Status.ToString() : null,
                    Amount = d.GrandTotal,
                    Date = d.OfferDate ?? d.CreatedDate
                })
                .ToListAsync();
        }

        private async Task<List<Customer360SimpleItemDto>> GetRecentQuotationsAsync(long customerId)
        {
            return await _unitOfWork.Quotations.Query(tracking: false)
                .Where(q => q.PotentialCustomerId == customerId && !q.IsDeleted)
                .OrderByDescending(q => q.OfferDate ?? q.CreatedDate)
                .Take(10)
                .Select(q => new Customer360SimpleItemDto
                {
                    Id = q.Id,
                    Title = q.OfferNo ?? q.Id.ToString(),
                    Subtitle = q.Description,
                    Status = q.Status.HasValue ? q.Status.ToString() : null,
                    Amount = q.GrandTotal,
                    Date = q.OfferDate ?? q.CreatedDate
                })
                .ToListAsync();
        }

        private async Task<List<Customer360SimpleItemDto>> GetRecentOrdersAsync(long customerId)
        {
            return await _unitOfWork.Orders.Query(tracking: false)
                .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted)
                .OrderByDescending(o => o.OfferDate ?? o.CreatedDate)
                .Take(10)
                .Select(o => new Customer360SimpleItemDto
                {
                    Id = o.Id,
                    Title = o.OfferNo ?? o.Id.ToString(),
                    Subtitle = o.Description,
                    Status = o.Status.HasValue ? o.Status.ToString() : null,
                    Amount = o.GrandTotal,
                    Date = o.OfferDate ?? o.CreatedDate
                })
                .ToListAsync();
        }

        private async Task<List<Customer360SimpleItemDto>> GetRecentActivitiesAsync(long customerId)
        {
            return await _unitOfWork.Activities.Query(tracking: false)
                .Where(a => a.PotentialCustomerId == customerId && !a.IsDeleted)
                .OrderByDescending(a => a.ActivityDate ?? a.CreatedDate)
                .Take(10)
                .Select(a => new Customer360SimpleItemDto
                {
                    Id = a.Id,
                    Title = a.Subject,
                    Subtitle = a.Description,
                    Status = a.Status,
                    Amount = null,
                    Date = a.ActivityDate ?? a.CreatedDate
                })
                .ToListAsync();
        }

        private async Task<DateTime?> GetLastActivityDateAsync(long customerId)
        {
            var demandDate = await _unitOfWork.Demands.Query(tracking: false)
                .Where(d => d.PotentialCustomerId == customerId && !d.IsDeleted)
                .Select(d => (DateTime?)(d.OfferDate ?? d.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var quotationDate = await _unitOfWork.Quotations.Query(tracking: false)
                .Where(q => q.PotentialCustomerId == customerId && !q.IsDeleted)
                .Select(q => (DateTime?)(q.OfferDate ?? q.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var orderDate = await _unitOfWork.Orders.Query(tracking: false)
                .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted)
                .Select(o => (DateTime?)(o.OfferDate ?? o.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var activityDate = await _unitOfWork.Activities.Query(tracking: false)
                .Where(a => a.PotentialCustomerId == customerId && !a.IsDeleted)
                .Select(a => (DateTime?)(a.ActivityDate ?? a.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var dates = new[] { demandDate, quotationDate, orderDate, activityDate }.Where(d => d.HasValue).Select(d => d!.Value).ToList();
            return dates.Count > 0 ? dates.Max() : null;
        }

        private async Task<List<Customer360TimelineItemDto>> BuildTimelineAsync(long customerId)
        {
            var demands = await _unitOfWork.Demands.Query(tracking: false)
                .Where(d => d.PotentialCustomerId == customerId && !d.IsDeleted)
                .Select(d => new Customer360TimelineItemDto
                {
                    Type = "Demand",
                    ItemId = d.Id,
                    Title = d.OfferNo ?? d.Id.ToString(),
                    Status = d.Status.HasValue ? d.Status.ToString() : null,
                    Amount = d.GrandTotal,
                    Date = d.OfferDate ?? d.CreatedDate
                })
                .ToListAsync();

            var quotations = await _unitOfWork.Quotations.Query(tracking: false)
                .Where(q => q.PotentialCustomerId == customerId && !q.IsDeleted)
                .Select(q => new Customer360TimelineItemDto
                {
                    Type = "Quotation",
                    ItemId = q.Id,
                    Title = q.OfferNo ?? q.Id.ToString(),
                    Status = q.Status.HasValue ? q.Status.ToString() : null,
                    Amount = q.GrandTotal,
                    Date = q.OfferDate ?? q.CreatedDate
                })
                .ToListAsync();

            var orders = await _unitOfWork.Orders.Query(tracking: false)
                .Where(o => o.PotentialCustomerId == customerId && !o.IsDeleted)
                .Select(o => new Customer360TimelineItemDto
                {
                    Type = "Order",
                    ItemId = o.Id,
                    Title = o.OfferNo ?? o.Id.ToString(),
                    Status = o.Status.HasValue ? o.Status.ToString() : null,
                    Amount = o.GrandTotal,
                    Date = o.OfferDate ?? o.CreatedDate
                })
                .ToListAsync();

            var activities = await _unitOfWork.Activities.Query(tracking: false)
                .Where(a => a.PotentialCustomerId == customerId && !a.IsDeleted)
                .Select(a => new Customer360TimelineItemDto
                {
                    Type = "Activity",
                    ItemId = a.Id,
                    Title = a.Subject,
                    Status = a.Status,
                    Amount = null,
                    Date = a.ActivityDate ?? a.CreatedDate
                })
                .ToListAsync();

            var timeline = demands.Concat(quotations).Concat(orders).Concat(activities)
                .OrderByDescending(t => t.Date)
                .Take(50)
                .ToList();

            return timeline;
        }
    }
}
