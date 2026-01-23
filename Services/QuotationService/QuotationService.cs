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


namespace crm_api.Services
{
    public class QuotationService : IQuotationService
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
        public QuotationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            CmsDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IErpService erpService,
            IDocumentSerialTypeService documentSerialTypeService,
            IConfiguration configuration,
            IUserService userService)
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
                    .Include(q => q.DocumentSerialType)
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
                    .Include(q => q.DocumentSerialType)
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
                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    return ApiResponse<QuotationDto>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;

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
                var userIdResponse = await _userService.GetCurrentUserIdAsync();
                if (!userIdResponse.Success)
                {
                    return ApiResponse<object>.ErrorResult(
                        userIdResponse.Message,
                        userIdResponse.Message,
                        StatusCodes.Status401Unauthorized);
                }
                var userId = userIdResponse.Data;


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
                var quotations = await _unitOfWork.Quotations.FindAsync(q => (int?)q.Status == status);
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
                var documentSerialType = await _documentSerialTypeService.GenerateDocumentSerialAsync(bulkDto.Quotation.DocumentSerialTypeId);
                if (!documentSerialType.Success)
                {
                    return ApiResponse<QuotationGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.DocumentSerialTypeGenerationError"),
                        documentSerialType.Message,
                        StatusCodes.Status500InternalServerError);
                }
                bulkDto.Quotation.OfferNo = documentSerialType.Data;
                bulkDto.Quotation.RevisionNo = documentSerialType.Data;
                bulkDto.Quotation.Status = ApprovalStatus.HavenotStarted;

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
                    .Include(q => q.DocumentSerialType)
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


      public async Task<ApiResponse<QuotationGetDto>> CreateRevisionOfQuotationAsync(long quotationId)
        {
        await _unitOfWork.BeginTransactionAsync();
            try
            {
                    var quotation = await _unitOfWork.Quotations.GetByIdAsync(quotationId);
                    if (quotation == null)
                    {
                        return ApiResponse<QuotationGetDto>.ErrorResult(
                            _localizationService.GetLocalizedString("QuotationService.QuotationNotFound"),
                            _localizationService.GetLocalizedString("QuotationService.QuotationNotFound"),
                            StatusCodes.Status404NotFound);
                    }

                    var quotationLines = await _unitOfWork.QuotationLines.Query()
                    .Where(x => !x.IsDeleted && x.QuotationId == quotationId).ToListAsync();

                    var QuotationExchangeRates = await _unitOfWork.QuotationExchangeRates.Query()
                    .Where(x => !x.IsDeleted && x.QuotationId == quotationId).ToListAsync();
                    
                    var documentSerialTypeWithRevision = await _documentSerialTypeService.GenerateDocumentSerialAsync(quotation.DocumentSerialTypeId, false,quotation.RevisionNo);
                    if (!documentSerialTypeWithRevision.Success)
                    {
                        return ApiResponse<QuotationGetDto>.ErrorResult(
                            _localizationService.GetLocalizedString("QuotationService.DocumentSerialTypeGenerationError"),
                            documentSerialTypeWithRevision.Message,
                            StatusCodes.Status500InternalServerError);
                    }

                    var newQuotation = new Quotation();
                    newQuotation.OfferType = quotation.OfferType;
                    newQuotation.RevisionId = quotation.Id;
                    newQuotation.OfferDate = quotation.OfferDate;
                    newQuotation.OfferNo = quotation.OfferNo;
                    newQuotation.RevisionNo = documentSerialTypeWithRevision.Data;
                    newQuotation.OfferDate = quotation.OfferDate;
                    newQuotation.Currency = quotation.Currency;
                    newQuotation.Total = quotation.Total;
                    newQuotation.GrandTotal = quotation.GrandTotal;
                    newQuotation.CreatedBy = quotation.CreatedBy;
                    newQuotation.CreatedDate = DateTime.UtcNow;
                    newQuotation.PotentialCustomerId = quotation.PotentialCustomerId;
                    newQuotation.ErpCustomerCode = quotation.ErpCustomerCode;
                    newQuotation.ContactId = quotation.ContactId;
                    newQuotation.ValidUntil = quotation.ValidUntil;
                    newQuotation.DeliveryDate = quotation.DeliveryDate;
                    newQuotation.ShippingAddressId = quotation.ShippingAddressId;
                    newQuotation.RepresentativeId = quotation.RepresentativeId;
                    newQuotation.ActivityId = quotation.ActivityId;
                    newQuotation.Description = quotation.Description;
                    newQuotation.PaymentTypeId = quotation.PaymentTypeId;
                    newQuotation.HasCustomerSpecificDiscount = quotation.HasCustomerSpecificDiscount;
                    newQuotation.Status = (int)ApprovalStatus.HavenotStarted;

                    await _unitOfWork.Quotations.AddAsync(newQuotation);
                    await _unitOfWork.SaveChangesAsync();

                    var newQuotationLines = new List<QuotationLine>();
                    foreach (var line in quotationLines)
                    {
                        var newLine = new QuotationLine();
                        newLine.QuotationId = newQuotation.Id;
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
                        newQuotationLines.Add(newLine);
                    }
                    await _unitOfWork.QuotationLines.AddAllAsync(newQuotationLines);
                    await _unitOfWork.SaveChangesAsync();

                    var newQuotationExchangeRates = new List<QuotationExchangeRate>();
                    foreach (var exchangeRate in QuotationExchangeRates)
                    {
                        var newExchangeRate = new QuotationExchangeRate();
                        newExchangeRate.QuotationId = newQuotation.Id;
                        newExchangeRate.ExchangeRate = exchangeRate.ExchangeRate;
                        newExchangeRate.ExchangeRateDate = exchangeRate.ExchangeRateDate;
                        newExchangeRate.Currency = exchangeRate.Currency;
                        newExchangeRate.IsOfficial = exchangeRate.IsOfficial;
                        newQuotationExchangeRates.Add(newExchangeRate);
                    }
                    await _unitOfWork.QuotationExchangeRates.AddAllAsync(newQuotationExchangeRates);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();
                    var dtos  = _mapper.Map<QuotationGetDto>(newQuotation);
                    return ApiResponse<QuotationGetDto>.SuccessResult(dtos, _localizationService.GetLocalizedString("QuotationService.QuotationRevisionCreated"));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<QuotationGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.CreateRevisionOfQuotationExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
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
                            _localizationService.GetLocalizedString("QuotationService.UserNotFound"),
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

                var quotation = await _unitOfWork.Quotations.GetByIdAsync(request.EntityId);
                if (quotation == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationService.QuotationNotFound"),
                        "Teklif bulunamadı.",
                        StatusCodes.Status404NotFound);
                }
                quotation.Status = ApprovalStatus.Waiting;

                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();

                // Transaction'ı commit et
                await _unitOfWork.CommitTransactionAsync();

                // UserId -> ApprovalActionId eşlemesi (onay linkleri için)
                var userIdToActionId = actions.ToDictionary(a => a.ApprovedByUserId, a => a.Id);
                var baseUrl = _configuration["FrontendSettings:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:5173";
                var approvalPath = _configuration["FrontendSettings:ApprovalPendingPath"]?.TrimStart('/') ?? "approvals/pending";
                var quotationPath = _configuration["FrontendSettings:QuotationDetailPath"]?.TrimStart('/') ?? "quotations";
                var quotationLink = $"{baseUrl}/{quotationPath}/{request.EntityId}";

                // Onaya düşen her kullanıcıya "Onay bekleyen kaydınız bulunmaktadır" maili (Onayla/Reddet + Teklife Git butonları)
                var emailSubject = _localizationService.GetLocalizedString("QuotationService.PendingApprovalEmailSubject")
                    ?? "Onay Bekleyen Kaydınız Bulunmaktadır";
                foreach (var (email, fullName, uid) in usersToNotify)
                {
                    var displayName = string.IsNullOrWhiteSpace(fullName) ? "Değerli Kullanıcı" : fullName;
                    var actionId = userIdToActionId.GetValueOrDefault(uid);
                    var approvalLink = actionId != 0
                        ? $"{baseUrl}/{approvalPath}?actionId={actionId}"
                        : $"{baseUrl}/{approvalPath}";

                    var emailBody = $@"
                        <html>
                        <head>
                            <style>
                                body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                                .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                                .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                                .content {{ padding: 20px; background-color: #f9f9f9; }}
                                .btn {{ display: inline-block; padding: 12px 24px; color: white; text-decoration: none; border-radius: 6px; margin: 8px 4px; font-weight: bold; }}
                                .btn-approve {{ background-color: #4CAF50; }}
                                .btn-approve:hover {{ background-color: #45a049; }}
                                .btn-quotation {{ background-color: #2196F3; }}
                                .btn-quotation:hover {{ background-color: #0b7dda; }}
                                .buttons {{ margin: 20px 0; text-align: center; }}
                                .footer {{ padding: 20px; text-align: center; color: #666; font-size: 12px; }}
                            </style>
                        </head>
                        <body>
                            <div class=""container"">
                                <div class=""header"">
                                    <h2>Onay Bekleyen Kaydınız Bulunmaktadır</h2>
                                </div>
                                <div class=""content"">
                                    <p>Sayın {displayName},</p>
                                    <p>Onay bekleyen kaydınız bulunmaktadır. Aşağıdaki butonlarla onaylayabilir/reddedebilir veya teklife gidebilirsiniz.</p>
                                    <div class=""buttons"">
                                        <a href=""{approvalLink}"" class=""btn btn-approve"">Onayla / Reddet</a>
                                        <a href=""{quotationLink}"" class=""btn btn-quotation"">Teklife Git</a>
                                    </div>
                                </div>
                                <div class=""footer"">
                                    <p>Bu e-posta otomatik olarak gönderilmiştir, lütfen yanıtlamayınız.</p>
                                </div>
                            </div>
                        </body>
                        </html>";
                    BackgroundJob.Enqueue<IMailJob>(job =>
                        job.SendEmailAsync(email, emailSubject, emailBody, true, null, null, null));
                }

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

                // Quotation'ı al (hem akış bittiğinde hem de sonraki step için gerekli)
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

                    // QuotationLine'ların ApprovalStatus'unu Approved yap
                    var quotationLines = await _context.QuotationLines
                        .Where(ql => ql.QuotationId == quotation.Id && !ql.IsDeleted)
                        .ToListAsync();

                    foreach (var line in quotationLines)
                    {
                        line.ApprovalStatus = ApprovalStatus.Approved;
                        line.UpdatedDate = DateTime.UtcNow;
                        line.UpdatedBy = userId;
                        await _unitOfWork.QuotationLines.UpdateAsync(line);
                    }

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

                // Eğer reddeden kullanıcı teklifi oluşturan kullanıcıysa ve en alt aşamadaysa (CurrentStep == 1)
                // QuotationLine'ların ApprovalStatus'unu Rejected yap
                if (approvalRequest.CurrentStep == 1)
                {
                    var quotation = await _context.Quotations
                        .FirstOrDefaultAsync(q => q.Id == approvalRequest.EntityId && !q.IsDeleted);

                    if (quotation != null && quotation.CreatedBy == userId)
                    {
                        var quotationLines = await _context.QuotationLines
                            .Where(ql => ql.QuotationId == quotation.Id && !ql.IsDeleted)
                            .ToListAsync();

                        foreach (var line in quotationLines)
                        {
                            line.ApprovalStatus = ApprovalStatus.Rejected;
                            line.UpdatedDate = DateTime.UtcNow;
                            line.UpdatedBy = userId;
                            await _unitOfWork.QuotationLines.UpdateAsync(line);
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }
                }

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
