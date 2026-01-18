using AutoMapper;
using cms_webapi.Models;
using cms_webapi.DTOs;

namespace cms_webapi.Mappings
{
    public class ApprovalUserRoleMappingProfile : Profile
    {
        public ApprovalUserRoleMappingProfile()
        {
            // ApprovalUserRole mappings
            CreateMap<ApprovalUserRole, ApprovalUserRoleGetDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : null))
                .ForMember(dest => dest.ApprovalRoleName, opt => opt.MapFrom(src => src.ApprovalRole != null ? src.ApprovalRole.Name : null))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<ApprovalUserRoleCreateDto, ApprovalUserRole>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovalRole, opt => opt.Ignore());

            CreateMap<ApprovalUserRoleUpdateDto, ApprovalUserRole>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovalRole, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
