using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;

namespace crm_api.Services
{
    public class ReportTemplateService : IReportTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportTemplateService> _logger;
        private readonly CmsDbContext _context;
        private readonly IReportPdfGeneratorService _pdfGenerator;

        public ReportTemplateService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ReportTemplateService> logger,
            CmsDbContext context,
            IReportPdfGeneratorService pdfGenerator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _pdfGenerator = pdfGenerator;
        }

        public async Task<ApiResponse<PagedResponse<ReportTemplateDto>>> GetAllAsync(
            PagedRequest request,
            DocumentRuleType? ruleType = null,
            bool? isActive = null)
        {
            try
            {
                var query = _context.ReportTemplates
                    .Where(rt => !rt.IsDeleted)
                    .AsQueryable();

                // Apply filters
                if (ruleType.HasValue)
                {
                    query = query.Where(rt => rt.RuleType == ruleType.Value);
                }

                if (isActive.HasValue)
                {
                    query = query.Where(rt => rt.IsActive == isActive.Value);
                }

                // Get total count
                var totalCount = await query.CountAsync();

                // Apply pagination
                var templates = await query
                    .OrderByDescending(rt => rt.CreatedDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Map to DTOs
                var templateDtos = templates.Select(template => new ReportTemplateDto
                {
                    Id = template.Id,
                    RuleType = template.RuleType,
                    Title = template.Title,
                    TemplateData = JsonSerializer.Deserialize<ReportTemplateData>(template.TemplateJson),
                    IsActive = template.IsActive,
                    CreatedByUserId = template.CreatedByUserId,
                    UpdatedByUserId = template.UpdatedByUserId,
                    CreatedDate = template.CreatedDate,
                    UpdatedDate = template.UpdatedDate
                }).ToList();

                var pagedResponse = new PagedResponse<ReportTemplateDto>
                {
                    Items = templateDtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ReportTemplateDto>>.SuccessResult(
                    pagedResponse,
                    "Report templates retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving report templates");
                return ApiResponse<PagedResponse<ReportTemplateDto>>.ErrorResult(
                    "Error retrieving report templates",
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<ReportTemplateDto>> GetByIdAsync(long id)
        {
            try
            {
                var template = await _context.ReportTemplates
                    .Where(rt => rt.Id == id && !rt.IsDeleted)
                    .FirstOrDefaultAsync();

                if (template == null)
                {
                    return ApiResponse<ReportTemplateDto>.ErrorResult(
                        "Report template not found",
                        null,
                        404);
                }

                var templateDto = new ReportTemplateDto
                {
                    Id = template.Id,
                    RuleType = template.RuleType,
                    Title = template.Title,
                    TemplateData = JsonSerializer.Deserialize<ReportTemplateData>(template.TemplateJson),
                    IsActive = template.IsActive,
                    CreatedByUserId = template.CreatedByUserId,
                    UpdatedByUserId = template.UpdatedByUserId,
                    CreatedDate = template.CreatedDate,
                    UpdatedDate = template.UpdatedDate
                };

                return ApiResponse<ReportTemplateDto>.SuccessResult(
                    templateDto,
                    "Report template retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving report template with ID {Id}", id);
                return ApiResponse<ReportTemplateDto>.ErrorResult(
                    "Error retrieving report template",
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<ReportTemplateDto>> CreateAsync(CreateReportTemplateDto dto, long userId)
        {
            try
            {
                // Serialize template data to JSON
                var templateJson = JsonSerializer.Serialize(dto.TemplateData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                });

                var template = new ReportTemplate
                {
                    RuleType = dto.RuleType,
                    Title = dto.Title,
                    TemplateJson = templateJson,
                    IsActive = dto.IsActive,
                    CreatedByUserId = userId,
                    CreatedDate = DateTime.UtcNow
                };

                await _context.ReportTemplates.AddAsync(template);
                await _context.SaveChangesAsync();

                var templateDto = new ReportTemplateDto
                {
                    Id = template.Id,
                    RuleType = template.RuleType,
                    Title = template.Title,
                    TemplateData = dto.TemplateData,
                    IsActive = template.IsActive,
                    CreatedByUserId = template.CreatedByUserId,
                    CreatedDate = template.CreatedDate
                };

                return ApiResponse<ReportTemplateDto>.SuccessResult(
                    templateDto,
                    "Report template created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating report template");
                return ApiResponse<ReportTemplateDto>.ErrorResult(
                    "Error creating report template",
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<ReportTemplateDto>> UpdateAsync(long id, UpdateReportTemplateDto dto, long userId)
        {
            try
            {
                var template = await _context.ReportTemplates
                    .Where(rt => rt.Id == id && !rt.IsDeleted)
                    .FirstOrDefaultAsync();

                if (template == null)
                {
                    return ApiResponse<ReportTemplateDto>.ErrorResult(
                        "Report template not found",
                        null,
                        404);
                }

                // Serialize template data to JSON
                var templateJson = JsonSerializer.Serialize(dto.TemplateData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                });

                template.RuleType = dto.RuleType;
                template.Title = dto.Title;
                template.TemplateJson = templateJson;
                template.IsActive = dto.IsActive;
                template.UpdatedByUserId = userId;
                template.UpdatedDate = DateTime.UtcNow;

                _context.ReportTemplates.Update(template);
                await _context.SaveChangesAsync();

                var templateDto = new ReportTemplateDto
                {
                    Id = template.Id,
                    RuleType = template.RuleType,
                    Title = template.Title,
                    TemplateData = dto.TemplateData,
                    IsActive = template.IsActive,
                    CreatedByUserId = template.CreatedByUserId,
                    UpdatedByUserId = template.UpdatedByUserId,
                    CreatedDate = template.CreatedDate,
                    UpdatedDate = template.UpdatedDate
                };

                return ApiResponse<ReportTemplateDto>.SuccessResult(
                    templateDto,
                    "Report template updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating report template with ID {Id}", id);
                return ApiResponse<ReportTemplateDto>.ErrorResult(
                    "Error updating report template",
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var template = await _context.ReportTemplates
                    .Where(rt => rt.Id == id && !rt.IsDeleted)
                    .FirstOrDefaultAsync();

                if (template == null)
                {
                    return ApiResponse<bool>.ErrorResult(
                        "Report template not found",
                        null,
                        404);
                }

                template.IsDeleted = true;
                template.DeletedDate = DateTime.UtcNow;

                _context.ReportTemplates.Update(template);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(
                    true,
                    "Report template deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting report template with ID {Id}", id);
                return ApiResponse<bool>.ErrorResult(
                    "Error deleting report template",
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<byte[]>> GeneratePdfAsync(long templateId, long entityId)
        {
            try
            {
                var template = await _context.ReportTemplates
                    .Where(rt => rt.Id == templateId && !rt.IsDeleted && rt.IsActive)
                    .FirstOrDefaultAsync();

                if (template == null)
                {
                    return ApiResponse<byte[]>.ErrorResult(
                        "Report template not found or inactive",
                        null,
                        404);
                }

                // Deserialize template data
                var templateData = JsonSerializer.Deserialize<ReportTemplateData>(template.TemplateJson);
                if (templateData == null)
                {
                    return ApiResponse<byte[]>.ErrorResult(
                        "Invalid template data",
                        null,
                        400);
                }

                // Generate PDF using the PDF generator service
                var pdfBytes = await _pdfGenerator.GeneratePdfAsync(template.RuleType, entityId, templateData);

                return ApiResponse<byte[]>.SuccessResult(
                    pdfBytes,
                    "PDF generated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF for template {TemplateId} and entity {EntityId}", templateId, entityId);
                return ApiResponse<byte[]>.ErrorResult(
                    "Error generating PDF",
                    ex.Message,
                    500);
            }
        }
    }
}
