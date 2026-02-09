using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;
using System.Linq;

namespace crm_api.Mappings
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<Contact, ContactDto>()
                .ForMember(dest => dest.TitleName, opt => opt.MapFrom(src => src.Title != null ? src.Title.TitleName : null))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : null))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.FullName)
                        ? string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim()
                        : src.FullName))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<CreateContactDto, Contact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.FullName)
                        ? string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim()
                        : src.FullName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());

            CreateMap<UpdateContactDto, Contact>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.FullName)
                        ? string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim()
                        : src.FullName))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
        }
    }
}
