using AutoMapper;
using crm_api.Modules.ReportBuilder.Application.Dtos;

namespace crm_api.Modules.ReportBuilder.Application.Mappings
{
    public class ReportDefinitionMappingProfile : Profile
    {
        public ReportDefinitionMappingProfile()
        {
            CreateMap<ReportDefinition, ReportListItemDto>();
            CreateMap<ReportDefinition, ReportDetailDto>();
            CreateMap<ReportCreateDto, ReportDefinition>();
            CreateMap<ReportUpdateDto, ReportDefinition>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.CreatedDate, o => o.Ignore());
        }
    }
}
