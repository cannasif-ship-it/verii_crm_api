using AutoMapper;
using cms_webapi.Models;
using cms_webapi.DTOs;

namespace cms_webapi.Mappings
{
    public class ProductPricingGroupByMappingProfile : Profile
    {
        public ProductPricingGroupByMappingProfile()
        {
            // ProductPricingGroupBy mappings
            CreateMap<ProductPricingGroupBy, ProductPricingGroupByDto>();

            CreateMap<CreateProductPricingGroupByDto, ProductPricingGroupBy>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore());

            CreateMap<UpdateProductPricingGroupByDto, ProductPricingGroupBy>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore());
        }
    }
}