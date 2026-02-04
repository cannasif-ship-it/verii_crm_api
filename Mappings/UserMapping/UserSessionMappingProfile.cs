using AutoMapper;
using crm_api.Models;
using crm_api.DTOs;

namespace crm_api.Mappings
{
    public class UserSessionMappingProfile : Profile
    {
        public UserSessionMappingProfile()
        {
            // Entity -> DTO
            CreateMap<UserSession, UserSessionDto>()
                .ForMember(
                    dest => dest.CreatedByFullUser,
                    opt => opt.MapFrom(src => FullName(src.CreatedByUser))
                )
                .ForMember(
                    dest => dest.UpdatedByFullUser,
                    opt => opt.MapFrom(src => FullName(src.UpdatedByUser))
                )
                .ForMember(
                    dest => dest.DeletedByFullUser,
                    opt => opt.MapFrom(src => FullName(src.DeletedByUser))
                );

            // Create DTO -> Entity
            CreateMap<CreateUserSessionDto, UserSession>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))

                // Sistem alanları kontrol altında
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());
        }

        private static string? FullName(User? user)
        {
            if (user == null) return null;
            return $"{user.FirstName} {user.LastName}".Trim();
        }
    }
}
