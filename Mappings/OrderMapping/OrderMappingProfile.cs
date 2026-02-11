using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;

namespace crm_api.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.PotentialCustomerName, opt => opt.MapFrom(src => src.PotentialCustomer != null ? src.PotentialCustomer.CustomerName : null))
                .ForMember(dest => dest.ShippingAddressText, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.Address : null))
                .ForMember(dest => dest.RepresentativeName, opt => opt.MapFrom(src => src.Representative != null ? src.Representative.FirstName + " " + src.Representative.LastName : null))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType != null ? src.PaymentType.Name : null))
                .ForMember(dest => dest.GeneralDiscountRate, opt => opt.MapFrom(src => src.GeneralDiscountRate))
                .ForMember(dest => dest.GeneralDiscountAmount, opt => opt.MapFrom(src => src.GeneralDiscountAmount))
                .ForMember(dest => dest.DocumentSerialTypeName, opt => opt.MapFrom(src => src.DocumentSerialType != null ? src.DocumentSerialType.SerialPrefix : null))
                .ForMember(dest => dest.SalesTypeDefinitionName, opt => opt.MapFrom(src => src.SalesTypeDefinition != null ? src.SalesTypeDefinition.Name : null));

            CreateMap<Order, OrderGetDto>()
                .ForMember(dest => dest.PotentialCustomerName, opt => opt.MapFrom(src => src.PotentialCustomer != null ? src.PotentialCustomer.CustomerName : null))
                .ForMember(dest => dest.ShippingAddressText, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.Address : null))
                .ForMember(dest => dest.RepresentativeName, opt => opt.MapFrom(src => src.Representative != null ? src.Representative.FirstName + " " + src.Representative.LastName : null))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType != null ? src.PaymentType.Name : null))
                .ForMember(dest => dest.GeneralDiscountRate, opt => opt.MapFrom(src => src.GeneralDiscountRate))
                .ForMember(dest => dest.GeneralDiscountAmount, opt => opt.MapFrom(src => src.GeneralDiscountAmount))
                .ForMember(dest => dest.DocumentSerialTypeName, opt => opt.MapFrom(src => src.DocumentSerialType != null ? src.DocumentSerialType.SerialPrefix : null))
                .ForMember(dest => dest.SalesTypeDefinitionName, opt => opt.MapFrom(src => src.SalesTypeDefinition != null ? src.SalesTypeDefinition.Name : null));

            CreateMap<CreateOrderDto, Order>()
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
                .ForMember(dest => dest.GeneralDiscountRate, opt => opt.MapFrom(src => src.GeneralDiscountRate))
                .ForMember(dest => dest.GeneralDiscountAmount, opt => opt.MapFrom(src => src.GeneralDiscountAmount))
                .ForMember(dest => dest.DocumentSerialType, opt => opt.Ignore())
                .ForMember(dest => dest.Quotation, opt => opt.Ignore())
                .ForMember(dest => dest.SalesTypeDefinition, opt => opt.Ignore());

            CreateMap<UpdateOrderDto, Order>()
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
                .ForMember(dest => dest.GeneralDiscountRate, opt => opt.MapFrom(src => src.GeneralDiscountRate))
                .ForMember(dest => dest.GeneralDiscountAmount, opt => opt.MapFrom(src => src.GeneralDiscountAmount))
                .ForMember(dest => dest.DocumentSerialType, opt => opt.Ignore())
                .ForMember(dest => dest.Quotation, opt => opt.Ignore())
                .ForMember(dest => dest.SalesTypeDefinition, opt => opt.Ignore());
        }
    }
}
