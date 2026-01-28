using AutoMapper;
using crm_api.Models.Notification;
using crm_api.DTOs.NotificationDto;

namespace crm_api.Mappings
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            // Entity -> DTO
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore()) // Will be set by localization service
                .ForMember(dest => dest.Message, opt => opt.Ignore()); // Will be set by localization service
        }
    }
}
