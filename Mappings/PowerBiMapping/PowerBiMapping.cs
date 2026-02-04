using AutoMapper;
using crm_api.DTOs.PowerBi;
using crm_api.Models.PowerBi;

namespace crm_api.Mappings.PowerBiMapping
{
    public class PowerBiMapping : Profile
    {
        public PowerBiMapping()
        {
            // -------------------------
            // Report Definition Mappings
            // -------------------------

            CreateMap<PowerBIReportDefinition, PowerBIReportDefinitionDto>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s =>
                    s.CreatedByUser != null
                        ? (s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName)
                        : null))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s =>
                    s.UpdatedByUser != null
                        ? (s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName)
                        : null));

            CreateMap<PowerBIReportDefinition, PowerBIReportDefinitionGetDto>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s =>
                    s.CreatedByUser != null
                        ? (s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName)
                        : null))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s =>
                    s.UpdatedByUser != null
                        ? (s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName)
                        : null));

            CreateMap<CreatePowerBIReportDefinitionDto, PowerBIReportDefinition>();
            CreateMap<UpdatePowerBIReportDefinitionDto, PowerBIReportDefinition>();

            // -------------------------
            // Group Mappings
            // -------------------------

            CreateMap<PowerBIGroup, PowerBIGroupDto>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s =>
                    s.CreatedByUser != null
                        ? (s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName)
                        : null))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s =>
                    s.UpdatedByUser != null
                        ? (s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName)
                        : null));

            CreateMap<PowerBIGroup, PowerBIGroupGetDto>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s =>
                    s.CreatedByUser != null
                        ? (s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName)
                        : null))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s =>
                    s.UpdatedByUser != null
                        ? (s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName)
                        : null));

            CreateMap<CreatePowerBIGroupDto, PowerBIGroup>();
            
            CreateMap<UpdatePowerBIGroupDto, PowerBIGroup>();
        }
    }
}
