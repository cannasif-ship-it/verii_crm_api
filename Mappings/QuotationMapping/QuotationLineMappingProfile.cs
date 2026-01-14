using AutoMapper;
using cms_webapi.Models;
using cms_webapi.DTOs;

namespace cms_webapi.Mappings
{
    public class QuotationLineMappingProfile : Profile
    {
        public QuotationLineMappingProfile()
        {
            // Entity -> DTO
            CreateMap<QuotationLine, QuotationLineDto>();
            CreateMap<QuotationLine, QuotationLineGetDto>();

            // Create DTO -> Entity
            CreateMap<CreateQuotationLineDto, QuotationLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Quotation, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCode, opt => opt.Ignore())
                .ForMember(dest => dest.PricingRuleHeader, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedStock, opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<UpdateQuotationLineDto, QuotationLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Quotation, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCode, opt => opt.Ignore())
                .ForMember(dest => dest.PricingRuleHeader, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedStock, opt => opt.Ignore());
                
        }
    }
}