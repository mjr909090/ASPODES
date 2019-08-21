using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.DTO.System;
using ASPODES.Model;
using AutoMapper;

namespace ASPODES.WebAPI.TypeMapping
{
    public class SystemProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TemplateReason, TemplateReasonDTO>()
                .ForMember(DTO => DTO.ReasonContent, config => config.MapFrom(a => a.Content));

            //Announcement
            CreateMap<Announcement, GetAnnouncementDTO>()
                .ForMember(DTO => DTO.PublisherName, config => config.MapFrom(a => a.Publisher.Name))
                .ForMember(DTO => DTO.Attachments, config => config.MapFrom( a=>a.AnnouncementAttachments == null ? null : a.AnnouncementAttachments.Select( Mapper.Map<GetAnnouncementAttachmentDTO>).ToList()));
            
            CreateMap<Announcement, GetAnnouncementComboDTO>()
                .ForMember(DTO => DTO.PublisherName, config => config.MapFrom(a => a.Publisher.Name));

            CreateMap<AddAnnouncementDTO, Announcement>()
                .ForMember(a => a.PublishDate, config => config.Ignore())
                .ForMember(a => a.Status, config => config.Ignore())
                .ForMember(a => a.PublisherId, config => config.Ignore());

            CreateMap<AnnouncementAttachment, GetAnnouncementAttachmentDTO>();
            CreateMap<AddAnnouncementAttachmentDTO, AnnouncementAttachment>();

            //TemplateReason
            CreateMap<TemplateReason, GetTemplateReasonDTO>();

        }

    }
}
