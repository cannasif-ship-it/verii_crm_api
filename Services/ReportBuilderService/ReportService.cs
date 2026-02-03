using System.Text.Json;
using AutoMapper;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.DTOs.ReportBuilderDto;
using crm_api.Interfaces;
using crm_api.Models.ReportBuilder;
using crm_api.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services.ReportBuilderService
{
    public class ReportService : IReportService
    {
        private readonly CmsDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReportingConnectionService _connectionService;
        private readonly IReportingCatalogService _catalogService;
        private readonly ILogger<ReportService> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public ReportService(
            CmsDbContext context,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IReportingConnectionService connectionService,
            IReportingCatalogService catalogService,
            ILogger<ReportService> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _connectionService = connectionService;
            _catalogService = catalogService;
            _logger = logger;
        }

        public async Task<ApiResponse<ReportDetailDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _context.ReportDefinitions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
                if (entity == null)
                    return ApiResponse<ReportDetailDto>.ErrorResult("Report not found", null, 404);
                var dto = _mapper.Map<ReportDetailDto>(entity);
                return ApiResponse<ReportDetailDto>.SuccessResult(dto, "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById report {Id}", id);
                return ApiResponse<ReportDetailDto>.ErrorResult("Error retrieving report", ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<ReportListItemDto>>> ListAsync(string? search, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var query = _context.ReportDefinitions.AsNoTracking().Where(r => !r.IsDeleted);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var term = search.Trim();
                    query = query.Where(r => r.Name.Contains(term) || (r.Description != null && r.Description.Contains(term)));
                }
                var total = await query.CountAsync();
                var list = await query
                    .OrderByDescending(r => r.UpdatedDate ?? r.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var items = _mapper.Map<List<ReportListItemDto>>(list);
                var paged = new PagedResponse<ReportListItemDto> { Items = items, TotalCount = total, PageNumber = pageNumber, PageSize = pageSize };
                return ApiResponse<PagedResponse<ReportListItemDto>>.SuccessResult(paged, "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "List reports");
                return ApiResponse<PagedResponse<ReportListItemDto>>.ErrorResult("Error listing reports", ex.Message, 500);
            }
        }

        public async Task<ApiResponse<ReportDetailDto>> CreateAsync(ReportCreateDto dto, long userId)
        {
            var validation = await ValidateForSaveAsync(dto.ConnectionKey, dto.DataSourceType, dto.DataSourceName, dto.ConfigJson);
            if (!validation.Success)
                return ApiResponse<ReportDetailDto>.ErrorResult(validation.Message, validation.ExceptionMessage, validation.StatusCode);

            try
            {
                var entity = _mapper.Map<ReportDefinition>(dto);
                entity.CreatedBy = userId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                var repo = _unitOfWork.Repository<ReportDefinition>();
                await repo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var created = await _context.ReportDefinitions.AsNoTracking().FirstOrDefaultAsync(r => r.Id == entity.Id);
                var detail = _mapper.Map<ReportDetailDto>(created!);
                return ApiResponse<ReportDetailDto>.SuccessResult(detail, "Created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create report");
                return ApiResponse<ReportDetailDto>.ErrorResult("Error creating report", ex.Message, 500);
            }
        }

        public async Task<ApiResponse<ReportDetailDto>> UpdateAsync(long id, ReportUpdateDto dto, long userId)
        {
            var validation = await ValidateForSaveAsync(dto.ConnectionKey, dto.DataSourceType, dto.DataSourceName, dto.ConfigJson);
            if (!validation.Success)
                return ApiResponse<ReportDetailDto>.ErrorResult(validation.Message, validation.ExceptionMessage, validation.StatusCode);

            var repo = _unitOfWork.Repository<ReportDefinition>();
            var entity = await repo.GetByIdForUpdateAsync(id);
            if (entity == null)
                return ApiResponse<ReportDetailDto>.ErrorResult("Report not found", null, 404);

            try
            {
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.ConnectionKey = dto.ConnectionKey;
                entity.DataSourceType = dto.DataSourceType;
                entity.DataSourceName = dto.DataSourceName;
                entity.ConfigJson = dto.ConfigJson;
                entity.UpdatedBy = userId;
                entity.UpdatedDate = DateTime.UtcNow;
                await repo.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var updated = await _context.ReportDefinitions.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                var detail = _mapper.Map<ReportDetailDto>(updated!);
                return ApiResponse<ReportDetailDto>.SuccessResult(detail, "Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update report {Id}", id);
                return ApiResponse<ReportDetailDto>.ErrorResult("Error updating report", ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            var repo = _unitOfWork.Repository<ReportDefinition>();
            var ok = await repo.SoftDeleteAsync(id);
            if (!ok)
                return ApiResponse<bool>.ErrorResult("Report not found", null, 404);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResult(true, "Deleted");
        }

        private async Task<ApiResponse<object>> ValidateForSaveAsync(string connectionKey, string dataSourceType, string dataSourceName, string configJson)
        {
            var connResp = _connectionService.ResolveConnectionString(connectionKey);
            if (!connResp.Success)
                return ApiResponse<object>.ErrorResult(connResp.Message ?? "Invalid connection.", null, connResp.StatusCode);

            var catalogResp = await _catalogService.CheckAndGetSchemaAsync(connectionKey, dataSourceType, dataSourceName);
            if (!catalogResp.Success)
                return ApiResponse<object>.ErrorResult(catalogResp.Message ?? "Catalog error.", catalogResp.ExceptionMessage, catalogResp.StatusCode);
            if (catalogResp.Data == null || catalogResp.Data.Count == 0)
                return ApiResponse<object>.ErrorResult("Datasource does not exist or has no columns.", null, 400);

            try
            {
                var config = JsonSerializer.Deserialize<ReportConfig>(configJson, JsonOptions);
                if (config == null)
                    return ApiResponse<object>.ErrorResult("Invalid ConfigJson.", null, 400);
                var err = ValidateReportConfig(config);
                if (!string.IsNullOrEmpty(err))
                    return ApiResponse<object>.ErrorResult(err, null, 400);
            }
            catch (JsonException ex)
            {
                return ApiResponse<object>.ErrorResult("Invalid ConfigJson format.", ex.Message, 400);
            }

            return ApiResponse<object>.SuccessResult(null!, "OK");
        }

        private static readonly HashSet<string> ChartTypes = new(StringComparer.OrdinalIgnoreCase) { "table", "bar", "line", "pie" };
        private static readonly HashSet<string> Aggregations = new(StringComparer.OrdinalIgnoreCase) { "none", "sum", "count", "avg", "min", "max" };

        private static string? ValidateReportConfig(ReportConfig config)
        {
            if (!ChartTypes.Contains(config.ChartType ?? ""))
                return "chartType must be one of: table, bar, line, pie.";
            if (config.Values == null || config.Values.Count == 0)
                return "At least one value field is required.";
            foreach (var v in config.Values)
            {
                if (string.IsNullOrWhiteSpace(v.Field))
                    return "Each value must have a field.";
                if (!Aggregations.Contains(v.Aggregation ?? "none"))
                    return "Invalid aggregation; use: none, sum, count, avg, min, max.";
            }
            return null;
        }
    }
}
