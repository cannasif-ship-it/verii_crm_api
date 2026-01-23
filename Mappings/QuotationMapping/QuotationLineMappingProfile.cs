using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;

namespace crm_api.Mappings
{
    public class QuotationLineMappingProfile : Profile
    {
        public QuotationLineMappingProfile()
        {
            // Entity -> DTO
            CreateMap<QuotationLine, QuotationLineDto>();
            
            CreateMap<QuotationLine, QuotationLineGetDto>();

            // Create DTO -> Entity
            // ProductCode, ApprovalStatus, PricingRuleHeaderId, RelatedStockId, RelatedProductKey, 
            // IsMainRelatedProduct ve diğer tüm eşleşen property'ler otomatik map edilecek
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
                .ForMember(dest => dest.PricingRuleHeader, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedStock, opt => opt.Ignore());

            // Update DTO -> Entity
            // ProductCode, ApprovalStatus, PricingRuleHeaderId, RelatedStockId, RelatedProductKey, 
            // IsMainRelatedProduct ve diğer tüm eşleşen property'ler otomatik map edilecek
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
                .ForMember(dest => dest.PricingRuleHeader, opt => opt.Ignore())
                .ForMember(dest => dest.RelatedStock, opt => opt.Ignore());
                
        }
    }
}
