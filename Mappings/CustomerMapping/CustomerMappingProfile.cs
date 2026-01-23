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
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Countries != null ? src.Countries.Name : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Cities != null ? src.Cities.Name : null))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.Districts != null ? src.Districts.Name : null))
                .ForMember(dest => dest.CustomerTypeName, opt => opt.MapFrom(src => src.CustomerTypes != null ? src.CustomerTypes.Name : null))
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
                .ForMember(dest => dest.Countries, opt => opt.Ignore())
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Districts, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerTypes, opt => opt.Ignore())
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
                .ForMember(dest => dest.Countries, opt => opt.Ignore())
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Districts, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerTypes, opt => opt.Ignore())
                .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src => src.CompletedDate))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.IsPendingApproval, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovalStatus, opt => opt.Ignore())
                .ForMember(dest => dest.IsERPIntegrated, opt => opt.Ignore());
        }
    }
}
