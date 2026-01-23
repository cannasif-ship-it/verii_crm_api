using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;

namespace crm_api.Mappings
{
    public class ShippingAddressMappingProfile : Profile
    {
        public ShippingAddressMappingProfile()
        {
            // ShippingAddress mappings
            CreateMap<ShippingAddress, ShippingAddressGetDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : string.Empty))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Countries != null ? src.Countries.Name : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Cities != null ? src.Cities.Name : null))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.Districts != null ? src.Districts.Name : null))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<CreateShippingAddressDto, ShippingAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Countries, opt => opt.Ignore())
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Districts, opt => opt.Ignore());

            CreateMap<UpdateShippingAddressDto, ShippingAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Countries, opt => opt.Ignore())
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Districts, opt => opt.Ignore());
        }
    }
}
