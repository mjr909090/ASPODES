using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.Model;
using ASPODES.DTO.Project;
using ASPODES.DTO.AnnualTask;

namespace ASPODES.WebAPI.TypeMapping
{
    public class ProjectProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Project, GetProjectDTO>()
                .ForMember(DTO => DTO.DelegateType, config => config.MapFrom(p => p.DelegateType == DelegateType.DIRECTIONAL ? "定向" : "非定向"))
                .ForMember(DTO => DTO.ProjectTypeName, config => config.MapFrom(p => p.ProjectType.Name))
                .ForMember(DTO => DTO.HostDepartMembers, config => config.MapFrom(p => p.Members == null ? null : p.Members.Where(m => m.Person.InstituteId == p.InstituteId && m.PersonId != p.LeaderId).Select(Mapper.Map<GetProjectMemberVO>).ToList()))
                .ForMember(DTO => DTO.OtherDepartMembers, config => config.MapFrom(p => p.Members == null ? null : p.Members.Where(m => m.Person.InstituteId != p.InstituteId).Select(Mapper.Map<GetProjectMemberVO>).ToList()))
                .ForMember(DTO => DTO.Docs, config => config.MapFrom(p => p.Docs == null ? null : p.Docs.Select(Mapper.Map<GetProjectDocDTO>).ToList()))
                .ForMember(DTO => DTO.AnnualTasks, config => config.MapFrom(p => p.AnnualTasks == null ? null : p.AnnualTasks.Select(Mapper.Map<GetAnnualTaskVO>).ToList()))
                .ForMember(DTO => DTO.Leader, config => config.MapFrom(p => p.Members == null ? null : Mapper.Map < GetProjectMemberVO > (p.Members.FirstOrDefault(m => m.PersonId == p.LeaderId))));

            CreateMap<ProjectDoc, GetProjectDocDTO>();

            CreateMap<Project, GetComboProjectDTO>()
                .ForMember(DTO => DTO.LeaderName, config => config.MapFrom(p => p.Leader.Name));

            CreateMap<ProjectMember, GetProjectMemberVO>()
                .ForMember(DTO => DTO.PersonName, config => config.MapFrom(p => p.Person.Name))
                .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(p => p.Person.Institute.Name))
                .ForMember(DTO => DTO.UserName, config => config.MapFrom(p => p.Person.Email));
        }
    }
}