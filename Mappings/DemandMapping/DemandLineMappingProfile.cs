using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;

namespace crm_api.Mappings
{
    public class DemandLineMappingProfile : Profile
    {
        public DemandLineMappingProfile()
        {
            // Entity -> DTO
            CreateMap<DemandLine, DemandLineDto>();
            
            CreateMap<DemandLine, DemandLineGetDto>();

            // Create DTO -> Entity
            CreateMap<CreateDemandLineDto, DemandLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Demand, opt => opt.Ignore())
                .ForMember(dest => dest.PricingRuleHeader, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedStock, opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<UpdateDemandLineDto, DemandLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Demand, opt => opt.Ignore())
                .ForMember(dest => dest.PricingRuleHeader, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedStock, opt => opt.Ignore());
                
        }
    }
}
