using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Models;
using cms_webapi.Interfaces;
using cms_webapi.UnitOfWork;
using cms_webapi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using cms_webapi.Helpers;
using System;
using System.Security.Claims;
using System.Linq;


namespace cms_webapi.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;
        public QuotationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            CmsDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<QuotationGetDto>>> GetAllQuotationsAsync(PagedRequest request)
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

                var query = _context.Quotations
                    .AsNoTracking()
                    .Where(q => !q.IsDeleted)
                    .Include(q => q.CreatedByUser)
                    .Include(q => q.UpdatedByUser)
                    .Include(q => q.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(Quotation.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<QuotationGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<QuotationGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<QuotationGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("QuotationService.QuotationsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<QuotationGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetAllQuotationsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<QuotationGetDto>> GetQuotationByIdAsync(long id)
        {
            try
            {
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                if (quotation == null)
                {
                    return ApiResponse<QuotationGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.QuotationNotFound"),
                    _localizationService.GetLocalizedString("QuotationService.QuotationNotFound"),
                    StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var quotationWithNav = await _context.Quotations
                    .AsNoTracking()
                    .Include(q => q.CreatedByUser)
                    .Include(q => q.UpdatedByUser)
                    .Include(q => q.DeletedByUser)
                    .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);

                var quotationDto = _mapper.Map<QuotationGetDto>(quotationWithNav ?? quotation);
                return ApiResponse<QuotationGetDto>.SuccessResult(quotationDto, _localizationService.GetLocalizedString("QuotationService.QuotationRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetQuotationByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<QuotationDto>> CreateQuotationAsync(CreateQuotationDto createQuotationDto)
        {
            try
            {
                var quotation = _mapper.Map<Quotation>(createQuotationDto);
                quotation.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.Quotations.AddAsync(quotation);
                await _unitOfWork.SaveChangesAsync();

                var quotationDto = _mapper.Map<QuotationDto>(quotation);
                return ApiResponse<QuotationDto>.SuccessResult(quotationDto, _localizationService.GetLocalizedString("QuotationService.QuotationCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.CreateQuotationExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<QuotationDto>> UpdateQuotationAsync(long id, UpdateQuotationDto updateQuotationDto)
        {
            try
            {
                // Get userId from HttpContext (should be set by middleware)
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return ApiResponse<QuotationDto>.ErrorResult(
                        "Kullanıcı kimliği bulunamadı.",
                        "User identity not found",
                        StatusCodes.Status401Unauthorized);
                }


                var quotation = await _unitOfWork.Quotations
                    .Query()
                    .Include(q => q.Lines)
                    .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);

                if (quotation == null)
                {
                    return ApiResponse<QuotationDto>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationNotFound"),
                        "Not found",
                        StatusCodes.Status404NotFound);
                }


                // 3. Güncelleme işlemi
                _mapper.Map(updateQuotationDto, quotation);
                quotation.UpdatedDate = DateTime.UtcNow;
                quotation.UpdatedBy = userId;

                // 4. Toplamları yeniden hesapla
                decimal total = 0m;
                decimal grandTotal = 0m;

                foreach (var line in quotation.Lines.Where(l => !l.IsDeleted))
                {
                    total += line.LineTotal;
                    grandTotal += line.LineGrandTotal;
                }

                quotation.Total = total;
                quotation.GrandTotal = grandTotal;

                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();

                var quotationDto = _mapper.Map<QuotationDto>(quotation);
                return ApiResponse<QuotationDto>.SuccessResult(quotationDto, _localizationService.GetLocalizedString("QuotationService.QuotationUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.UpdateQuotationExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<object>> DeleteQuotationAsync(long id)
        {
            try
            {
                // Get userId from HttpContext (should be set by middleware)
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        "Kullanıcı kimliği bulunamadı.",
                        "User identity not found",
                        StatusCodes.Status401Unauthorized);
                }


                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                if (quotation == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationNotFound"),
                        "Not found",
                        StatusCodes.Status404NotFound);
                }


                // 3. Soft delete
                await _unitOfWork.Quotations.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("QuotationService.QuotationDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.DeleteQuotationExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        private long? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return null;
            }
            return userId;
        }

        public async Task<ApiResponse<List<QuotationGetDto>>> GetQuotationsByPotentialCustomerIdAsync(long potentialCustomerId)
        {
            try
            {
                var quotations = await _unitOfWork.Quotations.FindAsync(q => q.PotentialCustomerId == potentialCustomerId);
                var quotationDtos = _mapper.Map<List<QuotationGetDto>>(quotations.ToList());
                return ApiResponse<List<QuotationGetDto>>.SuccessResult(quotationDtos, _localizationService.GetLocalizedString("QuotationService.QuotationsByPotentialCustomerRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QuotationGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetQuotationsByPotentialCustomerExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<QuotationGetDto>>> GetQuotationsByRepresentativeIdAsync(long representativeId)
        {
            try
            {
                var quotations = await _unitOfWork.Quotations.FindAsync(q => q.RepresentativeId == representativeId);
                var quotationDtos = _mapper.Map<List<QuotationGetDto>>(quotations.ToList());
                return ApiResponse<List<QuotationGetDto>>.SuccessResult(quotationDtos, _localizationService.GetLocalizedString("QuotationService.QuotationsByRepresentativeRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QuotationGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetQuotationsByRepresentativeExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<QuotationGetDto>>> GetQuotationsByStatusAsync(int status)
        {
            try
            {
                var quotations = await _unitOfWork.Quotations.FindAsync(q => q.Status == status);
                var quotationDtos = _mapper.Map<List<QuotationGetDto>>(quotations.ToList());
                return ApiResponse<List<QuotationGetDto>>.SuccessResult(quotationDtos, _localizationService.GetLocalizedString("QuotationService.QuotationsByStatusRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QuotationGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetQuotationsByStatusExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<bool>> QuotationExistsAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.Quotations.ExistsAsync(id);
                return ApiResponse<bool>.SuccessResult(exists, exists ? _localizationService.GetLocalizedString("QuotationService.QuotationRetrieved") : _localizationService.GetLocalizedString("QuotationService.QuotationNotFound"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.QuotationExistsExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<QuotationGetDto>> CreateQuotationBulkAsync(QuotationBulkCreateDto bulkDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Header map
                var quotation = _mapper.Map<Quotation>(bulkDto.Quotation);

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

                quotation.Total = total;
                quotation.GrandTotal = grandTotal;

                // 3. Save header
                await _unitOfWork.Quotations.AddAsync(quotation);
                await _unitOfWork.SaveChangesAsync();

                // 4. Map & calculate lines
                var lines = new List<QuotationLine>(bulkDto.Lines.Count);

                foreach (var lineDto in bulkDto.Lines)
                {
                    var line = _mapper.Map<QuotationLine>(lineDto);
                    line.QuotationId = quotation.Id;

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

                await _unitOfWork.QuotationLines.AddAllAsync(lines);

                // 5. Exchange rates
                if (bulkDto.ExchangeRates?.Any() == true)
                {
                    var rates = bulkDto.ExchangeRates
                        .Select(r =>
                        {
                            var rate = _mapper.Map<QuotationExchangeRate>(r);
                            rate.QuotationId = quotation.Id;
                            return rate;
                        }).ToList();

                    await _unitOfWork.QuotationExchangeRates.AddAllAsync(rates);
                }

                // 6. Commit
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // 8. Reload
                var quotationWithNav = await _unitOfWork.Quotations
                    .Query()
                    .AsNoTracking()
                    .Include(q => q.Representative)
                    .Include(q => q.Lines)
                    .Include(q => q.PotentialCustomer)
                    .Include(q => q.CreatedByUser)
                    .Include(q => q.UpdatedByUser)
                    .FirstOrDefaultAsync(q => q.Id == quotation.Id);

                var dto = _mapper.Map<QuotationGetDto>(quotationWithNav);

                return ApiResponse<QuotationGetDto>.SuccessResult(dto, _localizationService.GetLocalizedString("QuotationService.QuotationCreated"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return ApiResponse<QuotationGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.CreateQuotationBulkExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        private static LineCalculationResult CalculateLine(
    decimal quantity,
    decimal unitPrice,
    decimal discountRate1,
    decimal discountRate2,
    decimal discountRate3,
    decimal discountAmount1,
    decimal discountAmount2,
    decimal discountAmount3,
    decimal vatRate)
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


        public async Task<ApiResponse<List<PricingRuleLineGetDto>>> GetPriceRuleOfQuotationAsync(string customerCode, long salesmanId, DateTime quotationDate)
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
                        x.RuleType == PricingRuleType.Quotation &&
                        !x.IsDeleted &&
                        x.BranchCode == branchCode &&
                        x.ValidFrom <= quotationDate &&
                        x.ValidTo >= quotationDate
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
                var denme = baseQuery.ToList();
                // 3️⃣ Kural yoksa → bilinçli boş dönüş
                if (header == null)
                {
                    return ApiResponse<List<PricingRuleLineGetDto>>.SuccessResult(
                        new List<PricingRuleLineGetDto>(),
                        _localizationService.GetLocalizedString(
                            "QuotationService.PriceRuleNotFound"));
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
                        "QuotationService.PriceRuleOfQuotationRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PricingRuleLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetPriceRuleOfQuotationExceptionMessage", ex.Message
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

                return ApiResponse<List<PriceOfProductDto>>.SuccessResult(price, _localizationService.GetLocalizedString("QuotationService.PriceOfProductRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PriceOfProductDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetPriceOfProductExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<bool>> StartApprovalFlowAsync(StartApprovalFlowDto request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Get userId from HttpContext
                var startedByUserId = GetCurrentUserId();
                if (startedByUserId == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.UserIdentityNotFound"),
                        "User identity not found",
                        StatusCodes.Status401Unauthorized);
                }

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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalFlowAlreadyExists"),
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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalFlowNotFound"),
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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalFlowStepsNotFound"),
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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalRoleNotFound"),
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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalUsersNotFound"),
                        "Bu step için onay yetkisi olan kullanıcı bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                // 7️⃣ ApprovalAction kayıtlarını oluştur
                var actions = new List<ApprovalAction>();
                foreach (var userId in userIds)
                {
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

                // Transaction'ı commit et
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<bool>.SuccessResult(
                    true,
                    _localizationService.GetLocalizedString("QuotationService.ApprovalFlowStarted"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.StartApprovalFlowExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<ApprovalActionGetDto>>> GetWaitingApprovalsAsync()
        {
            try
            {
                // Eğer userId verilmemişse HttpContext'ten al
                var targetUserId = GetCurrentUserId();
                if (targetUserId == null)
                {
                    return ApiResponse<List<ApprovalActionGetDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.UserIdentityNotFound"),
                        "User identity not found",
                        StatusCodes.Status401Unauthorized);
                }

                var approvalActions = await _context.ApprovalActions
                    .Where(x =>
                        x.ApprovalRequest.DocumentType == PricingRuleType.Quotation &&
                        x.ApprovedByUserId == targetUserId &&
                        x.Status == ApprovalStatus.Waiting &&
                        !x.IsDeleted)
                    .ToListAsync();

                var dtos = _mapper.Map<List<ApprovalActionGetDto>>(approvalActions);

                return ApiResponse<List<ApprovalActionGetDto>>.SuccessResult(
                    dtos,
                    _localizationService.GetLocalizedString("QuotationService.WaitingApprovalsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ApprovalActionGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.GetWaitingApprovalsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<bool>> ApproveAsync(ApproveActionDto request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.UserIdentityNotFound"),
                        "User identity not found",
                        StatusCodes.Status401Unauthorized);
                }

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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalActionNotFound"),
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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalActionApproved"));
                }

                // Step tamamlandı → sonraki step'e geç
                var approvalRequest = await _context.ApprovalRequests
                    .Include(ar => ar.ApprovalFlow)
                    .FirstOrDefaultAsync(x => x.Id == action.ApprovalRequestId && !x.IsDeleted);

                if (approvalRequest == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.ApprovalRequestNotFound"),
                        "Onay talebi bulunamadı.",
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
                    await _unitOfWork.CommitTransactionAsync();

                    return ApiResponse<bool>.SuccessResult(
                        true,
                        _localizationService.GetLocalizedString("QuotationService.ApprovalFlowCompleted"));
                }

                // Yeni step için onaycıları oluştur
                approvalRequest.CurrentStep = nextStep.StepOrder;
                approvalRequest.UpdatedDate = DateTime.UtcNow;
                approvalRequest.UpdatedBy = userId;
                await _unitOfWork.ApprovalRequests.UpdateAsync(approvalRequest);
                await _unitOfWork.SaveChangesAsync();

                // Yeni step için rolleri bul (StartApprovalFlow'daki mantık)
                // Not: Burada totalAmount bilgisine ihtiyacımız var, ApprovalRequest'ten EntityId ile Quotation'a bakabiliriz
                var quotation = await _context.Quotations
                    .FirstOrDefaultAsync(q => q.Id == approvalRequest.EntityId && !q.IsDeleted);

                if (quotation == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.QuotationNotFound"),
                        "Teklif bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                var validRoles = await _context.ApprovalRoles
                    .Where(r =>
                        r.ApprovalRoleGroupId == nextStep.ApprovalRoleGroupId &&
                        r.MaxAmount >= quotation.GrandTotal &&
                        !r.IsDeleted)
                    .ToListAsync();

                if (!validRoles.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.ApprovalRoleNotFound"),
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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalUsersNotFound"),
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

                return ApiResponse<bool>.SuccessResult(
                    true,
                    _localizationService.GetLocalizedString("QuotationService.ApprovalActionApprovedAndNextStepStarted"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.ApproveExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<bool>> RejectAsync(RejectActionDto request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.UserIdentityNotFound"),
                        "User identity not found",
                        StatusCodes.Status401Unauthorized);
                }

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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalActionNotFound"),
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
                        _localizationService.GetLocalizedString("QuotationService.ApprovalRequestNotFound"),
                        "Onay talebi bulunamadı.",
                        StatusCodes.Status404NotFound);
                }

                approvalRequest.Status = ApprovalStatus.Rejected;
                approvalRequest.UpdatedDate = DateTime.UtcNow;
                approvalRequest.UpdatedBy = userId;

                await _unitOfWork.ApprovalRequests.UpdateAsync(approvalRequest);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // 📌 Burada:
                // - Teklif sahibine mail gönderilebilir
                // - UI'da "Reddedildi" gösterilebilir
                // - Düzelt → yeniden başlat işlemi yapılabilir

                return ApiResponse<bool>.SuccessResult(
                    true,
                    _localizationService.GetLocalizedString("QuotationService.ApprovalActionRejected"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.RejectExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
 
    }
}
