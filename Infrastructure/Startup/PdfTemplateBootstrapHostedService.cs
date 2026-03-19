using System.Text.Json;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Models;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Infrastructure.Startup
{
    public class PdfTemplateBootstrapHostedService : IHostedService
    {
        private static readonly BootstrapTemplateDefinition[] TemplateDefinitions =
        {
            new(
                DocumentRuleType.Quotation,
                "Teklif Varsayilan",
                MakeDefault: true),
            new(
                DocumentRuleType.Demand,
                "Talep Varsayilan",
                MakeDefault: true),
            new(
                DocumentRuleType.Order,
                "Siparis Varsayilan",
                MakeDefault: true),
        };

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PdfTemplateBootstrapHostedService> _logger;

        public PdfTemplateBootstrapHostedService(
            IServiceProvider serviceProvider,
            ILogger<PdfTemplateBootstrapHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<CmsDbContext>();

            var templates = await db.Set<ReportTemplate>()
                .Where(template => !template.IsDeleted)
                .ToListAsync(cancellationToken);

            var changed = false;
            foreach (var definition in TemplateDefinitions)
            {
                changed |= EnsureTemplate(definition, templates, db);
            }

            if (changed)
            {
                await db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Default PDF templates aligned for quotation, demand and order layouts.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static bool EnsureTemplate(
            BootstrapTemplateDefinition definition,
            List<ReportTemplate> templates,
            CmsDbContext db)
        {
            var scopedTemplates = templates
                .Where(template => template.RuleType == definition.RuleType)
                .ToList();

            var selectedTemplate = scopedTemplates
                .FirstOrDefault(template => template.Default)
                ?? scopedTemplates.FirstOrDefault();

            if (selectedTemplate == null)
            {
                selectedTemplate = new ReportTemplate
                {
                    RuleType = definition.RuleType,
                    Title = definition.Title,
                    TemplateJson = JsonSerializer.Serialize(BuildDefaultTemplate(), PdfReportTemplateJsonOptions.CamelCase),
                    IsActive = true,
                    Default = false,
                    CreatedDate = DateTimeProvider.Now,
                };

                db.Set<ReportTemplate>().Add(selectedTemplate);
                templates.Add(selectedTemplate);
                scopedTemplates.Add(selectedTemplate);
            }

            var changed = selectedTemplate.Id == 0;
            if (!selectedTemplate.IsActive)
            {
                selectedTemplate.IsActive = true;
                changed = true;
            }

            if (definition.MakeDefault && !selectedTemplate.Default)
            {
                selectedTemplate.Default = true;
                changed = true;
            }

            if (definition.MakeDefault)
            {
                foreach (var template in scopedTemplates.Where(template => template != selectedTemplate && template.Default))
                {
                    template.Default = false;
                    changed = true;
                }
            }

            return changed;
        }

        private static ReportTemplateData BuildDefaultTemplate()
        {
            return new ReportTemplateData
            {
                SchemaVersion = 1,
                Page = new PageConfig
                {
                    Width = 210,
                    Height = 297,
                    Unit = "mm",
                    PageCount = 1,
                },
                Elements = new List<ReportElement>(),
            };
        }

        private sealed record BootstrapTemplateDefinition(
            DocumentRuleType RuleType,
            string Title,
            bool MakeDefault);
    }
}
