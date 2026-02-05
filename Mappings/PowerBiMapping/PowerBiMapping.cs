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

            // -------------------------
            // GroupReportDefinition Mappings
            // -------------------------

            CreateMap<PowerBIGroupReportDefinition, PowerBIGroupReportDefinitionGetDto>()
                .ForMember(d => d.GroupName, opt => opt.MapFrom(s => s.Group != null ? s.Group.Name : null))
                .ForMember(d => d.ReportDefinitionName, opt => opt.MapFrom(s => s.ReportDefinition != null ? s.ReportDefinition.Name : null))
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s =>
                    s.CreatedByUser != null
                        ? (s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName)
                        : null))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s =>
                    s.UpdatedByUser != null
                        ? (s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName)
                        : null));

            CreateMap<CreatePowerBIGroupReportDefinitionDto, PowerBIGroupReportDefinition>();
            CreateMap<UpdatePowerBIGroupReportDefinitionDto, PowerBIGroupReportDefinition>();

            // -------------------------
            // UserPowerBIGroup Mappings
            // -------------------------

            CreateMap<UserPowerBIGroup, UserPowerBIGroupGetDto>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User != null ? (s.User.FirstName + " " + s.User.LastName) : null))
                .ForMember(d => d.GroupName, opt => opt.MapFrom(s => s.Group != null ? s.Group.Name : null))
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s =>
                    s.CreatedByUser != null
                        ? (s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName)
                        : null))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s =>
                    s.UpdatedByUser != null
                        ? (s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName)
                        : null));

            CreateMap<CreateUserPowerBIGroupDto, UserPowerBIGroup>();
            CreateMap<UpdateUserPowerBIGroupDto, UserPowerBIGroup>();
        }
    }
}
