using AutoMapper;
using cms_webapi.Models;
using cms_webapi.DTOs;

namespace cms_webapi.Mappings
{
    public class ApprovalQueueMappingProfile : Profile
    {
        public ApprovalQueueMappingProfile()
        {
            CreateMap<ApprovalQueue, ApprovalQueueGetDto>()
                .ForMember(dest => dest.QuotationOfferNo, opt => opt.MapFrom(src => src.Quotation != null ? src.Quotation.OfferNo : null))
                .ForMember(dest => dest.QuotationLineProductCode, opt => opt.MapFrom(src => src.QuotationLine != null ? src.QuotationLine.ProductCode : null))
                .ForMember(dest => dest.AssignedToUserFullName, opt => opt.MapFrom(src => src.AssignedToUser != null ? $"{src.AssignedToUser.FirstName} {src.AssignedToUser.LastName}".Trim() : null))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<ApprovalQueueCreateDto, ApprovalQueue>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Quotation, opt => opt.Ignore())
                .ForMember(dest => dest.QuotationLine, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUser, opt => opt.Ignore());

            CreateMap<ApprovalQueueUpdateDto, ApprovalQueue>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Quotation, opt => opt.Ignore())
                .ForMember(dest => dest.QuotationLine, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUser, opt => opt.Ignore());
        }
    }
}
