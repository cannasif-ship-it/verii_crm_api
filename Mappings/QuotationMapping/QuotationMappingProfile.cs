using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;

namespace crm_api.Mappings
{
    public class QuotationMappingProfile : Profile
    {
        public QuotationMappingProfile()
        {
            // Quotation mappings
            CreateMap<Quotation, QuotationDto>()
                .ForMember(dest => dest.PotentialCustomerName, opt => opt.MapFrom(src => src.PotentialCustomer != null ? src.PotentialCustomer.CustomerName : null))
                .ForMember(dest => dest.ShippingAddressText, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.Address : null))
                .ForMember(dest => dest.RepresentativeName, opt => opt.MapFrom(src => src.Representative != null ? src.Representative.FirstName + " " + src.Representative.LastName : null))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType != null ? src.PaymentType.Name : null))
                .ForMember(dest => dest.DocumentSerialTypeName, opt => opt.MapFrom(src => src.DocumentSerialType != null ? src.DocumentSerialType.SerialPrefix : null));

            CreateMap<Quotation, QuotationGetDto>()
                .ForMember(dest => dest.PotentialCustomerName, opt => opt.MapFrom(src => src.PotentialCustomer != null ? src.PotentialCustomer.CustomerName : null))
                .ForMember(dest => dest.ShippingAddressText, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.Address : null))
                .ForMember(dest => dest.RepresentativeName, opt => opt.MapFrom(src => src.Representative != null ? src.Representative.FirstName + " " + src.Representative.LastName : null))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType != null ? src.PaymentType.Name : null))
                .ForMember(dest => dest.DocumentSerialTypeName, opt => opt.MapFrom(src => src.DocumentSerialType != null ? src.DocumentSerialType.SerialPrefix : null));

            CreateMap<CreateQuotationDto, Quotation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.PotentialCustomer, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingAddress, opt => opt.Ignore())
                .ForMember(dest => dest.Representative, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentType, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentSerialType, opt => opt.Ignore())
                .ForMember(dest => dest.Demand, opt => opt.Ignore());

            CreateMap<UpdateQuotationDto, Quotation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.PotentialCustomer, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingAddress, opt => opt.Ignore())
                .ForMember(dest => dest.Representative, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentType, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentSerialType, opt => opt.Ignore())
                .ForMember(dest => dest.Demand, opt => opt.Ignore());
        }
    }
}
