using AutoMapper;
using crm_api.Modules.System.Application.Dtos;

namespace crm_api.Modules.System.Application.Mappings
{
    public class SystemSettingsMappingProfile : Profile
    {
        public SystemSettingsMappingProfile()
        {
            CreateMap<Domain.Entities.SystemSetting, SystemSettingsDto>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedDate ?? src.CreatedDate));

            CreateMap<UpdateSystemSettingsDto, Domain.Entities.SystemSetting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUser, opt => opt.Ignore());
        }
    }
}
