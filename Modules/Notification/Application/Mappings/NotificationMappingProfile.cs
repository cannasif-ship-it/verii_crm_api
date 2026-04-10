using AutoMapper;
using NotificationEntity = crm_api.Modules.Notification.Domain.Entities.Notification;
using crm_api.Modules.Notification.Application.Dtos;

namespace crm_api.Modules.Notification.Application.Mappings
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            // Entity -> DTO
            CreateMap<NotificationEntity, NotificationDto>()
                .ForMember(dest => dest.Title, opt => opt.Ignore()) // Will be set by localization service
                .ForMember(dest => dest.Message, opt => opt.Ignore()); // Will be set by localization service
        }
    }
}
