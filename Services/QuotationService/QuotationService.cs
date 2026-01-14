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
        private readonly IApprovalService _approvalService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuotationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            CmsDbContext context,
            IApprovalService approvalService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
            _approvalService = approvalService;
            _httpContextAccessor = httpContextAccessor;
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

                // 1. Düzenleme yetkisi kontrolü
                var canEdit = await _approvalService.CanUserEditQuotation(id, userId.Value);
                if (!canEdit)
                {
                    return ApiResponse<QuotationDto>.ErrorResult(
                        "Bu teklifi düzenleme yetkiniz yok. Teklif onay akışında olabilir.",
                        "You do not have permission to edit this quotation. It may be in approval process.",
                        StatusCodes.Status403Forbidden);
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

                // 2. Onay akışında mı kontrol et
                var isInApproval = await _approvalService.IsQuotationInApprovalProcess(id);
                if (isInApproval)
                {
                    return ApiResponse<QuotationDto>.ErrorResult(
                        "Onay akışındaki teklifler düzenlenemez.",
                        "Quotations in approval process cannot be edited",
                        StatusCodes.Status400BadRequest);
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

                // 5. Onay akışını yeniden başlat (gerekirse)
                await _approvalService.ProcessQuotationApproval(id);

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

                // 1. Silme yetkisi kontrolü
                var canDelete = await _approvalService.CanUserDeleteQuotation(id, userId.Value);
                if (!canDelete)
                {
                    return ApiResponse<object>.ErrorResult(
                        "Bu teklifi silme yetkiniz yok. Teklif onay akışında olabilir.",
                        "You do not have permission to delete this quotation. It may be in approval process.",
                        StatusCodes.Status403Forbidden);
                }

                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                if (quotation == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationNotFound"),
                        "Not found",
                        StatusCodes.Status404NotFound);
                }

                // 2. Onay akışında mı kontrol et
                var isInApproval = await _approvalService.IsQuotationInApprovalProcess(id);
                if (isInApproval)
                {
                    return ApiResponse<object>.ErrorResult(
                        "Onay akışındaki teklifler silinemez.",
                        "Quotations in approval process cannot be deleted",
                        StatusCodes.Status400BadRequest);
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

        public async Task<ApiResponse<CreateBulkQuotationResultDto>> CreateBulkQuotationAsync(CreateBulkQuotationDto dto)
        {
            try
            {
                // Start transaction
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var quotation = _mapper.Map<Quotation>(dto.Header);
                    quotation.OfferNo = "T";

                    await _unitOfWork.Quotations.AddAsync(quotation);
                    await _unitOfWork.SaveChangesAsync(); // Get ID

                    var createdLines = new List<QuotationLine>();
                    foreach (var line in dto.Lines)
                    {
                        var entity = new QuotationLine
                        {
                            QuotationId = quotation.Id,
                            ProductCode = line.ProductCode,
                            Quantity = line.Quantity,
                            UnitPrice = line.UnitPrice,
                            DiscountRate1 = line.DiscountRate1,
                            DiscountAmount1 = line.DiscountAmount1,
                            DiscountRate2 = line.DiscountRate2,
                            DiscountAmount2 = line.DiscountAmount2,
                            DiscountRate3 = line.DiscountRate3,
                            DiscountAmount3 = line.DiscountAmount3,
                            VatRate = line.VatRate,
                            VatAmount = line.VatAmount,
                            LineTotal = line.LineTotal,
                            LineGrandTotal = line.LineGrandTotal,
                            Description = line.Description,
                            CreatedDate = DateTime.UtcNow
                        };

                        await _unitOfWork.QuotationLines.AddAsync(entity);
                        createdLines.Add(entity);
                    }

                    quotation.Total = createdLines.Sum(x => x.LineTotal);
                    quotation.GrandTotal = createdLines.Sum(x => x.LineGrandTotal);
                    await _unitOfWork.Quotations.UpdateAsync(quotation);

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    // Start approval process
                    await _approvalService.ProcessQuotationApproval(quotation.Id);

                    var result = new CreateBulkQuotationResultDto
                    {
                        Quotation = _mapper.Map<QuotationDto>(quotation),
                        Lines = _mapper.Map<List<QuotationLineDto>>(createdLines)
                    };

                    return ApiResponse<CreateBulkQuotationResultDto>.SuccessResult(result, _localizationService.GetLocalizedString("QuotationService.QuotationCreated"));
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<CreateBulkQuotationResultDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationService.CreateBulkQuotationExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<QuotationGetDto>> CreateQuotationBulkAsync(QuotationBulkCreateDto bulkDto)
        {
            try
            {
                // Start transaction
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    // 1. Quotation Header'ı oluştur
                    var quotation = _mapper.Map<Quotation>(bulkDto.Quotation);

                    // 2. Lines toplamını hesapla (GrandTotal için)
                    decimal total = 0m;
                    decimal grandTotal = 0m;

                    foreach (var lineDto in bulkDto.Lines)
                    {
                        var lineTotal = (lineDto.Quantity * lineDto.UnitPrice)
                            - (lineDto.DiscountAmount1 + lineDto.DiscountAmount2 + lineDto.DiscountAmount3)
                            - ((lineDto.Quantity * lineDto.UnitPrice) * (lineDto.DiscountRate1 + lineDto.DiscountRate2 + lineDto.DiscountRate3) / 100);

                        var vatAmount = lineTotal * (lineDto.VatRate / 100);
                        var lineGrandTotal = lineTotal + vatAmount;

                        total += lineTotal;
                        grandTotal += lineGrandTotal;
                    }

                    quotation.Total = total;
                    quotation.GrandTotal = grandTotal;

                    // 3. Quotation'ı kaydet
                    await _unitOfWork.Quotations.AddAsync(quotation);
                    await _unitOfWork.SaveChangesAsync(); // ID'yi almak için

                    // 4. Lines'ı bulk insert et
                    var lines = new List<QuotationLine>();
                    foreach (var lineDto in bulkDto.Lines)
                    {
                        var line = _mapper.Map<QuotationLine>(lineDto);
                        line.QuotationId = quotation.Id;

                        // Line toplamlarını hesapla
                        line.LineTotal = (line.Quantity * line.UnitPrice)
                            - (line.DiscountAmount1 + line.DiscountAmount2 + line.DiscountAmount3)
                            - ((line.Quantity * line.UnitPrice) * (line.DiscountRate1 + line.DiscountRate2 + line.DiscountRate3) / 100);

                        line.VatAmount = line.LineTotal * (line.VatRate / 100);
                        line.LineGrandTotal = line.LineTotal + line.VatAmount;

                        lines.Add(line);
                    }

                    foreach (var line in lines)
                    {
                        await _unitOfWork.QuotationLines.AddAsync(line);
                    }

                    // 5. ExchangeRate'leri bulk insert et (varsa)
                    if (bulkDto.ExchangeRates != null && bulkDto.ExchangeRates.Any())
                    {
                        foreach (var rateDto in bulkDto.ExchangeRates)
                        {
                            var exchangeRate = _mapper.Map<QuotationExchangeRate>(rateDto);
                            exchangeRate.QuotationId = quotation.Id;
                            await _unitOfWork.QuotationExchangeRates.AddAsync(exchangeRate);
                        }
                    }

                    // 6. Transaction commit
                    await _unitOfWork.CommitTransactionAsync();

                    // 7. Onay akışını başlat
                    await _approvalService.ProcessQuotationApproval(quotation.Id);

                    // 8. Reload with navigation properties
                    var quotationWithNav = await _unitOfWork.Quotations
                        .Query()
                        .Include(q => q.Representative)
                        .Include(q => q.Lines)
                        .Include(q => q.PotentialCustomer)
                        .Include(q => q.CreatedByUser)
                        .Include(q => q.UpdatedByUser)
                        .FirstOrDefaultAsync(q => q.Id == quotation.Id);

                    var quotationDto = _mapper.Map<QuotationGetDto>(quotationWithNav);
                    return ApiResponse<QuotationGetDto>.SuccessResult(quotationDto, "Quotation created successfully");
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
        
        public async Task<ApiResponse<List<PricingRuleLineGetDto>>> GetPriceRuleOfQuotationAsync(string customerCode,long salesmenId,DateTime quotationDate)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
                short branchCode = short.Parse(branchCodeStr!);

                PricingRuleHeader? header = null;

                // 1️⃣ Satışçı + Cari
                header = await _unitOfWork.PricingRuleHeaders.Query()
                .AsNoTracking()
                    .Where(x =>
                        x.IsActive &&
                        !x.IsDeleted &&
                        x.ValidFrom <= quotationDate &&
                        x.ValidTo >= quotationDate &&
                        x.ErpCustomerCode == customerCode &&
                        x.BranchCode == branchCode &&
                        x.Salesmen.Any(s => s.SalesmanId == salesmenId && !s.IsDeleted)
                    )
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                // 2️⃣ Satışçı var – Cari yok
                if (header == null)
                {
                    header = await _unitOfWork.PricingRuleHeaders.Query()
                        .AsNoTracking()
                        .Where(x =>
                            x.IsActive &&
                            !x.IsDeleted &&
                            x.ValidFrom <= quotationDate &&
                            x.ValidTo >= quotationDate &&
                            (x.ErpCustomerCode == null || x.ErpCustomerCode == "") &&
                            x.BranchCode == branchCode &&
                            x.Salesmen.Any(s => s.SalesmanId == salesmenId && !s.IsDeleted)
                        )
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefaultAsync();
                }

                // 3️⃣ Satışçı yok – Cari yok – Ürün bazlı
                if (header == null)
                {
                    header = await _unitOfWork.PricingRuleHeaders.Query()
                        .AsNoTracking()
                        .Where(x =>
                            x.IsActive &&
                            !x.IsDeleted &&
                            x.ValidFrom <= quotationDate &&
                            x.ValidTo >= quotationDate &&
                            (x.ErpCustomerCode == null || x.ErpCustomerCode == "") &&
                            x.BranchCode == branchCode &&
                            x.Salesmen.Count == 0
                        )
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefaultAsync();
                }

                    if (header == null)
                    {
                        return ApiResponse<List<PricingRuleLineGetDto>>.SuccessResult(
                            new List<PricingRuleLineGetDto>(), _localizationService.GetLocalizedString("QuotationService.PriceRuleOfQuotationRetrieved"));
                    }

                    var lines = await _unitOfWork.PricingRuleLines.Query()
                        .AsNoTracking()
                        .Where(x =>
                            x.PricingRuleHeaderId == header.Id &&
                            !x.IsDeleted
                        )
                        .ToListAsync();

                var dto = _mapper.Map<List<PricingRuleLineGetDto>>(lines);

                return ApiResponse<List<PricingRuleLineGetDto>>.SuccessResult(dto, _localizationService.GetLocalizedString("QuotationService.PriceRuleOfQuotationRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PricingRuleLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
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
    }
}
