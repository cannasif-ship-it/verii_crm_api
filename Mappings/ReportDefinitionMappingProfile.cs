using AutoMapper;
using crm_api.DTOs.ReportBuilderDto;
using crm_api.Models.ReportBuilder;

namespace crm_api.Mappings
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
