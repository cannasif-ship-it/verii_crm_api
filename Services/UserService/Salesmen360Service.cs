using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services
{
    public class Salesmen360Service : ISalesmen360Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public Salesmen360Service(IUnitOfWork unitOfWork, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<Salesmen360OverviewDto>> GetOverviewAsync(long userId, string? currency = null)
        {
            try
            {
                var user = await GetUserAsync(userId);

                if (user == null)
                {
                    return ApiResponse<Salesmen360OverviewDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserService.UserNotFound"),
                        _localizationService.GetLocalizedString("UserService.UserNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var totalDemands = await _unitOfWork.Demands.Query(tracking: false)
                    .CountAsync(d => d.RepresentativeId == userId && !d.IsDeleted && (d.Status == null || d.Status != ApprovalStatus.Closed));

                var totalQuotations = await _unitOfWork.Quotations.Query(tracking: false)
                    .CountAsync(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed));

                var totalOrders = await _unitOfWork.Orders.Query(tracking: false)
                    .CountAsync(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed));

                var totalActivities = await _unitOfWork.Activities.Query(tracking: false)
                    .CountAsync(a => a.AssignedUserId == userId && !a.IsDeleted);

                var currencyNameMap = await GetCurrencyNameMapAsync();
                var normalizedCurrency = string.IsNullOrWhiteSpace(currency) ? null : NormalizeCurrency(currency);
                var currencyFilterValues = normalizedCurrency == null
                    ? null
                    : BuildCurrencyFilterValues(normalizedCurrency, currencyNameMap);
                decimal totalDemandAmount = 0m;
                decimal totalQuotationAmount = 0m;
                decimal totalOrderAmount = 0m;

                if (currencyFilterValues != null)
                {
                    totalDemandAmount = await _unitOfWork.Demands.Query(tracking: false)
                        .Where(d => d.RepresentativeId == userId && !d.IsDeleted && (d.Status == null || d.Status != ApprovalStatus.Closed) &&
                            currencyFilterValues.Contains((d.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(d => (decimal?)d.GrandTotal) ?? 0m;

                    totalQuotationAmount = await _unitOfWork.Quotations.Query(tracking: false)
                        .Where(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed) &&
                            currencyFilterValues.Contains((q.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(q => (decimal?)q.GrandTotal) ?? 0m;

                    totalOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                        .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed) &&
                            currencyFilterValues.Contains((o.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m;
                }

                var totalsByCurrency = await GetTotalsByCurrencyAsync(userId);
                totalsByCurrency = MergeCurrencyAmountRows(totalsByCurrency, currencyNameMap);

                var response = new Salesmen360OverviewDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Kpis = new Salesmen360KpiDto
                    {
                        Currency = normalizedCurrency == null ? null : ResolveCurrencyName(normalizedCurrency, currencyNameMap),
                        TotalDemands = totalDemands,
                        TotalQuotations = totalQuotations,
                        TotalOrders = totalOrders,
                        TotalActivities = totalActivities,
                        TotalDemandAmount = totalDemandAmount,
                        TotalQuotationAmount = totalQuotationAmount,
                        TotalOrderAmount = totalOrderAmount,
                        TotalsByCurrency = totalsByCurrency
                    }
                };

                return ApiResponse<Salesmen360OverviewDto>.SuccessResult(
                    response,
                    _localizationService.GetLocalizedString("General.OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<Salesmen360OverviewDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<Salesmen360AnalyticsSummaryDto>> GetAnalyticsSummaryAsync(long userId, string? currency = null)
        {
            try
            {
                var user = await GetUserAsync(userId);

                if (user == null)
                {
                    return ApiResponse<Salesmen360AnalyticsSummaryDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserService.UserNotFound"),
                        _localizationService.GetLocalizedString("UserService.UserNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var sinceDate = DateTime.UtcNow.AddMonths(-12);
                var currencyNameMap = await GetCurrencyNameMapAsync();
                var normalizedCurrency = string.IsNullOrWhiteSpace(currency) ? null : NormalizeCurrency(currency);
                var currencyFilterValues = normalizedCurrency == null
                    ? null
                    : BuildCurrencyFilterValues(normalizedCurrency, currencyNameMap);
                decimal last12MonthsOrderAmount = 0m;
                decimal openQuotationAmount = 0m;
                decimal openOrderAmount = 0m;

                if (currencyFilterValues != null)
                {
                    last12MonthsOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                        .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed) &&
                            (o.OfferDate ?? o.CreatedDate) >= sinceDate &&
                            currencyFilterValues.Contains((o.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m;

                    openQuotationAmount = await _unitOfWork.Quotations.Query(tracking: false)
                        .Where(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed) &&
                            q.Status != ApprovalStatus.Approved && q.Status != ApprovalStatus.Rejected &&
                            currencyFilterValues.Contains((q.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(q => (decimal?)q.GrandTotal) ?? 0m;

                    openOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                        .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed) &&
                            o.Status != ApprovalStatus.Approved && o.Status != ApprovalStatus.Rejected &&
                            currencyFilterValues.Contains((o.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m;
                }

                var activityCount = await _unitOfWork.Activities.Query(tracking: false)
                    .CountAsync(a => a.AssignedUserId == userId && !a.IsDeleted);
                var totalsByCurrency = await GetTotalsByCurrencyAsync(userId);
                totalsByCurrency = MergeCurrencyAmountRows(totalsByCurrency, currencyNameMap);

                var summary = new Salesmen360AnalyticsSummaryDto
                {
                    Currency = normalizedCurrency == null ? null : ResolveCurrencyName(normalizedCurrency, currencyNameMap),
                    Last12MonthsOrderAmount = last12MonthsOrderAmount,
                    OpenQuotationAmount = openQuotationAmount,
                    OpenOrderAmount = openOrderAmount,
                    ActivityCount = activityCount,
                    LastActivityDate = await GetLastActivityDateAsync(userId),
                    TotalsByCurrency = totalsByCurrency
                };

                return ApiResponse<Salesmen360AnalyticsSummaryDto>.SuccessResult(
                    summary,
                    _localizationService.GetLocalizedString("General.OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<Salesmen360AnalyticsSummaryDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<Salesmen360AnalyticsChartsDto>> GetAnalyticsChartsAsync(long userId, int months = 12, string? currency = null)
        {
            try
            {
                var user = await GetUserAsync(userId);

                if (user == null)
                {
                    return ApiResponse<Salesmen360AnalyticsChartsDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserService.UserNotFound"),
                        _localizationService.GetLocalizedString("UserService.UserNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var safeMonths = months <= 0 ? 12 : Math.Min(months, 36);
                var currencyNameMap = await GetCurrencyNameMapAsync();
                var normalizedCurrency = string.IsNullOrWhiteSpace(currency) ? null : NormalizeCurrency(currency);
                var currencyFilterValues = normalizedCurrency == null
                    ? null
                    : BuildCurrencyFilterValues(normalizedCurrency, currencyNameMap);
                var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                var startMonth = currentMonth.AddMonths(-(safeMonths - 1));
                var endExclusive = currentMonth.AddMonths(1);
                var monthLabels = Enumerable.Range(0, safeMonths)
                    .Select(i => startMonth.AddMonths(i).ToString("yyyy-MM"))
                    .ToList();

                var demandDates = await _unitOfWork.Demands.Query(tracking: false)
                    .Where(d => d.RepresentativeId == userId && !d.IsDeleted && (d.Status == null || d.Status != ApprovalStatus.Closed))
                    .Select(d => d.OfferDate ?? d.CreatedDate)
                    .Where(d => d >= startMonth && d < endExclusive)
                    .ToListAsync();

                var quotationDates = await _unitOfWork.Quotations.Query(tracking: false)
                    .Where(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed))
                    .Select(q => q.OfferDate ?? q.CreatedDate)
                    .Where(d => d >= startMonth && d < endExclusive)
                    .ToListAsync();

                var orderDates = await _unitOfWork.Orders.Query(tracking: false)
                    .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed))
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
                    .Select(label => new Salesmen360MonthlyTrendItemDto
                    {
                        Month = label,
                        DemandCount = demandByMonth.TryGetValue(label, out var dCount) ? dCount : 0,
                        QuotationCount = quotationByMonth.TryGetValue(label, out var qCount) ? qCount : 0,
                        OrderCount = orderByMonth.TryGetValue(label, out var oCount) ? oCount : 0
                    })
                    .ToList();

                var distribution = new Salesmen360DistributionDto
                {
                    DemandCount = await _unitOfWork.Demands.Query(tracking: false)
                        .CountAsync(d => d.RepresentativeId == userId && !d.IsDeleted && (d.Status == null || d.Status != ApprovalStatus.Closed)),
                    QuotationCount = await _unitOfWork.Quotations.Query(tracking: false)
                        .CountAsync(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed)),
                    OrderCount = await _unitOfWork.Orders.Query(tracking: false)
                        .CountAsync(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed))
                };

                var last12MonthsStart = currentMonth.AddMonths(-11);

                decimal last12OrderAmount = 0m;
                decimal openQuotationAmount = 0m;
                decimal openOrderAmount = 0m;

                if (currencyFilterValues != null)
                {
                    last12OrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                        .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed) &&
                            (o.OfferDate ?? o.CreatedDate) >= last12MonthsStart &&
                            (o.OfferDate ?? o.CreatedDate) < endExclusive &&
                            currencyFilterValues.Contains((o.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m;

                    openQuotationAmount = await _unitOfWork.Quotations.Query(tracking: false)
                        .Where(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed) &&
                            q.Status != ApprovalStatus.Approved && q.Status != ApprovalStatus.Rejected &&
                            currencyFilterValues.Contains((q.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(q => (decimal?)q.GrandTotal) ?? 0m;

                    openOrderAmount = await _unitOfWork.Orders.Query(tracking: false)
                        .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed) &&
                            o.Status != ApprovalStatus.Approved && o.Status != ApprovalStatus.Rejected &&
                            currencyFilterValues.Contains((o.Currency ?? "UNKNOWN").ToUpper()))
                        .SumAsync(o => (decimal?)o.GrandTotal) ?? 0m;
                }

                var amountComparison = new Salesmen360AmountComparisonDto
                {
                    Currency = normalizedCurrency == null ? null : ResolveCurrencyName(normalizedCurrency, currencyNameMap),
                    Last12MonthsOrderAmount = last12OrderAmount,
                    OpenQuotationAmount = openQuotationAmount,
                    OpenOrderAmount = openOrderAmount
                };

                var amountComparisonByCurrency = await GetAmountComparisonByCurrencyAsync(userId, last12MonthsStart, endExclusive);
                amountComparisonByCurrency = MergeAmountComparisonRows(amountComparisonByCurrency, currencyNameMap);

                var charts = new Salesmen360AnalyticsChartsDto
                {
                    MonthlyTrend = monthlyTrend,
                    Distribution = distribution,
                    AmountComparison = amountComparison,
                    AmountComparisonByCurrency = amountComparisonByCurrency
                };

                return ApiResponse<Salesmen360AnalyticsChartsDto>.SuccessResult(
                    charts,
                    _localizationService.GetLocalizedString("General.OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<Salesmen360AnalyticsChartsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<User?> GetUserAsync(long userId)
        {
            return await _unitOfWork.Users.Query(tracking: false)
                .Where(u => u.Id == userId && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        private static string NormalizeCurrency(string? currency)
        {
            return string.IsNullOrWhiteSpace(currency) ? "UNKNOWN" : currency.Trim().ToUpperInvariant();
        }

        private async Task<Dictionary<string, string>> GetCurrencyNameMapAsync()
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var rates = await _erpService.GetExchangeRateAsync(DateTime.Now, 1);

            if (!rates.Success || rates.Data == null)
            {
                return result;
            }

            foreach (var item in rates.Data)
            {
                if (item.DovizTipi <= 0 || string.IsNullOrWhiteSpace(item.DovizIsmi))
                {
                    continue;
                }

                var idKey = item.DovizTipi.ToString();
                var nameValue = NormalizeCurrency(item.DovizIsmi);

                if (!result.ContainsKey(idKey))
                {
                    result[idKey] = nameValue;
                }

                if (!result.ContainsKey(nameValue))
                {
                    result[nameValue] = nameValue;
                }
            }

            return result;
        }

        private static List<string> BuildCurrencyFilterValues(string normalizedCurrency, Dictionary<string, string> currencyNameMap)
        {
            var values = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { normalizedCurrency };

            if (currencyNameMap.TryGetValue(normalizedCurrency, out var mappedName))
            {
                values.Add(mappedName);
            }

            foreach (var item in currencyNameMap.Where(x => string.Equals(x.Value, normalizedCurrency, StringComparison.OrdinalIgnoreCase)))
            {
                values.Add(item.Key);
            }

            return values.Select(NormalizeCurrency).Distinct().ToList();
        }

        private static string ResolveCurrencyName(string? currency, Dictionary<string, string> currencyNameMap)
        {
            var normalized = NormalizeCurrency(currency);
            return currencyNameMap.TryGetValue(normalized, out var mappedName) ? mappedName : normalized;
        }

        private static List<Salesmen360CurrencyAmountDto> MergeCurrencyAmountRows(
            List<Salesmen360CurrencyAmountDto> rows,
            Dictionary<string, string> currencyNameMap)
        {
            return rows
                .GroupBy(x => ResolveCurrencyName(x.Currency, currencyNameMap), StringComparer.OrdinalIgnoreCase)
                .Select(g => new Salesmen360CurrencyAmountDto
                {
                    Currency = g.Key,
                    DemandAmount = g.Sum(x => x.DemandAmount),
                    QuotationAmount = g.Sum(x => x.QuotationAmount),
                    OrderAmount = g.Sum(x => x.OrderAmount)
                })
                .OrderBy(x => x.Currency)
                .ToList();
        }

        private static List<Salesmen360AmountComparisonDto> MergeAmountComparisonRows(
            List<Salesmen360AmountComparisonDto> rows,
            Dictionary<string, string> currencyNameMap)
        {
            return rows
                .GroupBy(x => ResolveCurrencyName(x.Currency, currencyNameMap), StringComparer.OrdinalIgnoreCase)
                .Select(g => new Salesmen360AmountComparisonDto
                {
                    Currency = g.Key,
                    Last12MonthsOrderAmount = g.Sum(x => x.Last12MonthsOrderAmount),
                    OpenQuotationAmount = g.Sum(x => x.OpenQuotationAmount),
                    OpenOrderAmount = g.Sum(x => x.OpenOrderAmount)
                })
                .OrderBy(x => x.Currency)
                .ToList();
        }

        private async Task<List<Salesmen360CurrencyAmountDto>> GetTotalsByCurrencyAsync(long userId)
        {
            var demandTotals = await _unitOfWork.Demands.Query(tracking: false)
                .Where(d => d.RepresentativeId == userId && !d.IsDeleted && (d.Status == null || d.Status != ApprovalStatus.Closed))
                .GroupBy(d => d.Currency)
                .Select(g => new { Currency = g.Key, Amount = g.Sum(x => x.GrandTotal) })
                .ToListAsync();

            var quotationTotals = await _unitOfWork.Quotations.Query(tracking: false)
                .Where(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed))
                .GroupBy(q => q.Currency)
                .Select(g => new { Currency = g.Key, Amount = g.Sum(x => x.GrandTotal) })
                .ToListAsync();

            var orderTotals = await _unitOfWork.Orders.Query(tracking: false)
                .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed))
                .GroupBy(o => o.Currency)
                .Select(g => new { Currency = g.Key, Amount = g.Sum(x => x.GrandTotal) })
                .ToListAsync();

            var result = new Dictionary<string, Salesmen360CurrencyAmountDto>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in demandTotals)
            {
                var key = NormalizeCurrency(item.Currency);
                if (!result.TryGetValue(key, out var dto))
                {
                    dto = new Salesmen360CurrencyAmountDto { Currency = key };
                    result[key] = dto;
                }
                dto.DemandAmount = item.Amount;
            }

            foreach (var item in quotationTotals)
            {
                var key = NormalizeCurrency(item.Currency);
                if (!result.TryGetValue(key, out var dto))
                {
                    dto = new Salesmen360CurrencyAmountDto { Currency = key };
                    result[key] = dto;
                }
                dto.QuotationAmount = item.Amount;
            }

            foreach (var item in orderTotals)
            {
                var key = NormalizeCurrency(item.Currency);
                if (!result.TryGetValue(key, out var dto))
                {
                    dto = new Salesmen360CurrencyAmountDto { Currency = key };
                    result[key] = dto;
                }
                dto.OrderAmount = item.Amount;
            }

            return result.Values.OrderBy(x => x.Currency).ToList();
        }

        private async Task<List<Salesmen360AmountComparisonDto>> GetAmountComparisonByCurrencyAsync(
            long userId,
            DateTime last12MonthsStart,
            DateTime endExclusive)
        {
            var quotationOpenByCurrency = await _unitOfWork.Quotations.Query(tracking: false)
                .Where(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed) &&
                    q.Status != ApprovalStatus.Approved && q.Status != ApprovalStatus.Rejected)
                .GroupBy(q => q.Currency)
                .Select(g => new { Currency = g.Key, Amount = g.Sum(x => x.GrandTotal) })
                .ToListAsync();

            var orderOpenByCurrency = await _unitOfWork.Orders.Query(tracking: false)
                .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed) &&
                    o.Status != ApprovalStatus.Approved && o.Status != ApprovalStatus.Rejected)
                .GroupBy(o => o.Currency)
                .Select(g => new { Currency = g.Key, Amount = g.Sum(x => x.GrandTotal) })
                .ToListAsync();

            var orderLast12ByCurrency = await _unitOfWork.Orders.Query(tracking: false)
                .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed) &&
                    (o.OfferDate ?? o.CreatedDate) >= last12MonthsStart &&
                    (o.OfferDate ?? o.CreatedDate) < endExclusive)
                .GroupBy(o => o.Currency)
                .Select(g => new { Currency = g.Key, Amount = g.Sum(x => x.GrandTotal) })
                .ToListAsync();

            var result = new Dictionary<string, Salesmen360AmountComparisonDto>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in orderLast12ByCurrency)
            {
                var key = NormalizeCurrency(item.Currency);
                if (!result.TryGetValue(key, out var dto))
                {
                    dto = new Salesmen360AmountComparisonDto { Currency = key };
                    result[key] = dto;
                }
                dto.Last12MonthsOrderAmount = item.Amount;
            }

            foreach (var item in quotationOpenByCurrency)
            {
                var key = NormalizeCurrency(item.Currency);
                if (!result.TryGetValue(key, out var dto))
                {
                    dto = new Salesmen360AmountComparisonDto { Currency = key };
                    result[key] = dto;
                }
                dto.OpenQuotationAmount = item.Amount;
            }

            foreach (var item in orderOpenByCurrency)
            {
                var key = NormalizeCurrency(item.Currency);
                if (!result.TryGetValue(key, out var dto))
                {
                    dto = new Salesmen360AmountComparisonDto { Currency = key };
                    result[key] = dto;
                }
                dto.OpenOrderAmount = item.Amount;
            }

            return result.Values.OrderBy(x => x.Currency).ToList();
        }

        private async Task<DateTime?> GetLastActivityDateAsync(long userId)
        {
            var demandDate = await _unitOfWork.Demands.Query(tracking: false)
                .Where(d => d.RepresentativeId == userId && !d.IsDeleted && (d.Status == null || d.Status != ApprovalStatus.Closed))
                .Select(d => (DateTime?)(d.OfferDate ?? d.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var quotationDate = await _unitOfWork.Quotations.Query(tracking: false)
                .Where(q => q.RepresentativeId == userId && !q.IsDeleted && (q.Status == null || q.Status != ApprovalStatus.Closed))
                .Select(q => (DateTime?)(q.OfferDate ?? q.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var orderDate = await _unitOfWork.Orders.Query(tracking: false)
                .Where(o => o.RepresentativeId == userId && !o.IsDeleted && (o.Status == null || o.Status != ApprovalStatus.Closed))
                .Select(o => (DateTime?)(o.OfferDate ?? o.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var activityDate = await _unitOfWork.Activities.Query(tracking: false)
                .Where(a => a.AssignedUserId == userId && !a.IsDeleted)
                .Select(a => (DateTime?)(a.ActivityDate ?? a.CreatedDate))
                .DefaultIfEmpty()
                .MaxAsync();

            var dates = new[] { demandDate, quotationDate, orderDate, activityDate }
                .Where(d => d.HasValue)
                .Select(d => d!.Value)
                .ToList();

            return dates.Count > 0 ? dates.Max() : null;
        }
    }
}
