using AutoMapper;
using crm_api.DTOs;
using crm_api.Models;
using crm_api.Interfaces;
using crm_api.UnitOfWork;
using crm_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using crm_api.Helpers;
using System;
using System.Security.Claims;
using System.Linq;
using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using Microsoft.Extensions.Configuration;
using crm_api.Models.Notification;
using crm_api.DTOs.NotificationDto;


namespace crm_api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;
        private readonly IDocumentSerialTypeService _documentSerialTypeService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            CmsDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IErpService erpService,
            IDocumentSerialTypeService documentSerialTypeService,
            IConfiguration configuration,
            IUserService userService,
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
            _documentSerialTypeService = documentSerialTypeService;
            _configuration = configuration;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<PagedResponse<OrderGetDto>>> GetAllOrdersAsync(PagedRequest request)
        {
            try
            {
                if (request == null)
                {
                    request = new PagedRequest();
                }

                if (request.Filters == null)
                {
                    request.Filters = new List<Filter>();
                }

                var query = _context.Orders
                    .AsNoTracking()
                    .Where(q => !q.IsDeleted)
                    .Include(q => q.CreatedByUser)
                    .Include(q => q.UpdatedByUser)
                    .Include(q => q.DeletedByUser)
                    .Include(q => q.DocumentSerialType)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(Order.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<OrderGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<OrderGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<OrderGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("OrderService.OrdersRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<OrderGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetAllOrdersExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderGetDto>> GetOrderByIdAsync(long id)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null)
                {
                    return ApiResponse<OrderGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.OrderNotFound"),
                    _localizationService.GetLocalizedString("OrderService.OrderNotFound"),
                    StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var orderWithNav = await _context.Orders
                    .AsNoTracking()
                    .Include(q => q.CreatedByUser)
                    .Include(q => q.UpdatedByUser)
                    .Include(q => q.DeletedByUser)
                    .Include(q => q.DocumentSerialType)
                    .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);

                var orderDto = _mapper.Map<OrderGetDto>(orderWithNav ?? order);
                return ApiResponse<OrderGetDto>.SuccessResult(orderDto, _localizationService.GetLocalizedString("OrderService.OrderRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetOrderByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(createOrderDto);
                order.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                var orderDto = _mapper.Map<OrderDto>(order);
                return ApiResponse<OrderDto>.SuccessResult(orderDto, _localizationService.GetLocalizedString("OrderService.OrderCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.CreateOrderExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<OrderDto>> UpdateOrderAsync(long id, UpdateOrderDto updateOrderDto)
        {
            try
            {
                // Get userId from HttpContext (should be set by middleware)
                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    return ApiResponse<OrderDto>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;

                var order = await _unitOfWork.Orders
                    .Query()
                    .Include(q => q.Lines)
                    .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);

                if (order == null)
                {
                    return ApiResponse<OrderDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderNotFound"),
                        "Not found",
                        StatusCodes.Status404NotFound);
                }


                // 3. Güncelleme işlemi
                _mapper.Map(updateOrderDto, order);
                order.UpdatedDate = DateTime.UtcNow;
                order.UpdatedBy = userId;

                // 4. Toplamları yeniden hesapla
                decimal total = 0m;
                decimal grandTotal = 0m;

                foreach (var line in order.Lines.Where(l => !l.IsDeleted))
                {
                    total += line.LineTotal;
                    grandTotal += line.LineGrandTotal;
                }

                order.Total = total;
                order.GrandTotal = grandTotal;

                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                var orderDto = _mapper.Map<OrderDto>(order);
                return ApiResponse<OrderDto>.SuccessResult(orderDto, _localizationService.GetLocalizedString("OrderService.OrderUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.UpdateOrderExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<object>> DeleteOrderAsync(long id)
        {
            try
            {
                // Get userId from HttpContext (should be set by middleware)
                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    return ApiResponse<object>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;


                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderNotFound"),
                        "Not found",
                        StatusCodes.Status404NotFound);
                }


                // 3. Soft delete
                await _unitOfWork.Orders.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("OrderService.OrderDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.DeleteOrderExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<OrderGetDto>>> GetOrdersByPotentialCustomerIdAsync(long potentialCustomerId)
        {
            try
            {
                var orders = await _unitOfWork.Orders.FindAsync(q => q.PotentialCustomerId == potentialCustomerId);
                var orderDtos = _mapper.Map<List<OrderGetDto>>(orders.ToList());
                return ApiResponse<List<OrderGetDto>>.SuccessResult(orderDtos, _localizationService.GetLocalizedString("OrderService.OrdersByPotentialCustomerRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OrderGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetOrdersByPotentialCustomerExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<OrderGetDto>>> GetOrdersByRepresentativeIdAsync(long representativeId)
        {
            try
            {
                var orders = await _unitOfWork.Orders.FindAsync(q => q.RepresentativeId == representativeId);
                var orderDtos = _mapper.Map<List<OrderGetDto>>(orders.ToList());
                return ApiResponse<List<OrderGetDto>>.SuccessResult(orderDtos, _localizationService.GetLocalizedString("OrderService.OrdersByRepresentativeRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OrderGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetOrdersByRepresentativeExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<OrderGetDto>>> GetOrdersByStatusAsync(int status)
        {
            try
            {
                var orders = await _unitOfWork.Orders.FindAsync(q => (int?)q.Status == status);
                var orderDtos = _mapper.Map<List<OrderGetDto>>(orders.ToList());
                return ApiResponse<List<OrderGetDto>>.SuccessResult(orderDtos, _localizationService.GetLocalizedString("OrderService.OrdersByStatusRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OrderGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetOrdersByStatusExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<bool>> OrderExistsAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.Orders.ExistsAsync(id);
                return ApiResponse<bool>.SuccessResult(exists, exists ? _localizationService.GetLocalizedString("OrderService.OrderRetrieved") : _localizationService.GetLocalizedString("OrderService.OrderNotFound"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.OrderExistsExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<OrderGetDto>> CreateOrderBulkAsync(OrderBulkCreateDto bulkDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var documentSerialType = await _documentSerialTypeService.GenerateDocumentSerialAsync(bulkDto.Order.DocumentSerialTypeId);
                if (!documentSerialType.Success)
                {
                    return ApiResponse<OrderGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.DocumentSerialTypeGenerationError"),
                        documentSerialType.Message,
                        StatusCodes.Status500InternalServerError);
                }
                bulkDto.Order.OfferNo = documentSerialType.Data;
                bulkDto.Order.RevisionNo = documentSerialType.Data;
                bulkDto.Order.Status = ApprovalStatus.HavenotStarted;

                // 1. Header map
                var order = _mapper.Map<Order>(bulkDto.Order);

                decimal total = 0m;
                decimal grandTotal = 0m;

                // 2. Header totals calculation
                foreach (var lineDto in bulkDto.Lines)
                {
                    var calc = CalculateLine(
                        lineDto.Quantity,
                        lineDto.UnitPrice,
                        lineDto.DiscountRate1,
                        lineDto.DiscountRate2,
                        lineDto.DiscountRate3,
                        lineDto.DiscountAmount1,
                        lineDto.DiscountAmount2,
                        lineDto.DiscountAmount3,
                        lineDto.VatRate
                    );

                    total += calc.NetTotal;
                    grandTotal += calc.GrandTotal;
                }

                order.Total = total;
                order.GrandTotal = grandTotal;

                // 3. Save header
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // 4. Map & calculate lines
                var lines = new List<OrderLine>(bulkDto.Lines.Count);

                foreach (var lineDto in bulkDto.Lines)
                {
                    var line = _mapper.Map<OrderLine>(lineDto);
                    line.OrderId = order.Id;

                    var calc = CalculateLine(
                        line.Quantity,
                        line.UnitPrice,
                        line.DiscountRate1,
                        line.DiscountRate2,
                        line.DiscountRate3,
                        line.DiscountAmount1,
                        line.DiscountAmount2,
                        line.DiscountAmount3,
                        line.VatRate
                    );

                    line.LineTotal = calc.NetTotal;
                    line.VatAmount = calc.VatAmount;
                    line.LineGrandTotal = calc.GrandTotal;

                    lines.Add(line);
                }

                await _unitOfWork.OrderLines.AddAllAsync(lines);

                // 5. Exchange rates
                if (bulkDto.ExchangeRates?.Any() == true)
                {
                    var rates = bulkDto.ExchangeRates
                        .Select(r =>
                        {
                            var rate = _mapper.Map<OrderExchangeRate>(r);
                            rate.OrderId = order.Id;
                            return rate;
                        }).ToList();

                    await _unitOfWork.OrderExchangeRates.AddAllAsync(rates);
                }

                // 6. Commit
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // 8. Reload
                var orderWithNav = await _unitOfWork.Orders
                    .Query()
                    .AsNoTracking()
                    .Include(q => q.Representative)
                    .Include(q => q.Lines)
                    .Include(q => q.PotentialCustomer)
                    .Include(q => q.CreatedByUser)
                    .Include(q => q.UpdatedByUser)
                    .Include(q => q.DocumentSerialType)
                    .FirstOrDefaultAsync(q => q.Id == order.Id);

                var dto = _mapper.Map<OrderGetDto>(orderWithNav);

                return ApiResponse<OrderGetDto>.SuccessResult(dto, _localizationService.GetLocalizedString("OrderService.OrderCreated"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return ApiResponse<OrderGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.CreateOrderBulkExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<OrderGetDto>> CreateRevisionOfOrderAsync(long orderId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<OrderGetDto>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;

                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                {
                    return ApiResponse<OrderGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.OrderNotFound"),
                        _localizationService.GetLocalizedString("OrderService.OrderNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var orderLines = await _unitOfWork.OrderLines.Query()
                    .Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

                var orderExchangeRates = await _unitOfWork.OrderExchangeRates.Query()
                    .Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

                var documentSerialTypeWithRevision = await _documentSerialTypeService.GenerateDocumentSerialAsync(order.DocumentSerialTypeId, false, order.RevisionNo);
                if (!documentSerialTypeWithRevision.Success)
                {
                    return ApiResponse<OrderGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.DocumentSerialTypeGenerationError"),
                        documentSerialTypeWithRevision.Message,
                        StatusCodes.Status500InternalServerError);
                }

                var newOrder = new Order();
                newOrder.OfferType = order.OfferType;
                newOrder.RevisionId = order.Id;
                newOrder.OfferDate = order.OfferDate;
                newOrder.OfferNo = order.OfferNo;
                newOrder.RevisionNo = documentSerialTypeWithRevision.Data;
                newOrder.OfferDate = order.OfferDate;
                newOrder.Currency = order.Currency;
                newOrder.Total = order.Total;
                newOrder.GrandTotal = order.GrandTotal;
                newOrder.CreatedBy = userId;
                newOrder.CreatedDate = DateTime.UtcNow;
                newOrder.PotentialCustomerId = order.PotentialCustomerId;
                newOrder.ErpCustomerCode = order.ErpCustomerCode;
                newOrder.ContactId = order.ContactId;
                newOrder.ValidUntil = order.ValidUntil;
                newOrder.DeliveryDate = order.DeliveryDate;
                newOrder.ShippingAddressId = order.ShippingAddressId;
                newOrder.RepresentativeId = order.RepresentativeId;
                newOrder.ActivityId = order.ActivityId;
                newOrder.Description = order.Description;
                newOrder.PaymentTypeId = order.PaymentTypeId;
                newOrder.HasCustomerSpecificDiscount = order.HasCustomerSpecificDiscount;
                newOrder.QuotationId = order.QuotationId;
                newOrder.Status = (int)ApprovalStatus.HavenotStarted;

                await _unitOfWork.Orders.AddAsync(newOrder);
                await _unitOfWork.SaveChangesAsync();

                var newOrderLines = new List<OrderLine>();
                foreach (var line in orderLines)
                {
                    var newLine = new OrderLine();
                    newLine.OrderId = newOrder.Id;
                    newLine.ProductCode = line.ProductCode;
                    newLine.Quantity = line.Quantity;
                    newLine.UnitPrice = line.UnitPrice;
                    newLine.DiscountRate1 = line.DiscountRate1;
                    newLine.DiscountRate2 = line.DiscountRate2;
                    newLine.DiscountRate3 = line.DiscountRate3;
                    newLine.DiscountAmount1 = line.DiscountAmount1;
                    newLine.DiscountAmount2 = line.DiscountAmount2;
                    newLine.DiscountAmount3 = line.DiscountAmount3;
                    newLine.VatRate = line.VatRate;
                    newLine.LineTotal = line.LineTotal;
                    newLine.VatAmount = line.VatAmount;
                    newLine.LineGrandTotal = line.LineGrandTotal;
                    newLine.Description = line.Description;
                    newLine.PricingRuleHeaderId = line.PricingRuleHeaderId;
                    newLine.RelatedStockId = line.RelatedStockId;
                    newLine.RelatedProductKey = line.RelatedProductKey;
                    newLine.IsMainRelatedProduct = line.IsMainRelatedProduct;
                    newLine.ApprovalStatus = ApprovalStatus.HavenotStarted;
                    newOrderLines.Add(newLine);
                }
                await _unitOfWork.OrderLines.AddAllAsync(newOrderLines);
                await _unitOfWork.SaveChangesAsync();

                var newOrderExchangeRates = new List<OrderExchangeRate>();
                foreach (var exchangeRate in orderExchangeRates)
                {
                    var newExchangeRate = new OrderExchangeRate();
                    newExchangeRate.OrderId = newOrder.Id;
                    newExchangeRate.Currency = exchangeRate.Currency;
                    newExchangeRate.ExchangeRate = exchangeRate.ExchangeRate;
                    newExchangeRate.ExchangeRateDate = exchangeRate.ExchangeRateDate;
                    newExchangeRate.IsOfficial = exchangeRate.IsOfficial;
                    newExchangeRate.CreatedDate = DateTime.UtcNow;
                    newExchangeRate.CreatedBy = userId;
                    newOrderExchangeRates.Add(newExchangeRate);
                }
                await _unitOfWork.OrderExchangeRates.AddAllAsync(newOrderExchangeRates);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                var dto = _mapper.Map<OrderGetDto>(newOrder);
                return ApiResponse<OrderGetDto>.SuccessResult(dto, _localizationService.GetLocalizedString("OrderService.RevisionCreated"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<OrderGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.CreateRevisionExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        private static LineCalculationResult CalculateLine(decimal quantity,decimal unitPrice,decimal discountRate1,decimal discountRate2,decimal discountRate3,decimal discountAmount1,decimal discountAmount2,decimal discountAmount3,decimal vatRate)
        {
            decimal gross = quantity * unitPrice;

            // Sequential discount rates
            decimal netAfterRates = gross;
            netAfterRates *= (1 - discountRate1 / 100m);
            netAfterRates *= (1 - discountRate2 / 100m);
            netAfterRates *= (1 - discountRate3 / 100m);

            // Discount amounts
            decimal net = netAfterRates
                - discountAmount1
                - discountAmount2
                - discountAmount3;

            if (net < 0)
                net = 0;

            net = Math.Round(net, 2, MidpointRounding.AwayFromZero);

            decimal vat = Math.Round(net * vatRate / 100m, 2, MidpointRounding.AwayFromZero);
            decimal grandTotal = net + vat;

            return new LineCalculationResult
            {
                NetTotal = net,
                VatAmount = vat,
                GrandTotal = grandTotal
            };
        }

        private sealed class LineCalculationResult
        {
            public decimal NetTotal { get; init; }
            public decimal VatAmount { get; init; }
            public decimal GrandTotal { get; init; }
        }


        public async Task<ApiResponse<List<PricingRuleLineGetDto>>> GetPriceRuleOfOrderAsync(string customerCode, long salesmanId, DateTime orderDate)
        {
            try
            {
                var branchCodeRequest = await _erpService.GetBranchCodeFromContext();
                if (!branchCodeRequest.Success)
                {
                    return ApiResponse<List<PricingRuleLineGetDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("ErpService.BranchCodeRetrievalError"),
                        _localizationService.GetLocalizedString("ErpService.BranchCodeRetrievalErrorMessage"),
                        StatusCodes.Status500InternalServerError);
                }
                
                short branchCode = branchCodeRequest.Data;

                // 1️⃣ Ortak filtre (tek doğruluk kaynağı)
                var baseQuery = _unitOfWork.PricingRuleHeaders.Query()
                    .AsNoTracking()
                    .Where(x =>
                        x.IsActive &&
                        x.RuleType == PricingRuleType.Order &&
                        !x.IsDeleted &&
                        x.BranchCode == branchCode &&
                        x.ValidFrom <= orderDate &&
                        x.ValidTo >= orderDate
                    );

                // 2️⃣ İş kuralı öncelik sırası AÇIK
                PricingRuleHeader? header =
                    // 1. Satışçı + Cari
                    await baseQuery
                        .Where(x =>
                            x.ErpCustomerCode == customerCode &&
                            x.Salesmen.Any(s => s.SalesmanId == salesmanId && !s.IsDeleted))
                        .FirstOrDefaultAsync()

                    // 2. Cari var – Satışçı yok
                    ?? await baseQuery
                        .Where(x =>
                            x.ErpCustomerCode == customerCode &&
                            !x.Salesmen.Any(s => !s.IsDeleted))
                        .FirstOrDefaultAsync()

                    // 3. Satışçı var – Cari yok
                    ?? await baseQuery
                        .Where(x =>
                            string.IsNullOrEmpty(x.ErpCustomerCode) &&
                            x.Salesmen.Any(s => s.SalesmanId == salesmanId && !s.IsDeleted))
                        .FirstOrDefaultAsync()

                    // 4. Genel (Cari yok – Satışçı yok)
                    ?? await baseQuery
                        .Where(x =>
                            string.IsNullOrEmpty(x.ErpCustomerCode) &&
                            !x.Salesmen.Any(s => !s.IsDeleted))
                        .FirstOrDefaultAsync();
                // 3️⃣ Kural yoksa → bilinçli boş dönüş
                if (header == null)
                {
                    return ApiResponse<List<PricingRuleLineGetDto>>.SuccessResult(
                        new List<PricingRuleLineGetDto>(),
                        _localizationService.GetLocalizedString(
                            "OrderService.PriceRuleNotFound"));
                }

                // 4️⃣ Line’ları getir
                var lines = await _unitOfWork.PricingRuleLines.Query()
                    .AsNoTracking()
                    .Where(x =>
                        x.PricingRuleHeaderId == header.Id &&
                        !x.IsDeleted)
                    .ToListAsync();

                var dto = _mapper.Map<List<PricingRuleLineGetDto>>(lines);

                return ApiResponse<List<PricingRuleLineGetDto>>.SuccessResult(
                    dto,
                    _localizationService.GetLocalizedString(
                        "OrderService.PriceRuleOfOrderRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PricingRuleLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetPriceRuleOfOrderExceptionMessage", ex.Message
                    , StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<PriceOfProductDto>>> GetPriceOfProductAsync(List<PriceOfProductRequestDto> request)
        {
            try
            {
                var price = new List<PriceOfProductDto>();

                // `request` koleksiyonundaki `ProductCode` ve `GroupCode` değerlerini alıyoruz
                var productCodes = request.Select(y => y.ProductCode).ToList();

                // 1. `ProductCode`'a göre fiyat bilgisi almak
                var pricePricing = await _unitOfWork.ProductPricings.Query()
                    .Where(x => productCodes.Contains(x.ErpProductCode) && !x.IsDeleted)
                    .ToListAsync();

                // Eğer fiyatlar varsa, bunları ekleyelim
                if (pricePricing.Count > 0)
                {
                    foreach (var item in pricePricing)
                    {
                        price.Add(new PriceOfProductDto()
                        {
                            ProductCode = item.ErpProductCode,
                            GroupCode = item.ErpGroupCode,
                            Currency = item.Currency,
                            ListPrice = item.ListPrice,
                            CostPrice = item.CostPrice,
                            Discount1 = item.Discount1,
                            Discount2 = item.Discount2,
                            Discount3 = item.Discount3
                        });
                    }
                }

                // 2. Eğer `ProductCode` için fiyat bilgisi yoksa, `GroupCode`'a göre fiyatları alıyoruz
                var leftBehindProductCodesWithGroup = request
                    .Where(x => !pricePricing.Any(y => y.ErpProductCode == x.ProductCode))  // Fiyatı olmayanları filtrele
                    .Select(x => new { x.ProductCode, x.GroupCode })  // Hem `ProductCode` hem de `GroupCode`'u seç
                    .ToList();  // Belleğe alalım

                // 3. Eğer `GroupCode`'a göre fiyatlar varsa, onları da alıp ekleyelim
                if (leftBehindProductCodesWithGroup.Count > 0)
                {
                    var groupCodeValues = leftBehindProductCodesWithGroup.Select(x => x.GroupCode).ToList();
                    // 2. `ProductPricingGroupBys` tablosundan, sadece `GroupCode`'larına göre fiyatları alıyoruz
                    var priceGroupBy = await _unitOfWork.ProductPricingGroupBys.Query()
                        .Where(x => groupCodeValues.Contains(x.ErpGroupCode) && !x.IsDeleted)  // Grup kodlarıyla eşleşen fiyatları alıyoruz
                        .ToListAsync();

                    foreach (var groupItem in priceGroupBy)
                    {
                        // `GroupCode` bazında fiyatları alıyoruz, fakat `ProductCode`'u ilişkili ürünlerle eşleştiriyoruz
                        foreach (var item in leftBehindProductCodesWithGroup.Where(x => x.GroupCode == groupItem.ErpGroupCode))
                        {
                            price.Add(new PriceOfProductDto()
                            {
                                ProductCode = item.ProductCode,  // Fiyat grup bazında alınacak, `ProductCode` grup koduna göre atanır
                                GroupCode = groupItem.ErpGroupCode,
                                Currency = groupItem.Currency,
                                ListPrice = groupItem.ListPrice,
                                CostPrice = groupItem.CostPrice,
                                Discount1 = groupItem.Discount1,
                                Discount2 = groupItem.Discount2,
                                Discount3 = groupItem.Discount3
                            });
                        }
                    }
                }

                return ApiResponse<List<PriceOfProductDto>>.SuccessResult(price, _localizationService.GetLocalizedString("OrderService.PriceOfProductRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PriceOfProductDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetPriceOfProductExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<bool>> StartApprovalFlowAsync(StartApprovalFlowDto request)
        {

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Get userId from HttpContext
                var startedByUserIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!startedByUserIdResponse.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        startedByUserIdResponse.Message,
                        startedByUserIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var startedByUserId = startedByUserIdResponse.Data;

                // 1️⃣ Daha önce başlatılmış mı?
                bool exists = await _context.ApprovalRequests
                    .AnyAsync(x =>
                        x.EntityId == request.EntityId &&
                        x.DocumentType == request.DocumentType &&
                        x.Status == ApprovalStatus.Waiting &&
                        !x.IsDeleted);

                if (exists)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalFlowAlreadyExists"),
                        "Bu belge için zaten aktif bir onay süreci var.",
                        StatusCodes.Status400BadRequest);
                }

                // 2️⃣ Aktif flow bul
                var flow = await _context.ApprovalFlows
                    .FirstOrDefaultAsync(x => 
                        x.DocumentType == request.DocumentType && 
                        x.IsActive && 
                        !x.IsDeleted);

                if (flow == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalFlowNotFound"),
                        "Bu belge tipi için onay akışı tanımlı değil.",
                        StatusCodes.Status404NotFound);
                }

                // 3️⃣ Step'leri sırayla al
                var steps = await _context.ApprovalFlowSteps
                    .Where(x => 
                        x.ApprovalFlowId == flow.Id && 
                        !x.IsDeleted)
                    .OrderBy(x => x.StepOrder)
                    .ToListAsync();

                if (!steps.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalFlowStepsNotFound"),
                        "Flow'a ait step tanımı yok.",
                        StatusCodes.Status404NotFound);
                }

                // 4️⃣ Tutarı karşılayan ilk step'i bul
                ApprovalFlowStep? selectedStep = null;
                List<ApprovalRole>? validRoles = null;

                foreach (var step in steps)
                {
                    var roles = await _context.ApprovalRoles
                        .Where(r =>
                            r.ApprovalRoleGroupId == step.ApprovalRoleGroupId &&
                            r.MaxAmount >= request.TotalAmount &&
                            !r.IsDeleted)
                        .ToListAsync();

                    if (roles.Any())
                    {
                        selectedStep = step;
                        validRoles = roles;
                        break;
                    }
                }

                if (selectedStep == null || validRoles == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalRoleNotFound"),
                        "Bu tutarı karşılayan onay yetkisi bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // 5️⃣ ApprovalRequest oluştur
                var approvalRequest = new ApprovalRequest
                {
                    EntityId = request.EntityId,
                    DocumentType = request.DocumentType,
                    ApprovalFlowId = flow.Id,
                    CurrentStep = selectedStep.StepOrder,
                    Status = ApprovalStatus.Waiting,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = startedByUserId,
                    IsDeleted = false
                };

                await _unitOfWork.ApprovalRequests.AddAsync(approvalRequest);
                await _unitOfWork.SaveChangesAsync();

                // 6️⃣ Bu step için onaylayacak kullanıcıları bul
                var roleIds = validRoles.Select(r => r.Id).ToList();
                var userIds = await _context.ApprovalUserRoles
                    .Where(x => 
                        roleIds.Contains(x.ApprovalRoleId) && 
                        !x.IsDeleted)
                    .Select(x => x.UserId)
                    .Distinct()
                    .ToListAsync();

                if (!userIds.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalUsersNotFound"),
                        "Bu step için onay yetkisi olan kullanıcı bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // 7️⃣ ApprovalAction kayıtlarını oluştur ve onay maili gönderilecek kullanıcıları topla
                var actions = new List<ApprovalAction>();
                var usersToNotify = new List<(string Email, string FullName, long UserId)>();

                foreach (var userId in userIds)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
                    if (user == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<bool>.ErrorResult(
                            _localizationService.GetLocalizedString("OrderService.UserNotFound"),
                            "Kullanıcı bulunamadı.",
                            StatusCodes.Status404NotFound);
                    }

                    if (!string.IsNullOrWhiteSpace(user.Email))
                        usersToNotify.Add((user.Email, user.FullName, userId));

                    var action = new ApprovalAction
                    {
                        ApprovalRequestId = approvalRequest.Id,
                        StepOrder = selectedStep.StepOrder,
                        ApprovedByUserId = userId,
                        Status = ApprovalStatus.Waiting,
                        ActionDate = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = startedByUserId,
                        IsDeleted = false
                    };

                    actions.Add(action);
                }

                await _unitOfWork.ApprovalActions.AddAllAsync(actions);
                await _unitOfWork.SaveChangesAsync();

                var order = await _unitOfWork.Orders.GetByIdAsync(request.EntityId);
                if (order == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.OrderNotFound"),
                        "Sipariş bulunamadı.",
                        StatusCodes.Status404NotFound);
                }
                order.Status = ApprovalStatus.Waiting;

                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // Transaction'ı commit et
                await _unitOfWork.CommitTransactionAsync();

                // UserId -> ApprovalActionId eşlemesi (onay linkleri için)
                var userIdToActionId = actions.ToDictionary(a => a.ApprovedByUserId, a => a.Id);
                var baseUrl = _configuration["FrontendSettings:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:5173";
                var approvalPath = _configuration["FrontendSettings:ApprovalPendingPath"]?.TrimStart('/') ?? "approvals/pending";
                var orderPath = _configuration["FrontendSettings:OrderDetailPath"]?.TrimStart('/') ?? "orders";

                // Send Notifications
                foreach (var user in usersToNotify)
                {
                    try
                    {
                        await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                        {
                            UserId = user.UserId,
                            TitleKey = "Notification.OrderApproval.Title", // "Onay Bekleyen Sipariş"
                            TitleArgs = new object[] { order.Id },
                            MessageKey = "Notification.OrderApproval.Message", // "{0} numaralı sipariş onay beklemektedir."
                            MessageArgs = new object[] { order.OfferNo ?? "" },
                            NotificationType = NotificationType.OrderApproval,
                            RelatedEntityName = "Order",
                            RelatedEntityId = order.Id
                        });
                    }
                    catch (Exception)
                    {
                        // ignore
                    }
                }

                BackgroundJob.Enqueue<IMailJob>(job =>
                    job.SendBulkOrderApprovalPendingEmailsAsync(
                        usersToNotify.ToList(),
                        userIdToActionId,
                        baseUrl,
                        approvalPath,
                        orderPath,
                        request.EntityId));

                return ApiResponse<bool>.SuccessResult(
                    true,
                    _localizationService.GetLocalizedString("OrderService.ApprovalFlowStarted"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.StartApprovalFlowExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<ApprovalActionGetDto>>> GetWaitingApprovalsAsync()
        {
            try
            {
                // Eğer userId verilmemişse HttpContext'ten al
                var targetUserIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!targetUserIdResponse.Success)
                {
                    return ApiResponse<List<ApprovalActionGetDto>>.ErrorResult(
                        targetUserIdResponse.Message,
                        targetUserIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var targetUserId = targetUserIdResponse.Data;

                var approvalActions = await _context.ApprovalActions
                    .Where(x =>
                        x.ApprovalRequest.DocumentType == PricingRuleType.Order &&
                        x.ApprovedByUserId == targetUserId &&
                        x.Status == ApprovalStatus.Waiting &&
                        !x.IsDeleted)
                    .ToListAsync();

                var dtos = _mapper.Map<List<ApprovalActionGetDto>>(approvalActions);

                return ApiResponse<List<ApprovalActionGetDto>>.SuccessResult(
                    dtos,
                    _localizationService.GetLocalizedString("OrderService.WaitingApprovalsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ApprovalActionGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetWaitingApprovalsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<bool>> ApproveAsync(ApproveActionDto request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;

                // Onay kaydını bul
                var action = await _context.ApprovalActions
                    .Include(a => a.ApprovalRequest)
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.ApprovalActionId &&
                        x.ApprovedByUserId == userId &&
                        !x.IsDeleted);

                if (action == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalActionNotFound"),
                        "Onay kaydı bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // Onay işlemini gerçekleştir
                action.Status = ApprovalStatus.Approved;
                action.ActionDate = DateTime.UtcNow;
                action.UpdatedDate = DateTime.UtcNow;
                action.UpdatedBy = userId;

                await _unitOfWork.ApprovalActions.UpdateAsync(action);
                await _unitOfWork.SaveChangesAsync();

                // Aynı step'te bekleyen var mı?
                bool anyWaiting = await _context.ApprovalActions
                    .AnyAsync(x =>
                        x.ApprovalRequestId == action.ApprovalRequestId &&
                        x.StepOrder == action.StepOrder &&
                        x.Status == ApprovalStatus.Waiting &&
                        !x.IsDeleted);

                if (anyWaiting)
                {
                    // Herkes onaylamadan ilerleme
                    await _unitOfWork.CommitTransactionAsync();
                    return ApiResponse<bool>.SuccessResult(
                        true,
                        _localizationService.GetLocalizedString("OrderService.ApprovalActionApproved"));
                }

                // Step tamamlandı → sonraki step'e geç
                var approvalRequest = await _context.ApprovalRequests
                    .Include(ar => ar.ApprovalFlow)
                    .FirstOrDefaultAsync(x => x.Id == action.ApprovalRequestId && !x.IsDeleted);

                if (approvalRequest == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalRequestNotFound"),
                        "Onay talebi bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // Order'ı al (hem akış bittiğinde hem de sonraki step için gerekli)
                var order = await _context.Orders
                    .FirstOrDefaultAsync(q => q.Id == approvalRequest.EntityId && !q.IsDeleted);

                if (order == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.OrderNotFound"),
                        "Sipariş bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                int nextStepOrder = approvalRequest.CurrentStep + 1;

                var nextStep = await _context.ApprovalFlowSteps
                    .FirstOrDefaultAsync(x =>
                        x.ApprovalFlowId == approvalRequest.ApprovalFlowId &&
                        x.StepOrder == nextStepOrder &&
                        !x.IsDeleted);

                if (nextStep == null)
                {
                    // 🎉 AKIŞ BİTTİ
                    approvalRequest.Status = ApprovalStatus.Approved;
                    approvalRequest.UpdatedDate = DateTime.UtcNow;
                    approvalRequest.UpdatedBy = userId;
                    await _unitOfWork.ApprovalRequests.UpdateAsync(approvalRequest);
                    await _unitOfWork.SaveChangesAsync();

                    // OrderLine'ların ApprovalStatus'unu Approved yap
                    var orderLines = await _context.OrderLines
                        .Where(ql => ql.OrderId == order.Id && !ql.IsDeleted)
                        .ToListAsync();

                    foreach (var line in orderLines)
                    {
                        line.ApprovalStatus = ApprovalStatus.Approved;
                        line.UpdatedDate = DateTime.UtcNow;
                        line.UpdatedBy = userId;
                        await _unitOfWork.OrderLines.UpdateAsync(line);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    // Sipariş sahibine onaylandı bildirimi ve mail gönder (eğer onaylayan kişi sipariş sahibi değilse)
                    if (order.CreatedBy > 0 && order.CreatedBy != userId)
                    {
                        try
                        {
                            var orderForNotification = await _unitOfWork.Orders.Query()
                                .Include(o => o.CreatedByUser)
                                .FirstOrDefaultAsync(o => o.Id == order.Id);

                            if (orderForNotification != null && orderForNotification.CreatedByUser != null)
                            {
                                // Bildirim oluştur
                                try
                                {
                                    await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                                    {
                                        UserId = orderForNotification.CreatedBy ?? 0L,
                                        TitleKey = "Notification.OrderApproved.Title", // "Sipariş Onaylandı"
                                        TitleArgs = new object[] { orderForNotification.Id },
                                        MessageKey = "Notification.OrderApproved.Message", // "{0} numaralı sipariş onaylandı."
                                        MessageArgs = new object[] { orderForNotification.OfferNo ?? "" },
                                        NotificationType = NotificationType.OrderDetail,
                                        RelatedEntityName = "Order",
                                        RelatedEntityId = orderForNotification.Id
                                    });
                                }
                                catch (Exception)
                                {
                                    // ignore
                                }

                                // Mail gönder
                                var approverUser = await _unitOfWork.Users.Query().FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
                                if (approverUser != null && !string.IsNullOrWhiteSpace(orderForNotification.CreatedByUser.Email))
                                {
                                    var baseUrl = _configuration["FrontendSettings:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:5173";
                                    var orderPath = _configuration["FrontendSettings:OrderDetailPath"]?.TrimStart('/') ?? "orders";
                                    var orderLink = $"{baseUrl}/{orderPath}/{orderForNotification.Id}";

                                    var creatorFullName = $"{orderForNotification.CreatedByUser.FirstName} {orderForNotification.CreatedByUser.LastName}".Trim();
                                    if (string.IsNullOrWhiteSpace(creatorFullName)) creatorFullName = orderForNotification.CreatedByUser.Username;

                                    var approverFullName = $"{approverUser.FirstName} {approverUser.LastName}".Trim();
                                    if (string.IsNullOrWhiteSpace(approverFullName)) approverFullName = approverUser.Username;

                                    BackgroundJob.Enqueue<IMailJob>(job =>
                                        job.SendOrderApprovedEmailAsync(
                                            orderForNotification.CreatedByUser.Email,
                                            creatorFullName,
                                            approverFullName,
                                            orderForNotification.OfferNo ?? "",
                                            orderLink
                                        ));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Bildirim ve mail gönderimi başarısız olsa bile işlem başarılı sayılmalı
                        }
                    }

                    return ApiResponse<bool>.SuccessResult(
                        true,
                        _localizationService.GetLocalizedString("OrderService.ApprovalFlowCompleted"));
                }

                // Yeni step için onaycıları oluştur
                approvalRequest.CurrentStep = nextStep.StepOrder;
                approvalRequest.UpdatedDate = DateTime.UtcNow;
                approvalRequest.UpdatedBy = userId;
                await _unitOfWork.ApprovalRequests.UpdateAsync(approvalRequest);
                await _unitOfWork.SaveChangesAsync();

                // Yeni step için rolleri bul (StartApprovalFlow'daki mantık)
                var validRoles = await _context.ApprovalRoles
                    .Where(r =>
                        r.ApprovalRoleGroupId == nextStep.ApprovalRoleGroupId &&
                        r.MaxAmount >= order.GrandTotal &&
                        !r.IsDeleted)
                    .ToListAsync();

                if (!validRoles.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalRoleNotFound"),
                        "Yeni step için uygun onay yetkisi bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // Onaylayacak kullanıcıları bul
                var roleIds = validRoles.Select(r => r.Id).ToList();
                var userIds = await _context.ApprovalUserRoles
                    .Where(x =>
                        roleIds.Contains(x.ApprovalRoleId) &&
                        !x.IsDeleted)
                    .Select(x => x.UserId)
                    .Distinct()
                    .ToListAsync();

                if (!userIds.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalUsersNotFound"),
                        "Yeni step için onay yetkisi olan kullanıcı bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // Yeni ApprovalAction kayıtlarını oluştur
                var newActions = new List<ApprovalAction>();
                foreach (var newUserId in userIds)
                {
                    var newAction = new ApprovalAction
                    {
                        ApprovalRequestId = approvalRequest.Id,
                        StepOrder = nextStep.StepOrder,
                        ApprovedByUserId = newUserId,
                        Status = ApprovalStatus.Waiting,
                        ActionDate = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = userId,
                        IsDeleted = false
                    };

                    newActions.Add(newAction);
                }

                await _unitOfWork.ApprovalActions.AddAllAsync(newActions);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // Yeni step için onaycılara bildirim ve mail gönder
                try
                {
                    var orderForNotification = await _unitOfWork.Orders.Query()
                        .FirstOrDefaultAsync(o => o.Id == order.Id);

                    if (orderForNotification != null)
                    {
                        // UserId -> ApprovalActionId eşlemesi (onay linkleri için)
                        var userIdToActionId = newActions.ToDictionary(a => a.ApprovedByUserId, a => a.Id);
                        var baseUrl = _configuration["FrontendSettings:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:5173";
                        var approvalPath = _configuration["FrontendSettings:ApprovalPendingPath"]?.TrimStart('/') ?? "approvals/pending";
                        var orderPath = _configuration["FrontendSettings:OrderDetailPath"]?.TrimStart('/') ?? "orders";

                        var usersToNotify = new List<(string Email, string FullName, long UserId)>();

                        foreach (var newUserId in userIds)
                        {
                            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == newUserId && !x.IsDeleted);
                            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
                            {
                                usersToNotify.Add((user.Email, user.FullName, newUserId));
                            }
                        }

                        // Bildirim gönder
                        foreach (var user in usersToNotify)
                        {
                            try
                            {
                                await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                                {
                                    UserId = user.UserId,
                                    TitleKey = "Notification.OrderApproval.Title", // "Onay Bekleyen Sipariş"
                                    TitleArgs = new object[] { orderForNotification.Id },
                                    MessageKey = "Notification.OrderApproval.Message", // "{0} numaralı sipariş onay beklemektedir."
                                    MessageArgs = new object[] { orderForNotification.OfferNo ?? "" },
                                    NotificationType = NotificationType.OrderApproval,
                                    RelatedEntityName = "Order",
                                    RelatedEntityId = orderForNotification.Id
                                });
                            }
                            catch (Exception)
                            {
                                // ignore
                            }
                        }

                        // Mail gönder
                        if (usersToNotify.Any())
                        {
                            BackgroundJob.Enqueue<IMailJob>(job =>
                                job.SendBulkOrderApprovalPendingEmailsAsync(
                                    usersToNotify,
                                    userIdToActionId,
                                    baseUrl,
                                    approvalPath,
                                    orderPath,
                                    orderForNotification.Id));
                        }
                    }
                }
                catch (Exception)
                {
                    // Bildirim ve mail gönderimi başarısız olsa bile işlem başarılı sayılmalı
                }

                return ApiResponse<bool>.SuccessResult(
                    true,
                    _localizationService.GetLocalizedString("OrderService.ApprovalActionApprovedAndNextStepStarted"));

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.ApproveExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<bool>> RejectAsync(RejectActionDto request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;

                // Onay kaydını bul
                var action = await _context.ApprovalActions
                    .Include(a => a.ApprovalRequest)
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.ApprovalActionId &&
                        x.ApprovedByUserId == userId &&
                        !x.IsDeleted);

                if (action == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalActionNotFound"),
                        "Onay kaydı bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // Red işlemini gerçekleştir
                action.Status = ApprovalStatus.Rejected;
                action.ActionDate = DateTime.UtcNow;
                action.UpdatedDate = DateTime.UtcNow;
                action.UpdatedBy = userId;

                await _unitOfWork.ApprovalActions.UpdateAsync(action);
                await _unitOfWork.SaveChangesAsync();

                // ApprovalRequest'i reddedildi olarak işaretle
                var approvalRequest = await _context.ApprovalRequests
                    .FirstOrDefaultAsync(x => x.Id == action.ApprovalRequestId && !x.IsDeleted);

                if (approvalRequest == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("OrderService.ApprovalRequestNotFound"),
                        "Onay talebi bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                approvalRequest.Status = ApprovalStatus.Rejected;
                approvalRequest.UpdatedDate = DateTime.UtcNow;
                approvalRequest.UpdatedBy = userId;

                await _unitOfWork.ApprovalRequests.UpdateAsync(approvalRequest);
                await _unitOfWork.SaveChangesAsync();

                // Eğer reddeden kullanıcı teklifi oluşturan kullanıcıysa ve en alt aşamadaysa (CurrentStep == 1)
                // OrderLine'ların ApprovalStatus'unu Rejected yap
                if (approvalRequest.CurrentStep == 1)
                {
                    var order = await _context.Orders
                        .FirstOrDefaultAsync(q => q.Id == approvalRequest.EntityId && !q.IsDeleted);

                    if (order != null && order.CreatedBy == userId)
                    {
                        var orderLines = await _context.OrderLines
                            .Where(ql => ql.OrderId == order.Id && !ql.IsDeleted)
                            .ToListAsync();

                        foreach (var line in orderLines)
                        {
                            line.ApprovalStatus = ApprovalStatus.Rejected;
                            line.UpdatedDate = DateTime.UtcNow;
                            line.UpdatedBy = userId;
                            await _unitOfWork.OrderLines.UpdateAsync(line);
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                // Sipariş sahibine mail gönder (eğer reddeden kişi sipariş sahibi değilse)
                try 
                {
                    var orderForMail = await _context.Orders
                        .Include(q => q.CreatedByUser)
                        .FirstOrDefaultAsync(q => q.Id == approvalRequest.EntityId);

                    if (orderForMail != null && orderForMail.CreatedBy != userId)
                    {
                        // Bildirim oluştur
                        try
                        {
                            await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                            {
                                UserId = orderForMail.CreatedBy ?? 0L,
                                TitleKey = "Notification.OrderRejected.Title", // "Sipariş Reddedildi"
                                TitleArgs = new object[] { orderForMail.Id },
                                MessageKey = "Notification.OrderRejected.Message", // "{0} numaralı sipariş reddedildi."
                                MessageArgs = new object[] { orderForMail.OfferNo ?? "" },
                                NotificationType = NotificationType.OrderDetail,
                                RelatedEntityName = "Order",
                                RelatedEntityId = orderForMail.Id
                            });
                        }
                        catch (Exception)
                        {
                            // ignore
                        }

                        var rejectorUser = await _context.Users.FindAsync(userId);
                        if (rejectorUser != null && orderForMail.CreatedByUser != null)
                        {
                            var baseUrl = _configuration["FrontendSettings:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:5173";
                            var orderPath = _configuration["FrontendSettings:OrderDetailPath"]?.TrimStart('/') ?? "orders";
                            var orderLink = $"{baseUrl}/{orderPath}/{orderForMail.Id}";
                            
                            var creatorFullName = $"{orderForMail.CreatedByUser.FirstName} {orderForMail.CreatedByUser.LastName}".Trim();
                            if (string.IsNullOrWhiteSpace(creatorFullName)) creatorFullName = orderForMail.CreatedByUser.Username;

                            var rejectorFullName = $"{rejectorUser.FirstName} {rejectorUser.LastName}".Trim();
                            if (string.IsNullOrWhiteSpace(rejectorFullName)) rejectorFullName = rejectorUser.Username;

                            BackgroundJob.Enqueue<IMailJob>(job => 
                                job.SendOrderRejectedEmailAsync(
                                    orderForMail.CreatedByUser.Email,
                                    creatorFullName,
                                    rejectorFullName,
                                    orderForMail.OfferNo ?? "",
                                    request.RejectReason ?? "Belirtilmedi",
                                    orderLink
                                ));
                        }
                    }
                }
                catch (Exception)
                {
                    // Mail gönderimi başarısız olsa bile işlem başarılı sayılmalı
                }

                return ApiResponse<bool>.SuccessResult(
                    true,
                    _localizationService.GetLocalizedString("OrderService.ApprovalActionRejected"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.RejectExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PagedResponse<OrderGetDto>>> GetRelatedOrders(PagedRequest request)
        {
            try
            {
                if (request == null)
                {
                    request = new PagedRequest();
                }

                if (request.Filters == null)
                {
                    request.Filters = new List<Filter>();
                }

                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    return ApiResponse<PagedResponse<OrderGetDto>>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;

                var avaibleUsersResponse = await GetOrderRelatedUsersAsync(userId);
                if (!avaibleUsersResponse.Success)
                {
                    return ApiResponse<PagedResponse<OrderGetDto>>.ErrorResult(
                        avaibleUsersResponse.Message,
                        avaibleUsersResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var avaibleUsers = avaibleUsersResponse.Data;
                var avaibleUsersIds = avaibleUsers.Select(x => x.UserId).ToList();


                var query = _context.Orders
                    .AsNoTracking()
                    .Where(q => !q.IsDeleted && (q.CreatedBy == userId || avaibleUsersIds.Contains(q.RepresentativeId.Value)))
                    .Include(q => q.CreatedByUser)
                    .Include(q => q.UpdatedByUser)
                    .Include(q => q.DeletedByUser)
                    .Include(q => q.DocumentSerialType)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(Order.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<OrderGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<OrderGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<OrderGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("OrderService.OrdersRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<OrderGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderService.InternalServerError"),
                    _localizationService.GetLocalizedString("OrderService.GetAllOrdersExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<ApprovalScopeUserDto>>> GetOrderRelatedUsersAsync(long userId)
        {
            try
            {
                /* -------------------------------------------------------
                 * 1️ Kullanıcının bulunduğu flow + max step
                 * -------------------------------------------------------*/
                var myFlowSteps = await
                (
                    from ur in _unitOfWork.ApprovalUserRoles.Query()
                    join ar in _unitOfWork.ApprovalRoles.Query()
                        on ur.ApprovalRoleId equals ar.Id
                    join fs in _unitOfWork.ApprovalFlowSteps.Query()
                        on ar.ApprovalRoleGroupId equals fs.ApprovalRoleGroupId
                    join f in _unitOfWork.ApprovalFlows.Query()
                        on fs.ApprovalFlowId equals f.Id
                    where ur.UserId == userId
                          && !ur.IsDeleted
                          && !ar.IsDeleted
                          && !fs.IsDeleted
                          && !f.IsDeleted
                          && f.IsActive
                          && f.DocumentType == PricingRuleType.Order
                    group fs by fs.ApprovalFlowId into g
                    select new MyFlowStepDto
                    {
                        ApprovalFlowId = g.Key,
                        MaxStepOrder = g.Max(x => x.StepOrder)
                    }
                ).ToListAsync();

                if (!myFlowSteps.Any())
                    return ApiResponse<List<ApprovalScopeUserDto>>
                        .SuccessResult(new List<ApprovalScopeUserDto>(), "");

                var flowStepMap = myFlowSteps
                    .ToDictionary(x => x.ApprovalFlowId, x => x.MaxStepOrder);

                /* -------------------------------------------------------
                 * 2️ Flow altındaki kullanıcılar
                 * -------------------------------------------------------*/
                var rawUsers =
                    (
                        from fs in _unitOfWork.ApprovalFlowSteps.Query()
                        join ar in _unitOfWork.ApprovalRoles.Query()
                            on fs.ApprovalRoleGroupId equals ar.ApprovalRoleGroupId
                        join ur in _unitOfWork.ApprovalUserRoles.Query()
                            on ar.Id equals ur.ApprovalRoleId
                        join u in _unitOfWork.Users.Query()
                            on ur.UserId equals u.Id
                        join rg in _unitOfWork.ApprovalRoleGroups.Query()
                            on fs.ApprovalRoleGroupId equals rg.Id
                        where !fs.IsDeleted
                              && !ar.IsDeleted
                              && !ur.IsDeleted
                              && !u.IsDeleted
                        select new
                        {
                            fs.ApprovalFlowId,
                            fs.StepOrder,
                            u.Id,
                            u.FirstName,
                            u.LastName,
                            RoleGroupName = rg.Name
                        }
                    )
                    .AsEnumerable(); // SQL → Memory geçişi

                var usersUnderMe = rawUsers
                    .Where(x =>
                        flowStepMap.TryGetValue(x.ApprovalFlowId, out var maxStep)
                        && (
                            x.StepOrder < maxStep
                            || (x.StepOrder == maxStep && x.Id == userId)
                        )
                    )
                    .Select(x => new ApprovalScopeUserDto
                    {
                        FlowId = x.ApprovalFlowId,
                        UserId = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        RoleGroupName = x.RoleGroupName,
                        StepOrder = x.StepOrder
                    })
                    .Distinct()
                    .OrderBy(x => x.FlowId)
                    .ThenBy(x => x.StepOrder)
                    .ToList();

                return ApiResponse<List<ApprovalScopeUserDto>>
                    .SuccessResult(
                        usersUnderMe,
                        _localizationService.GetLocalizedString(
                            "OrderService.OrderApproverUsersRetrieved"
                        )
                    );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ApprovalScopeUserDto>>
                    .ErrorResult(
                        _localizationService.GetLocalizedString(
                            "OrderService.InternalServerError"
                        ),
                        ex.Message,
                        StatusCodes.Status500InternalServerError
                    );
            }
        }
    }
}
