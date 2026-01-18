using AutoMapper;
using cms_webapi.Models;
using cms_webapi.DTOs;

namespace cms_webapi.Mappings
{
    public class ApprovalFlowMappingProfile : Profile
    {
        public ApprovalFlowMappingProfile()
        {
            // ApprovalFlow mappings
            CreateMap<ApprovalFlow, ApprovalFlowGetDto>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => (int)src.DocumentType))
                .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType.ToString()))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<ApprovalFlowCreateDto, ApprovalFlow>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => (PricingRuleType)src.DocumentType));

            CreateMap<ApprovalFlowUpdateDto, ApprovalFlow>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => (PricingRuleType)src.DocumentType))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
