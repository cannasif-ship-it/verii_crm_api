using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;

namespace crm_api.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Customer mappings
            CreateMap<Customer, CustomerGetDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone1))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.Name : null))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null))
                .ForMember(dest => dest.CustomerTypeName, opt => opt.MapFrom(src => src.CustomerType != null ? src.CustomerType.Name : null))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<CustomerCreateDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone1, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CompletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.District, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore())
                .ForMember(dest => dest.DefaultShippingAddress, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.IsPendingApproval, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovalStatus, opt => opt.Ignore())
                .ForMember(dest => dest.IsERPIntegrated, opt => opt.Ignore());

            CreateMap<CustomerUpdateDto, Customer>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone1, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.District, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore())
                .ForMember(dest => dest.DefaultShippingAddress, opt => opt.Ignore())
                .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src => src.CompletedDate))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.IsPendingApproval, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovalStatus, opt => opt.Ignore())
                .ForMember(dest => dest.IsERPIntegrated, opt => opt.Ignore());
        }
    }
}
