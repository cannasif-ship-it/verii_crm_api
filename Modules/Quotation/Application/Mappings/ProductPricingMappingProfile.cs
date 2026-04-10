using AutoMapper;

namespace crm_api.Modules.Quotation.Application.Mappings
{
    public class ProductPricingMappingProfile : Profile
    {
        public ProductPricingMappingProfile()
        {
            // ProductPricing mappings
            CreateMap<ProductPricing, ProductPricingGetDto>();

            CreateMap<ProductPricingCreateDto, ProductPricing>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore());

            CreateMap<ProductPricingUpdateDto, ProductPricing>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore());
        }
    }
}
