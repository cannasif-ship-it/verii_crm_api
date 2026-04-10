using AutoMapper;
using System.Text.Json;

namespace crm_api.Modules.PdfBuilder.Application.Mappings
{
    public class ReportTemplateMappingProfile : Profile
    {
        public ReportTemplateMappingProfile()
        {
            CreateMap<ReportTemplate, ReportTemplateDto>()
                .ForMember(dest => dest.TemplateData,
                    opt => opt.MapFrom(src => JsonSerializer.Deserialize<ReportTemplateData>(src.TemplateJson, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    })));

            CreateMap<CreateReportTemplateDto, ReportTemplate>()
                .ForMember(dest => dest.TemplateJson,
                    opt => opt.MapFrom(src => JsonSerializer.Serialize(src.TemplateData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false
                    })));

            CreateMap<UpdateReportTemplateDto, ReportTemplate>()
                .ForMember(dest => dest.TemplateJson,
                    opt => opt.MapFrom(src => JsonSerializer.Serialize(src.TemplateData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false
                    })));
        }
    }
}
