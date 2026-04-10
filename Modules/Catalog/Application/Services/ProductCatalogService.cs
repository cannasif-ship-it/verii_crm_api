using AutoMapper;
using crm_api.Helpers;
using crm_api.Modules.Catalog.Domain.Entities;
using crm_api.Modules.Catalog.Domain.Enums;
using crm_api.Modules.Identity.Application.Services;
using crm_api.Shared.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using StockEntity = crm_api.Modules.Stock.Domain.Entities.Stock;

namespace crm_api.Modules.Catalog.Application.Services
{
    public class ProductCatalogService : IProductCatalogService
    {
        private static readonly string[] StockSearchableColumns =
        [
            nameof(StockEntity.ErpStockCode),
            nameof(StockEntity.StockName),
            nameof(StockEntity.Unit),
            nameof(StockEntity.GrupKodu),
            nameof(StockEntity.GrupAdi),
            nameof(StockEntity.Kod1),
            nameof(StockEntity.Kod1Adi),
            nameof(StockEntity.Kod2),
            nameof(StockEntity.Kod2Adi),
            nameof(StockEntity.Kod3),
            nameof(StockEntity.Kod3Adi)
        ];

        private readonly CmsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IUserContextService _userContextService;
        private readonly IFileUploadService _fileUploadService;

        public ProductCatalogService(
            CmsDbContext dbContext,
            IMapper mapper,
            ILocalizationService localizationService,
            IUserContextService userContextService,
            IFileUploadService fileUploadService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizationService = localizationService;
            _userContextService = userContextService;
            _fileUploadService = fileUploadService;
        }

        public async Task<ApiResponse<string>> UploadCategoryImageAsync(IFormFile file)
        {
            return await _fileUploadService.UploadCategoryImageAsync(file).ConfigureAwait(false);
        }

        public async Task<ApiResponse<List<ProductCatalogGetDto>>> GetCatalogsAsync()
        {
            try
            {
                var catalogs = await _dbContext.ProductCatalogs
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var data = catalogs.Select(_mapper.Map<ProductCatalogGetDto>).ToList();
                return ApiResponse<List<ProductCatalogGetDto>>.SuccessResult(
                    data,
                    _localizationService.GetLocalizedString("ProductCatalogService.CatalogsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductCatalogGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.GetCatalogsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductCatalogGetDto>> GetCatalogByIdAsync(long id)
        {
            try
            {
                var catalog = await _dbContext.ProductCatalogs
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (catalog == null)
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<ProductCatalogGetDto>.SuccessResult(
                    _mapper.Map<ProductCatalogGetDto>(catalog),
                    _localizationService.GetLocalizedString("ProductCatalogService.CatalogRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.GetCatalogByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductCatalogGetDto>> CreateCatalogAsync(ProductCatalogCreateDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var name = request.Name?.Trim();
                var code = request.Code?.Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var exists = await _dbContext.ProductCatalogs
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Code == code && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (exists)
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogCodeAlreadyExists"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogCodeAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                var entity = _mapper.Map<ProductCatalog>(request);
                entity.Name = name;
                entity.Code = code;
                entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                entity.CreatedBy = _userContextService.GetCurrentUserId();

                await _dbContext.ProductCatalogs.AddAsync(entity).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var created = await _dbContext.ProductCatalogs
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstAsync(x => x.Id == entity.Id)
                    .ConfigureAwait(false);

                return ApiResponse<ProductCatalogGetDto>.SuccessResult(
                    _mapper.Map<ProductCatalogGetDto>(created),
                    _localizationService.GetLocalizedString("ProductCatalogService.CatalogCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.CreateCatalogExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductCatalogGetDto>> UpdateCatalogAsync(long id, ProductCatalogUpdateDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var catalog = await _dbContext.ProductCatalogs
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (catalog == null)
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var name = request.Name?.Trim();
                var code = request.Code?.Trim().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var duplicateCode = await _dbContext.ProductCatalogs
                    .AnyAsync(x => x.Id != id && x.Code == code && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (duplicateCode)
                {
                    return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogCodeAlreadyExists"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogCodeAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                catalog.Name = name;
                catalog.Code = code;
                catalog.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                catalog.CatalogType = request.CatalogType;
                catalog.BranchCode = request.BranchCode;
                catalog.SortOrder = request.SortOrder;
                catalog.UpdatedDate = DateTimeProvider.Now;
                catalog.UpdatedBy = _userContextService.GetCurrentUserId();

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var updated = await _dbContext.ProductCatalogs
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstAsync(x => x.Id == id)
                    .ConfigureAwait(false);

                return ApiResponse<ProductCatalogGetDto>.SuccessResult(
                    _mapper.Map<ProductCatalogGetDto>(updated),
                    _localizationService.GetLocalizedString("ProductCatalogService.CatalogUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductCatalogGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.UpdateCatalogExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteCatalogAsync(long id)
        {
            try
            {
                var catalog = await _dbContext.ProductCatalogs
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (catalog == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var hasCategories = await _dbContext.CatalogCategories
                    .AnyAsync(x => x.CatalogId == id && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (hasCategories)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogHasCategories"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogHasCategories"),
                        StatusCodes.Status400BadRequest);
                }

                catalog.IsDeleted = true;
                catalog.DeletedDate = DateTimeProvider.Now;
                catalog.DeletedBy = _userContextService.GetCurrentUserId();
                catalog.UpdatedDate = DateTimeProvider.Now;
                catalog.UpdatedBy = _userContextService.GetCurrentUserId();

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("ProductCatalogService.CatalogDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.DeleteCatalogExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<CatalogCategoryNodeDto>>> GetCatalogCategoriesAsync(long catalogId, long? parentCatalogCategoryId)
        {
            try
            {
                var catalogExists = await _dbContext.ProductCatalogs
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (!catalogExists)
                {
                    return ApiResponse<List<CatalogCategoryNodeDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var nodes = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Where(x => x.CatalogId == catalogId &&
                                x.ParentCatalogCategoryId == parentCatalogCategoryId &&
                                !x.IsDeleted &&
                                !x.Category.IsDeleted)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Category.Name)
                    .Select(x => new CatalogCategoryNodeDto
                    {
                        CatalogCategoryId = x.Id,
                        CategoryId = x.CategoryId,
                        ParentCatalogCategoryId = x.ParentCatalogCategoryId,
                        Name = x.Category.Name,
                        Code = x.Category.Code,
                        Description = x.Category.Description,
                        Level = x.Category.Level,
                        FullPath = x.Category.FullPath,
                        IsLeaf = x.Category.IsLeaf,
                        HasChildren = _dbContext.CatalogCategories.Any(child => child.ParentCatalogCategoryId == x.Id && !child.IsDeleted),
                        SortOrder = x.SortOrder,
                        VisualPreset = x.Category.VisualPreset,
                        ImageUrl = x.Category.ImageUrl,
                        IconName = x.Category.IconName,
                        ColorHex = x.Category.ColorHex
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

                return ApiResponse<List<CatalogCategoryNodeDto>>.SuccessResult(
                    nodes,
                    _localizationService.GetLocalizedString("ProductCatalogService.CategoriesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CatalogCategoryNodeDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.GetCatalogCategoriesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CatalogCategoryNodeDto>> CreateCatalogCategoryAsync(long catalogId, CatalogCategoryCreateDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var catalog = await _dbContext.ProductCatalogs
                    .FirstOrDefaultAsync(x => x.Id == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (catalog == null)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CatalogNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var name = request.Name?.Trim();
                var code = request.Code?.Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var parentCatalogCategory = request.ParentCatalogCategoryId.HasValue
                    ? await _dbContext.CatalogCategories
                        .Include(x => x.Category)
                        .FirstOrDefaultAsync(x => x.Id == request.ParentCatalogCategoryId.Value &&
                                                  x.CatalogId == catalogId &&
                                                  !x.IsDeleted &&
                                                  !x.Category.IsDeleted)
                        .ConfigureAwait(false)
                    : null;

                if (request.ParentCatalogCategoryId.HasValue && parentCatalogCategory == null)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.ParentCategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.ParentCategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var categoryCodeExists = await _dbContext.ProductCategories
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Code == code && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryCodeExists)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryCodeAlreadyExists"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryCodeAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                var userId = _userContextService.GetCurrentUserId();
                var parentCategory = parentCatalogCategory?.Category;

                var category = new ProductCategory
                {
                    ParentCategoryId = parentCategory?.Id,
                    Name = name,
                    Code = code,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    Level = parentCategory != null ? parentCategory.Level + 1 : 1,
                    FullPath = parentCategory != null
                        ? $"{parentCategory.FullPath ?? parentCategory.Name}/{name}"
                        : name,
                    SortOrder = request.SortOrder,
                    VisualPreset = request.VisualPreset,
                    ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim(),
                    IsLeaf = request.IsLeaf,
                    CreatedBy = userId
                };

                ApplyVisualPreset(category, request.VisualPreset);

                await _dbContext.ProductCategories.AddAsync(category).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var catalogCategory = new CatalogCategory
                {
                    CatalogId = catalogId,
                    CategoryId = category.Id,
                    ParentCatalogCategoryId = request.ParentCatalogCategoryId,
                    SortOrder = request.SortOrder,
                    IsRoot = !request.ParentCatalogCategoryId.HasValue,
                    CreatedBy = userId
                };

                await _dbContext.CatalogCategories.AddAsync(catalogCategory).ConfigureAwait(false);

                if (parentCategory != null && parentCategory.IsLeaf)
                {
                    parentCategory.IsLeaf = false;
                    parentCategory.UpdatedDate = DateTimeProvider.Now;
                    parentCategory.UpdatedBy = userId;
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var createdNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Where(x => x.Id == catalogCategory.Id && !x.IsDeleted && !x.Category.IsDeleted)
                    .Select(x => new CatalogCategoryNodeDto
                    {
                        CatalogCategoryId = x.Id,
                        CategoryId = x.CategoryId,
                        ParentCatalogCategoryId = x.ParentCatalogCategoryId,
                        Name = x.Category.Name,
                        Code = x.Category.Code,
                        Description = x.Category.Description,
                        Level = x.Category.Level,
                        FullPath = x.Category.FullPath,
                        IsLeaf = x.Category.IsLeaf,
                        HasChildren = false,
                        SortOrder = x.SortOrder,
                        VisualPreset = x.Category.VisualPreset,
                        ImageUrl = x.Category.ImageUrl,
                        IconName = x.Category.IconName,
                        ColorHex = x.Category.ColorHex
                    })
                    .FirstAsync()
                    .ConfigureAwait(false);

                return ApiResponse<CatalogCategoryNodeDto>.SuccessResult(
                    createdNode,
                    _localizationService.GetLocalizedString("ProductCatalogService.CategoryCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.CreateCategoryExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CatalogCategoryNodeDto>> UpdateCatalogCategoryAsync(long catalogId, long catalogCategoryId, CatalogCategoryUpdateDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var catalogCategory = await _dbContext.CatalogCategories
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId &&
                                              x.CatalogId == catalogId &&
                                              !x.IsDeleted &&
                                              !x.Category.IsDeleted)
                    .ConfigureAwait(false);

                if (catalogCategory == null)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var name = request.Name?.Trim();
                var code = request.Code?.Trim().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        _localizationService.GetLocalizedString("ProductCatalogService.NameAndCodeRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var hasChildren = await _dbContext.CatalogCategories
                    .AnyAsync(x => x.ParentCatalogCategoryId == catalogCategoryId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (request.IsLeaf && hasChildren)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.LeafCategoryCannotHaveChildren"),
                        _localizationService.GetLocalizedString("ProductCatalogService.LeafCategoryCannotHaveChildren"),
                        StatusCodes.Status400BadRequest);
                }

                var duplicateCode = await _dbContext.ProductCategories
                    .AnyAsync(x => x.Id != catalogCategory.CategoryId && x.Code == code && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (duplicateCode)
                {
                    return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryCodeAlreadyExists"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryCodeAlreadyExists"),
                        StatusCodes.Status400BadRequest);
                }

                var category = catalogCategory.Category;
                category.Name = name;
                category.Code = code;
                category.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                category.SortOrder = request.SortOrder;
                category.VisualPreset = request.VisualPreset;
                category.ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();
                category.IsLeaf = request.IsLeaf;
                category.FullPath = await BuildCategoryPathAsync(category.ParentCategoryId, category.Name).ConfigureAwait(false);
                category.UpdatedDate = DateTimeProvider.Now;
                category.UpdatedBy = _userContextService.GetCurrentUserId();
                ApplyVisualPreset(category, request.VisualPreset);

                catalogCategory.SortOrder = request.SortOrder;
                catalogCategory.UpdatedDate = DateTimeProvider.Now;
                catalogCategory.UpdatedBy = _userContextService.GetCurrentUserId();

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var updatedNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Where(x => x.Id == catalogCategoryId && !x.IsDeleted && !x.Category.IsDeleted)
                    .Select(x => new CatalogCategoryNodeDto
                    {
                        CatalogCategoryId = x.Id,
                        CategoryId = x.CategoryId,
                        ParentCatalogCategoryId = x.ParentCatalogCategoryId,
                        Name = x.Category.Name,
                        Code = x.Category.Code,
                        Description = x.Category.Description,
                        Level = x.Category.Level,
                        FullPath = x.Category.FullPath,
                        IsLeaf = x.Category.IsLeaf,
                        HasChildren = _dbContext.CatalogCategories.Any(child => child.ParentCatalogCategoryId == x.Id && !child.IsDeleted),
                        SortOrder = x.SortOrder,
                        VisualPreset = x.Category.VisualPreset,
                        ImageUrl = x.Category.ImageUrl,
                        IconName = x.Category.IconName,
                        ColorHex = x.Category.ColorHex
                    })
                    .FirstAsync()
                    .ConfigureAwait(false);

                return ApiResponse<CatalogCategoryNodeDto>.SuccessResult(
                    updatedNode,
                    _localizationService.GetLocalizedString("ProductCatalogService.CategoryUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CatalogCategoryNodeDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.UpdateCategoryExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> ReorderCatalogCategoriesAsync(long catalogId, CatalogCategoryReorderDto request)
        {
            try
            {
                if (request == null || request.OrderedCatalogCategoryIds.Count == 0)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var siblings = await _dbContext.CatalogCategories
                    .Where(x => x.CatalogId == catalogId &&
                                x.ParentCatalogCategoryId == request.ParentCatalogCategoryId &&
                                !x.IsDeleted)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (siblings.Count == 0)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var siblingIds = siblings.Select(x => x.Id).OrderBy(x => x).ToList();
                var requestIds = request.OrderedCatalogCategoryIds.Distinct().OrderBy(x => x).ToList();
                if (!siblingIds.SequenceEqual(requestIds))
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidCategoryReorderPayload"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidCategoryReorderPayload"),
                        StatusCodes.Status400BadRequest);
                }

                var userId = _userContextService.GetCurrentUserId();
                for (var index = 0; index < request.OrderedCatalogCategoryIds.Count; index++)
                {
                    var targetId = request.OrderedCatalogCategoryIds[index];
                    var sibling = siblings.First(x => x.Id == targetId);
                    sibling.SortOrder = index;
                    sibling.UpdatedDate = DateTimeProvider.Now;
                    sibling.UpdatedBy = userId;
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("ProductCatalogService.CategoryOrderUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.ReorderCategoryExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteCatalogCategoryAsync(long catalogId, long catalogCategoryId)
        {
            try
            {
                var catalogCategory = await _dbContext.CatalogCategories
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId &&
                                              x.CatalogId == catalogId &&
                                              !x.IsDeleted &&
                                              !x.Category.IsDeleted)
                    .ConfigureAwait(false);

                if (catalogCategory == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var hasChildren = await _dbContext.CatalogCategories
                    .AnyAsync(x => x.ParentCatalogCategoryId == catalogCategoryId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (hasChildren)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryHasChildren"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryHasChildren"),
                        StatusCodes.Status400BadRequest);
                }

                var hasStockAssignments = await _dbContext.StockCategories
                    .AnyAsync(x => x.CategoryId == catalogCategory.CategoryId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (hasStockAssignments)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryHasStocks"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryHasStocks"),
                        StatusCodes.Status400BadRequest);
                }

                var userId = _userContextService.GetCurrentUserId();

                catalogCategory.IsDeleted = true;
                catalogCategory.DeletedDate = DateTimeProvider.Now;
                catalogCategory.DeletedBy = userId;
                catalogCategory.UpdatedDate = DateTimeProvider.Now;
                catalogCategory.UpdatedBy = userId;

                var category = catalogCategory.Category;
                category.IsDeleted = true;
                category.DeletedDate = DateTimeProvider.Now;
                category.DeletedBy = userId;
                category.UpdatedDate = DateTimeProvider.Now;
                category.UpdatedBy = userId;

                if (catalogCategory.ParentCatalogCategoryId.HasValue)
                {
                    var siblingCount = await _dbContext.CatalogCategories
                        .CountAsync(x => x.ParentCatalogCategoryId == catalogCategory.ParentCatalogCategoryId &&
                                         x.Id != catalogCategoryId &&
                                         !x.IsDeleted)
                        .ConfigureAwait(false);

                    if (siblingCount == 0)
                    {
                        var parentCatalogCategory = await _dbContext.CatalogCategories
                            .Include(x => x.Category)
                            .FirstOrDefaultAsync(x => x.Id == catalogCategory.ParentCatalogCategoryId.Value && !x.IsDeleted)
                            .ConfigureAwait(false);

                        if (parentCatalogCategory?.Category != null)
                        {
                            parentCatalogCategory.Category.IsLeaf = true;
                            parentCatalogCategory.Category.UpdatedDate = DateTimeProvider.Now;
                            parentCatalogCategory.Category.UpdatedBy = userId;
                        }
                    }
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("ProductCatalogService.CategoryDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.DeleteCategoryExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<ProductCategoryRuleGetDto>>> GetCategoryRulesAsync(long catalogId, long catalogCategoryId)
        {
            try
            {
                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<List<ProductCategoryRuleGetDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var rules = await _dbContext.ProductCategoryRules
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .Where(x => x.CategoryId == categoryNode.CategoryId && !x.IsDeleted)
                    .OrderBy(x => x.Priority)
                    .ThenBy(x => x.RuleName)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return ApiResponse<List<ProductCategoryRuleGetDto>>.SuccessResult(
                    rules.Select(_mapper.Map<ProductCategoryRuleGetDto>).ToList(),
                    _localizationService.GetLocalizedString("ProductCatalogService.RulesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductCategoryRuleGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.GetCategoryRulesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<List<CategoryRuleValueOptionDto>>> GetCategoryRuleValueOptionsAsync(long catalogId, long catalogCategoryId, StockAttributeType stockAttributeType, string? search)
        {
            try
            {
                var categoryNodeExists = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (!categoryNodeExists)
                {
                    return ApiResponse<List<CategoryRuleValueOptionDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var normalizedSearch = search?.Trim();

                var stockAttributeQuery = GetStockAttributeQuery(stockAttributeType);

                var options = await stockAttributeQuery
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x!.Trim())
                    .Where(x => normalizedSearch == null || EF.Functions.Like(x, $"%{normalizedSearch}%"))
                    .GroupBy(x => x)
                    .Select(x => new CategoryRuleValueOptionDto
                    {
                        Value = x.Key,
                        UsageCount = x.Count()
                    })
                    .OrderByDescending(x => x.UsageCount)
                    .ThenBy(x => x.Value)
                    .Take(100)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return ApiResponse<List<CategoryRuleValueOptionDto>>.SuccessResult(
                    options,
                    _localizationService.GetLocalizedString("ProductCatalogService.RuleValueOptionsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CategoryRuleValueOptionDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.GetCategoryRuleValueOptionsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductCategoryRuleGetDto>> CreateCategoryRuleAsync(long catalogId, long catalogCategoryId, ProductCategoryRuleCreateDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var ruleName = request.RuleName?.Trim();
                var ruleValue = request.Value?.Trim();
                var ruleCode = request.RuleCode?.Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(ruleName) || string.IsNullOrWhiteSpace(ruleValue))
                {
                    return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNameAndValueRequired"),
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNameAndValueRequired"),
                        StatusCodes.Status400BadRequest);
                }

                var entity = new ProductCategoryRule
                {
                    CategoryId = categoryNode.CategoryId,
                    RuleName = ruleName,
                    RuleCode = string.IsNullOrWhiteSpace(ruleCode) ? null : ruleCode,
                    StockAttributeType = request.StockAttributeType,
                    OperatorType = request.OperatorType,
                    Value = ruleValue,
                    Priority = request.Priority,
                    CreatedBy = _userContextService.GetCurrentUserId()
                };

                await _dbContext.ProductCategoryRules.AddAsync(entity).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var created = await _dbContext.ProductCategoryRules
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstAsync(x => x.Id == entity.Id)
                    .ConfigureAwait(false);

                return ApiResponse<ProductCategoryRuleGetDto>.SuccessResult(
                    _mapper.Map<ProductCategoryRuleGetDto>(created),
                    _localizationService.GetLocalizedString("ProductCatalogService.RuleCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.CreateCategoryRuleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductCategoryRuleGetDto>> UpdateCategoryRuleAsync(long catalogId, long catalogCategoryId, long ruleId, ProductCategoryRuleUpdateDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var rule = await _dbContext.ProductCategoryRules
                    .FirstOrDefaultAsync(x => x.Id == ruleId && x.CategoryId == categoryNode.CategoryId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (rule == null)
                {
                    return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var ruleName = request.RuleName?.Trim();
                var ruleValue = request.Value?.Trim();
                var ruleCode = request.RuleCode?.Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(ruleName) || string.IsNullOrWhiteSpace(ruleValue))
                {
                    return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNameAndValueRequired"),
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNameAndValueRequired"),
                        StatusCodes.Status400BadRequest);
                }

                rule.RuleName = ruleName;
                rule.RuleCode = string.IsNullOrWhiteSpace(ruleCode) ? null : ruleCode;
                rule.StockAttributeType = request.StockAttributeType;
                rule.OperatorType = request.OperatorType;
                rule.Value = ruleValue;
                rule.Priority = request.Priority;
                rule.UpdatedDate = DateTimeProvider.Now;
                rule.UpdatedBy = _userContextService.GetCurrentUserId();

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var updated = await _dbContext.ProductCategoryRules
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstAsync(x => x.Id == rule.Id)
                    .ConfigureAwait(false);

                return ApiResponse<ProductCategoryRuleGetDto>.SuccessResult(
                    _mapper.Map<ProductCategoryRuleGetDto>(updated),
                    _localizationService.GetLocalizedString("ProductCatalogService.RuleUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductCategoryRuleGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.UpdateCategoryRuleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteCategoryRuleAsync(long catalogId, long catalogCategoryId, long ruleId)
        {
            try
            {
                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var rule = await _dbContext.ProductCategoryRules
                    .FirstOrDefaultAsync(x => x.Id == ruleId && x.CategoryId == categoryNode.CategoryId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (rule == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleNotFound"),
                        StatusCodes.Status404NotFound);
                }

                rule.IsDeleted = true;
                rule.DeletedDate = DateTimeProvider.Now;
                rule.DeletedBy = _userContextService.GetCurrentUserId();
                rule.UpdatedDate = DateTimeProvider.Now;
                rule.UpdatedBy = _userContextService.GetCurrentUserId();

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("ProductCatalogService.RuleDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.DeleteCategoryRuleExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CategoryRulePreviewResultDto>> PreviewCategoryRulesAsync(long catalogId, long catalogCategoryId)
        {
            try
            {
                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<CategoryRulePreviewResultDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var rules = await _dbContext.ProductCategoryRules
                    .AsNoTracking()
                    .Where(x => x.CategoryId == categoryNode.CategoryId && !x.IsDeleted)
                    .OrderBy(x => x.Priority)
                    .ThenBy(x => x.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (rules.Count == 0)
                {
                    return ApiResponse<CategoryRulePreviewResultDto>.SuccessResult(
                        new CategoryRulePreviewResultDto(),
                        _localizationService.GetLocalizedString("ProductCatalogService.RulePreviewCompleted"));
                }

                var allStocks = await _dbContext.Stocks
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var stockIds = allStocks.Select(x => x.Id).ToList();
                var existingAssignments = await _dbContext.StockCategories
                    .AsNoTracking()
                    .Where(x => stockIds.Contains(x.StockId) && !x.IsDeleted)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var assignmentsByStock = existingAssignments
                    .GroupBy(x => x.StockId)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var result = new CategoryRulePreviewResultDto();

                foreach (var stock in allStocks)
                {
                    var matchedRule = rules.FirstOrDefault(rule => IsRuleMatched(stock, rule));
                    if (matchedRule == null)
                    {
                        continue;
                    }

                    result.MatchedStockCount++;

                    assignmentsByStock.TryGetValue(stock.Id, out var stockAssignments);
                    stockAssignments ??= [];

                    var existingForCategory = stockAssignments.FirstOrDefault(x => x.CategoryId == categoryNode.CategoryId && !x.IsDeleted);

                    CategoryRulePreviewActionType actionType;
                    if (existingForCategory != null &&
                        existingForCategory.AssignmentType is StockCategoryAssignmentType.Manual or StockCategoryAssignmentType.ManualOverride)
                    {
                        result.SkippedManualAssignmentCount++;
                        actionType = CategoryRulePreviewActionType.SkipManualAssignment;
                    }
                    else if (existingForCategory != null)
                    {
                        result.UpdatedAssignmentCount++;
                        actionType = CategoryRulePreviewActionType.UpdateRuleAssignment;
                    }
                    else
                    {
                        result.CreatedAssignmentCount++;
                        actionType = CategoryRulePreviewActionType.CreateAssignment;
                    }

                    if (result.PreviewItems.Count < 12)
                    {
                        result.PreviewItems.Add(new CategoryRulePreviewItemDto
                        {
                            StockId = stock.Id,
                            ErpStockCode = stock.ErpStockCode,
                            StockName = stock.StockName,
                            ExistingStockCategoryId = existingForCategory?.Id,
                            MatchedRuleName = matchedRule.RuleName,
                            MatchedRuleCode = matchedRule.RuleCode,
                            Priority = matchedRule.Priority,
                            ActionType = actionType
                        });
                    }
                }

                return ApiResponse<CategoryRulePreviewResultDto>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("ProductCatalogService.RulePreviewCompleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryRulePreviewResultDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.PreviewCategoryRulesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CategoryRuleApplyResultDto>> ApplyCategoryRulesAsync(long catalogId, long catalogCategoryId)
        {
            try
            {
                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<CategoryRuleApplyResultDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var rules = await _dbContext.ProductCategoryRules
                    .AsNoTracking()
                    .Where(x => x.CategoryId == categoryNode.CategoryId && !x.IsDeleted)
                    .OrderBy(x => x.Priority)
                    .ThenBy(x => x.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (rules.Count == 0)
                {
                    return ApiResponse<CategoryRuleApplyResultDto>.SuccessResult(
                        new CategoryRuleApplyResultDto(),
                        _localizationService.GetLocalizedString("ProductCatalogService.RuleApplyCompleted"));
                }

                var allStocks = await _dbContext.Stocks
                    .Where(x => !x.IsDeleted)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var stockIds = allStocks.Select(x => x.Id).ToList();
                var existingAssignments = await _dbContext.StockCategories
                    .Where(x => stockIds.Contains(x.StockId) && !x.IsDeleted)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var assignmentsByStock = existingAssignments
                    .GroupBy(x => x.StockId)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var result = new CategoryRuleApplyResultDto();
                var userId = _userContextService.GetCurrentUserId();

                foreach (var stock in allStocks)
                {
                    var matchedRule = rules.FirstOrDefault(rule => IsRuleMatched(stock, rule));
                    if (matchedRule == null)
                    {
                        continue;
                    }

                    result.MatchedStockCount++;

                    assignmentsByStock.TryGetValue(stock.Id, out var stockAssignments);
                    stockAssignments ??= new List<StockCategory>();

                    var existingForCategory = stockAssignments.FirstOrDefault(x => x.CategoryId == categoryNode.CategoryId && !x.IsDeleted);

                    if (existingForCategory != null)
                    {
                        if (existingForCategory.AssignmentType is StockCategoryAssignmentType.Manual or StockCategoryAssignmentType.ManualOverride)
                        {
                            result.SkippedManualAssignmentCount++;
                            continue;
                        }

                        existingForCategory.RuleId = matchedRule.Id;
                        existingForCategory.AssignmentType = StockCategoryAssignmentType.RuleResolved;
                        existingForCategory.SortOrder = matchedRule.Priority;
                        existingForCategory.Note = $"Applied by rule: {matchedRule.RuleName}";
                        existingForCategory.UpdatedDate = DateTimeProvider.Now;
                        existingForCategory.UpdatedBy = userId;
                        result.UpdatedAssignmentCount++;
                        continue;
                    }

                    var hasPrimary = stockAssignments.Any(x => x.IsPrimary && !x.IsDeleted);
                    var newAssignment = new StockCategory
                    {
                        StockId = stock.Id,
                        CategoryId = categoryNode.CategoryId,
                        AssignmentType = StockCategoryAssignmentType.RuleResolved,
                        RuleId = matchedRule.Id,
                        IsPrimary = !hasPrimary,
                        SortOrder = matchedRule.Priority,
                        Note = $"Applied by rule: {matchedRule.RuleName}",
                        CreatedBy = userId
                    };

                    await _dbContext.StockCategories.AddAsync(newAssignment).ConfigureAwait(false);
                    stockAssignments.Add(newAssignment);
                    assignmentsByStock[stock.Id] = stockAssignments;
                    result.CreatedAssignmentCount++;
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<CategoryRuleApplyResultDto>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("ProductCatalogService.RuleApplyCompleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryRuleApplyResultDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.ApplyCategoryRulesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CatalogStockItemDto>> CreateStockCategoryAssignmentAsync(long catalogId, long catalogCategoryId, StockCategoryCreateDto request)
        {
            try
            {
                if (request == null || request.StockId <= 0)
                {
                    return ApiResponse<CatalogStockItemDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        _localizationService.GetLocalizedString("ProductCatalogService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId &&
                                              x.CatalogId == catalogId &&
                                              !x.IsDeleted &&
                                              !x.Category.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<CatalogStockItemDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var stock = await _dbContext.Stocks
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .FirstOrDefaultAsync(x => x.Id == request.StockId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (stock == null)
                {
                    return ApiResponse<CatalogStockItemDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.StockNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.StockNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var exists = await _dbContext.StockCategories
                    .AnyAsync(x => x.StockId == request.StockId &&
                                   x.CategoryId == categoryNode.CategoryId &&
                                   !x.IsDeleted)
                    .ConfigureAwait(false);

                if (exists)
                {
                    return ApiResponse<CatalogStockItemDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.StockAlreadyAssigned"),
                        _localizationService.GetLocalizedString("ProductCatalogService.StockAlreadyAssigned"),
                        StatusCodes.Status400BadRequest);
                }

                var userId = _userContextService.GetCurrentUserId();

                if (request.IsPrimary)
                {
                    var currentPrimaryAssignments = await _dbContext.StockCategories
                        .Where(x => x.StockId == request.StockId && !x.IsDeleted && x.IsPrimary)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    foreach (var assignment in currentPrimaryAssignments)
                    {
                        assignment.IsPrimary = false;
                        assignment.UpdatedDate = DateTimeProvider.Now;
                        assignment.UpdatedBy = userId;
                    }
                }

                var entity = new StockCategory
                {
                    StockId = request.StockId,
                    CategoryId = categoryNode.CategoryId,
                    AssignmentType = StockCategoryAssignmentType.Manual,
                    IsPrimary = request.IsPrimary,
                    SortOrder = request.SortOrder,
                    Note = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim(),
                    CreatedBy = userId
                };

                await _dbContext.StockCategories.AddAsync(entity).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                var dto = new CatalogStockItemDto
                {
                    Id = stock.Id,
                    StockCategoryId = entity.Id,
                    StockId = stock.Id,
                    ErpStockCode = stock.ErpStockCode,
                    StockName = stock.StockName,
                    Unit = stock.Unit,
                    GrupKodu = stock.GrupKodu,
                    GrupAdi = stock.GrupAdi,
                    Kod1 = stock.Kod1,
                    Kod1Adi = stock.Kod1Adi,
                    Kod2 = stock.Kod2,
                    Kod2Adi = stock.Kod2Adi,
                    Kod3 = stock.Kod3,
                    Kod3Adi = stock.Kod3Adi,
                    IsPrimaryCategory = entity.IsPrimary,
                    CreatedDate = stock.CreatedDate,
                    UpdatedDate = stock.UpdatedDate,
                    DeletedDate = stock.DeletedDate,
                    IsDeleted = stock.IsDeleted,
                    CreatedByFullUser = stock.CreatedByUser != null ? $"{stock.CreatedByUser.FirstName} {stock.CreatedByUser.LastName}".Trim() : null,
                    UpdatedByFullUser = stock.UpdatedByUser != null ? $"{stock.UpdatedByUser.FirstName} {stock.UpdatedByUser.LastName}".Trim() : null,
                    DeletedByFullUser = stock.DeletedByUser != null ? $"{stock.DeletedByUser.FirstName} {stock.DeletedByUser.LastName}".Trim() : null
                };

                return ApiResponse<CatalogStockItemDto>.SuccessResult(
                    dto,
                    _localizationService.GetLocalizedString("ProductCatalogService.StockAssigned"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CatalogStockItemDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.CreateStockAssignmentExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteStockCategoryAssignmentAsync(long catalogId, long catalogCategoryId, long stockCategoryId)
        {
            try
            {
                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var assignment = await _dbContext.StockCategories
                    .FirstOrDefaultAsync(x => x.Id == stockCategoryId &&
                                              x.CategoryId == categoryNode.CategoryId &&
                                              !x.IsDeleted)
                    .ConfigureAwait(false);

                if (assignment == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.StockAssignmentNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.StockAssignmentNotFound"),
                        StatusCodes.Status404NotFound);
                }

                assignment.IsDeleted = true;
                assignment.DeletedDate = DateTimeProvider.Now;
                assignment.DeletedBy = _userContextService.GetCurrentUserId();
                assignment.UpdatedDate = DateTimeProvider.Now;
                assignment.UpdatedBy = _userContextService.GetCurrentUserId();

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(
                    null,
                    _localizationService.GetLocalizedString("ProductCatalogService.StockAssignmentDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.DeleteStockAssignmentExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PagedResponse<CatalogStockItemDto>>> GetCatalogCategoryStocksAsync(long catalogId, long catalogCategoryId, PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();

                var categoryNode = await _dbContext.CatalogCategories
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == catalogCategoryId && x.CatalogId == catalogId && !x.IsDeleted && !x.Category.IsDeleted)
                    .ConfigureAwait(false);

                if (categoryNode == null)
                {
                    return ApiResponse<PagedResponse<CatalogStockItemDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        _localizationService.GetLocalizedString("ProductCatalogService.CategoryNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var query = _dbContext.StockCategories
                    .AsNoTracking()
                    .Include(x => x.Stock)
                    .Where(x => x.CategoryId == categoryNode.CategoryId &&
                                !x.IsDeleted &&
                                !x.Stock.IsDeleted)
                    .Select(x => x.Stock)
                    .ApplySearch(request.Search, StockSearchableColumns);

                var totalCount = await query.CountAsync().ConfigureAwait(false);

                var stocks = await query
                    .OrderBy(x => x.StockName)
                    .ThenBy(x => x.ErpStockCode)
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var stockIds = stocks.Select(x => x.Id).ToList();
                var stockAssignments = await _dbContext.StockCategories
                    .AsNoTracking()
                    .Where(x => x.CategoryId == categoryNode.CategoryId && stockIds.Contains(x.StockId) && !x.IsDeleted)
                    .ToDictionaryAsync(x => x.StockId, x => x)
                    .ConfigureAwait(false);

                var items = stocks.Select(stock =>
                {
                    stockAssignments.TryGetValue(stock.Id, out var assignment);
                    return new CatalogStockItemDto
                    {
                        Id = stock.Id,
                        StockCategoryId = assignment?.Id ?? 0,
                        StockId = stock.Id,
                        ErpStockCode = stock.ErpStockCode,
                        StockName = stock.StockName,
                        Unit = stock.Unit,
                        GrupKodu = stock.GrupKodu,
                        GrupAdi = stock.GrupAdi,
                        Kod1 = stock.Kod1,
                        Kod1Adi = stock.Kod1Adi,
                        Kod2 = stock.Kod2,
                        Kod2Adi = stock.Kod2Adi,
                        Kod3 = stock.Kod3,
                        Kod3Adi = stock.Kod3Adi,
                        IsPrimaryCategory = assignment?.IsPrimary ?? false,
                        CreatedDate = stock.CreatedDate,
                        UpdatedDate = stock.UpdatedDate,
                        DeletedDate = stock.DeletedDate,
                        IsDeleted = stock.IsDeleted,
                        CreatedByFullUser = stock.CreatedByUser != null ? $"{stock.CreatedByUser.FirstName} {stock.CreatedByUser.LastName}".Trim() : null,
                        UpdatedByFullUser = stock.UpdatedByUser != null ? $"{stock.UpdatedByUser.FirstName} {stock.UpdatedByUser.LastName}".Trim() : null,
                        DeletedByFullUser = stock.DeletedByUser != null ? $"{stock.DeletedByUser.FirstName} {stock.DeletedByUser.LastName}".Trim() : null
                    };
                }).ToList();

                var pagedResponse = new PagedResponse<CatalogStockItemDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<CatalogStockItemDto>>.SuccessResult(
                    pagedResponse,
                    _localizationService.GetLocalizedString("ProductCatalogService.StocksRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<CatalogStockItemDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductCatalogService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductCatalogService.GetCatalogCategoryStocksExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<string> BuildCategoryPathAsync(long? parentCategoryId, string categoryName)
        {
            if (!parentCategoryId.HasValue)
            {
                return categoryName;
            }

            var parent = await _dbContext.ProductCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == parentCategoryId.Value && !x.IsDeleted)
                .ConfigureAwait(false);

            if (parent == null)
            {
                return categoryName;
            }

            return $"{parent.FullPath ?? parent.Name}/{categoryName}";
        }

        private static void ApplyVisualPreset(ProductCategory category, CategoryVisualPresetType preset)
        {
            category.VisualPreset = preset;

            (category.IconName, category.ColorHex) = preset switch
            {
                CategoryVisualPresetType.Glass => ("panels-top-left", "#0EA5E9"),
                CategoryVisualPresetType.Profile => ("square-stack", "#7C3AED"),
                CategoryVisualPresetType.Accessory => ("package-plus", "#F59E0B"),
                CategoryVisualPresetType.Service => ("wrench", "#10B981"),
                CategoryVisualPresetType.Logistics => ("truck", "#EF4444"),
                CategoryVisualPresetType.Machinery => ("cog", "#475569"),
                CategoryVisualPresetType.Electronics => ("cpu", "#2563EB"),
                CategoryVisualPresetType.Security => ("shield", "#DC2626"),
                CategoryVisualPresetType.Lighting => ("lightbulb", "#FACC15"),
                CategoryVisualPresetType.Construction => ("hammer", "#B45309"),
                CategoryVisualPresetType.Furniture => ("armchair", "#92400E"),
                CategoryVisualPresetType.Hardware => ("nut", "#6B7280"),
                CategoryVisualPresetType.Automation => ("bot", "#4F46E5"),
                CategoryVisualPresetType.Energy => ("zap", "#65A30D"),
                CategoryVisualPresetType.Office => ("briefcase", "#0F766E"),
                CategoryVisualPresetType.Textile => ("shirt", "#DB2777"),
                CategoryVisualPresetType.Chemical => ("flask-conical", "#0891B2"),
                CategoryVisualPresetType.Outdoor => ("trees", "#15803D"),
                CategoryVisualPresetType.Cooling => ("snowflake", "#38BDF8"),
                CategoryVisualPresetType.Heating => ("flame", "#EA580C"),
                CategoryVisualPresetType.Plumbing => ("droplets", "#0284C7"),
                CategoryVisualPresetType.Decoration => ("sparkles", "#D946EF"),
                CategoryVisualPresetType.Packaging => ("package", "#A16207"),
                CategoryVisualPresetType.Automotive => ("car", "#B91C1C"),
                CategoryVisualPresetType.Agriculture => ("leaf", "#4D7C0F"),
                CategoryVisualPresetType.Medical => ("heart-pulse", "#DC2626"),
                CategoryVisualPresetType.Pharmacy => ("pill", "#2563EB"),
                CategoryVisualPresetType.Cosmetics => ("gem", "#DB2777"),
                CategoryVisualPresetType.Food => ("utensils-crossed", "#EA580C"),
                CategoryVisualPresetType.Beverage => ("cup-soda", "#0891B2"),
                CategoryVisualPresetType.Cleaning => ("spray-can", "#0EA5E9"),
                CategoryVisualPresetType.Education => ("graduation-cap", "#4338CA"),
                CategoryVisualPresetType.Finance => ("landmark", "#0F766E"),
                CategoryVisualPresetType.Insurance => ("shield-check", "#1D4ED8"),
                CategoryVisualPresetType.RealEstate => ("building-2", "#A16207"),
                CategoryVisualPresetType.Tourism => ("plane", "#0284C7"),
                CategoryVisualPresetType.Hospitality => ("hotel", "#7C3AED"),
                CategoryVisualPresetType.Restaurant => ("chef-hat", "#C2410C"),
                CategoryVisualPresetType.ECommerce => ("shopping-cart", "#7C2D12"),
                CategoryVisualPresetType.Retail => ("store", "#BE123C"),
                CategoryVisualPresetType.Wholesale => ("warehouse", "#475569"),
                CategoryVisualPresetType.Manufacturing => ("factory", "#334155"),
                CategoryVisualPresetType.Mining => ("pickaxe", "#92400E"),
                CategoryVisualPresetType.Marine => ("ship", "#0369A1"),
                CategoryVisualPresetType.Aviation => ("plane-takeoff", "#2563EB"),
                CategoryVisualPresetType.Telecom => ("tower-control", "#6D28D9"),
                CategoryVisualPresetType.Software => ("code-2", "#4F46E5"),
                CategoryVisualPresetType.DataCenter => ("server", "#1E40AF"),
                CategoryVisualPresetType.Media => ("clapperboard", "#E11D48"),
                CategoryVisualPresetType.Printing => ("printer", "#6B7280"),
                CategoryVisualPresetType.Sports => ("dumbbell", "#15803D"),
                CategoryVisualPresetType.BabyKids => ("baby", "#EC4899"),
                CategoryVisualPresetType.Pet => ("paw-print", "#A16207"),
                CategoryVisualPresetType.Jewelry => ("diamond", "#A21CAF"),
                CategoryVisualPresetType.Luxury => ("crown", "#CA8A04"),
                CategoryVisualPresetType.Stationery => ("pen-tool", "#0F766E"),
                CategoryVisualPresetType.Laboratory => ("microscope", "#0891B2"),
                CategoryVisualPresetType.Recycling => ("recycle", "#16A34A"),
                CategoryVisualPresetType.Defense => ("shield-half", "#1F2937"),
                _ => ("shapes", "#E11D48")
            };
        }

        private static bool IsRuleMatched(StockEntity stock, ProductCategoryRule rule)
        {
            var sourceValue = GetStockAttributeValue(stock, rule.StockAttributeType);
            if (string.IsNullOrWhiteSpace(sourceValue))
            {
                return false;
            }

            var stockValue = sourceValue.Trim();
            var ruleValue = rule.Value?.Trim();
            if (string.IsNullOrWhiteSpace(ruleValue))
            {
                return false;
            }

            return rule.OperatorType switch
            {
                CategoryRuleOperatorType.Equals => string.Equals(stockValue, ruleValue, StringComparison.OrdinalIgnoreCase),
                CategoryRuleOperatorType.Contains => stockValue.Contains(ruleValue, StringComparison.OrdinalIgnoreCase),
                CategoryRuleOperatorType.StartsWith => stockValue.StartsWith(ruleValue, StringComparison.OrdinalIgnoreCase),
                CategoryRuleOperatorType.EndsWith => stockValue.EndsWith(ruleValue, StringComparison.OrdinalIgnoreCase),
                CategoryRuleOperatorType.InList => ruleValue
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Any(item => string.Equals(item, stockValue, StringComparison.OrdinalIgnoreCase)),
                _ => false
            };
        }

        private IQueryable<string?> GetStockAttributeQuery(StockAttributeType attributeType)
        {
            var stocks = _dbContext.Stocks
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            return attributeType switch
            {
                StockAttributeType.GroupCode => stocks.Select(x => x.GrupKodu),
                StockAttributeType.GroupName => stocks.Select(x => x.GrupAdi),
                StockAttributeType.Code1 => stocks.Select(x => x.Kod1),
                StockAttributeType.Code1Name => stocks.Select(x => x.Kod1Adi),
                StockAttributeType.Code2 => stocks.Select(x => x.Kod2),
                StockAttributeType.Code2Name => stocks.Select(x => x.Kod2Adi),
                StockAttributeType.Code3 => stocks.Select(x => x.Kod3),
                StockAttributeType.Code3Name => stocks.Select(x => x.Kod3Adi),
                StockAttributeType.Code4 => stocks.Select(x => x.Kod4),
                StockAttributeType.Code4Name => stocks.Select(x => x.Kod4Adi),
                StockAttributeType.Code5 => stocks.Select(x => x.Kod5),
                StockAttributeType.Code5Name => stocks.Select(x => x.Kod5Adi),
                StockAttributeType.ManufacturerCode => stocks.Select(x => x.UreticiKodu),
                StockAttributeType.ErpStockCode => stocks.Select(x => x.ErpStockCode),
                StockAttributeType.StockName => stocks.Select(x => x.StockName),
                _ => stocks.Select(x => x.StockName)
            };
        }

        private static string? GetStockAttributeValue(StockEntity stock, StockAttributeType attributeType)
        {
            return attributeType switch
            {
                StockAttributeType.GroupCode => stock.GrupKodu,
                StockAttributeType.GroupName => stock.GrupAdi,
                StockAttributeType.Code1 => stock.Kod1,
                StockAttributeType.Code1Name => stock.Kod1Adi,
                StockAttributeType.Code2 => stock.Kod2,
                StockAttributeType.Code2Name => stock.Kod2Adi,
                StockAttributeType.Code3 => stock.Kod3,
                StockAttributeType.Code3Name => stock.Kod3Adi,
                StockAttributeType.Code4 => stock.Kod4,
                StockAttributeType.Code4Name => stock.Kod4Adi,
                StockAttributeType.Code5 => stock.Kod5,
                StockAttributeType.Code5Name => stock.Kod5Adi,
                StockAttributeType.ManufacturerCode => stock.UreticiKodu,
                StockAttributeType.ErpStockCode => stock.ErpStockCode,
                StockAttributeType.StockName => stock.StockName,
                _ => null
            };
        }
    }
}
