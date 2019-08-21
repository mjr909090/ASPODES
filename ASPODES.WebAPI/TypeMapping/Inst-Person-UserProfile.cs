using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using ASPODES.DTO.Inst_Person_User;
using ASPODES.Model;
using ASPODES.Common.Util;

namespace ASPODES.WebAPI.TypeMapping
{
    public class Inst_Person_UserProfile : Profile
    {
        protected override void Configure()
        {
            //Institute
            CreateMap<Institute, GetInstituteDTO>()
                .ForMember( DTO=>DTO.ContactEmail, config=>config.MapFrom( i=>i.Contact == null ? null:i.Contact.UserId ) )
                .ForMember(DTO => DTO.ContactName, config => config.MapFrom(i => i.Contact == null ? null : i.Contact.Name))
                .ForMember(DTO => DTO.ContactPhone, config => config.MapFrom(i => i.Contact == null ? null : i.Contact.Person.Phone));

            CreateMap<AddInstituteDTO, Institute>()
                .ForMember( i=>i.Type, config=>config.MapFrom( DTO=>DTO.Type == "院机关" ? InstituteType.HEADQUATER : InstituteType.INSTITUTE));
            CreateMap<Institute, GetComboInstDTO>();
            CreateMap<AddInstDTO, Institute>()
                .ForMember(i => i.Type, config => config.MapFrom(uid => Enum.Parse(typeof(InstituteType), uid.Type)));
            
            //Person
            CreateMap<Person, GetPersonDTO>()
                .ForMember( DTO=>DTO.InstituteName, config=>config.MapFrom( p=>p.Institute.Name ));
            CreateMap<User, GetPersonDTO>()
                .ForMember(DTO => DTO.Name, config => config.MapFrom(p => p.Name))
                .ForMember(DTO => DTO.Phone, config => config.MapFrom(p => p.Person.Phone))
                .ForMember(DTO => DTO.IDCard, config => config.MapFrom(p => p.Person.IDCard))
                .ForMember(DTO => DTO.Male, config => config.MapFrom(p => p.Person.Male))
                .ForMember(DTO => DTO.Email, config => config.MapFrom(p => p.Person.Email))
                .ForMember(DTO => DTO.Name, config => config.MapFrom(p => p.Name))

                .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(p => p.Institute.Name));
            CreateMap<AddPersonDTO, Person>();
            CreateMap<UpdatePersonDTO, Person>();

            CreateMap<Person, GetComboPersonDTO>();
            CreateMap<Person, User>()
                .ForMember(u => u.UserId, config => config.MapFrom(p => p.Email))
                .ForMember(u => u.Password, config => config.MapFrom(p => HashHelper.IntoMd5("123456")))
                .ForMember(u=>u.LastLogin,config=>config.Ignore())
                .ForMember(u => u.LastLogin, config => config.Ignore())
                .ForMember(u => u.Login, config => config.Ignore())
                .ForMember(u => u.ReviewAmount, config => config.Ignore())
                .ForMember(u => u.SessionId, config => config.Ignore());

            CreateMap<AddInstDTO, Person>()
                .ForMember(p => p.Name, config => config.MapFrom(uid => uid.FirstName + uid.LastName));

            //ExpertField
            CreateMap<AddExpertFieldDTO, ExpertField>();
            
            CreateMap<ExpertField, GetExpertFieldDTO>()
                .ForMember( DTO=>DTO.UserName, config=>config.MapFrom( ef=>ef.User.Name ) )
                .ForMember( DTO=>DTO.FieldId, config=>config.MapFrom(ef=>ef.SubField.ParentName ));

            CreateMap<User, GetExpertInfoDTO>()
                .ForMember( DTO=>DTO.InstituteName, config=>config.MapFrom(p=>p.Institute.Name))
                .ForMember(DTO=>DTO.FirstExpertFieldId, config=>config.Ignore())
                .ForMember(DTO => DTO.FirstSubFieldId, config => config.Ignore())
                .ForMember(DTO => DTO.SecondExpertFieldId, config => config.Ignore())
                .ForMember(DTO => DTO.SecondSubFieldId, config => config.Ignore());
        }
    }
}