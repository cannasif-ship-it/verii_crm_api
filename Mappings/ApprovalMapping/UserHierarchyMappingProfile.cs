using AutoMapper;
using cms_webapi.Models;
using cms_webapi.DTOs;

namespace cms_webapi.Mappings
{
    public class UserHierarchyMappingProfile : Profile
    {
        public UserHierarchyMappingProfile()
        {
            CreateMap<UserHierarchy, UserHierarchyGetDto>()
                .ForMember(dest => dest.SalespersonFullName, opt => opt.MapFrom(src => src.Salesperson != null ? $"{src.Salesperson.FirstName} {src.Salesperson.LastName}".Trim() : null))
                .ForMember(dest => dest.ManagerFullName, opt => opt.MapFrom(src => src.Manager != null ? $"{src.Manager.FirstName} {src.Manager.LastName}".Trim() : null))
                .ForMember(dest => dest.GeneralManagerFullName, opt => opt.MapFrom(src => src.GeneralManager != null ? $"{src.GeneralManager.FirstName} {src.GeneralManager.LastName}".Trim() : null))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<UserHierarchyCreateDto, UserHierarchy>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Salesperson, opt => opt.Ignore())
                .ForMember(dest => dest.Manager, opt => opt.Ignore())
                .ForMember(dest => dest.GeneralManager, opt => opt.Ignore());

            CreateMap<UserHierarchyUpdateDto, UserHierarchy>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Salesperson, opt => opt.Ignore())
                .ForMember(dest => dest.Manager, opt => opt.Ignore())
                .ForMember(dest => dest.GeneralManager, opt => opt.Ignore());
        }
    }
}
