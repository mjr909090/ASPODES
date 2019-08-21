using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.DTO.System;
using ASPODES.Model;
using AutoMapper;

namespace ASPODES.WebAPI.TypeMapping
{
    public class NoticeProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Notice, NoticeDTO>()
                .ForMember(DTO => DTO.NoticeContent, config => config.MapFrom(n => n.Content == null ? n.NoticeTemplate.Content:n.Content))
                .ForMember(DTO => DTO.Type, config => config.MapFrom(n => n.NoticeTemplate.NoticeType.ToString()))
                .ForMember(DTO => DTO.ReciverType, config => config.MapFrom(n => n.NoticeTemplate.ReceiverType))
                .ForMember(DTO => DTO.URL, config => config.MapFrom(n => n.NoticeTemplate.URL));
            //CreateMap<NoticeDTO, Notice>();
            CreateMap<SpNotice, NoticeDTO>()
                .ForMember(d => d.NoticeContent, config => config.MapFrom(n => n.Content.Replace("Number", n.Number.ToString())))
                .ForMember(d => d.Type, config => config.MapFrom(s => s.NoticeType.ToString()))
                .ForMember(d => d.URL, config => config.MapFrom(s => s.URL))
                .ForMember(d => d.ReciverType, config => config.MapFrom(s => s.ReceiverType))
                .AfterMap((n, d) => d.SendTime = DateTime.Now);
        }
    }
}
