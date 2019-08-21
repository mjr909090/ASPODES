using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ASPODES.Model;
using ASPODES.DTO.Application;

namespace ASPODES.WebAPI
{
    public class ApplicationProfile:Profile
    {
        protected override void Configure()
        {
            CreateMap<AddApplicationDTO, Application>();
            CreateMap<Application, GetApplicationDTO>()
                .ForMember( DTO=>DTO.ProjectTypeName, config=>config.MapFrom( a=>a.ProjectType.Name))
                .ForMember( DTO=>DTO.LeaderName, config=>config.MapFrom( a=>a.Leader.Name ))
                .ForMember( DTO=>DTO.LeaderEmail, config=>config.MapFrom( a=>a.Leader.Email) )
                .ForMember( DTO=>DTO.LeaderPhone, config=>config.MapFrom( a=>a.LeaderPhone))
                .ForMember( DTO=>DTO.ContactEmail, config=>config.MapFrom(a=>a.Institute.ContactId))
                .ForMember( DTO=>DTO.InstituteName, config=>config.MapFrom( a=>a.Institute.Name ))
                .ForMember( DTO=>DTO.SupportCategoryName, config=>config.MapFrom(a=>a.SupportCategory.Name))
                .ForMember(DTO => DTO.DeleageType, config => config.MapFrom(a => a.DeleageType));
            CreateMap<Application, Project>()
                .ForMember(p => p.ProjectId, config => config.Ignore())
                .ForMember(p => p.StartDate, config => config.Ignore())
                .ForMember(p => p.EndDate, config => config.Ignore())
                .ForMember(p=>p.Name, config=>config.MapFrom( a=>a.ProjectName))
                .ForMember(p=>p.Status, config=>config.MapFrom( a=> ProjectStatus.ACTIVE ));
                


            CreateMap<AddMemberDTO, Member>();
            CreateMap<Member, GetMemberDTO>()
                .ForMember(DTO => DTO.Duty, config => config.MapFrom(m => m.Person.Duty))
                .ForMember(DTO => DTO.Name, config => config.MapFrom(m => m.Person.Name))
                .ForMember(DTO => DTO.IDCard, config => config.MapFrom(m => m.Person.IDCard))
                .ForMember(DTO => DTO.Male, config => config.MapFrom(m => m.Person.Male))
                .ForMember(DTO => DTO.InstName, config => config.MapFrom(m => m.Person.Institute.Name))
                .ForMember(DTO => DTO.Phone, config => config.MapFrom(m => m.Person.Phone))
                .ForMember(DTO => DTO.Major, config => config.MapFrom(m => m.Person.Major));

            CreateMap<AddInstBudgetDTO, InstBudget>();
            CreateMap<InstBudget, GetInstBudgetDTO>()
                .ForMember(DTO => DTO.InstName, config => config.MapFrom(i => i.Institute.Name))
                .ForMember(DTO=>DTO.AnnualBudgets, config=>config.MapFrom( i=>i.InstAnnualBudgets.Select(Mapper.Map<GetInstAnnualBudgetDTO>)));

            CreateMap<AddInstAnnualBudgetDTO, InstAnnualBudget>();
            CreateMap<InstAnnualBudget, GetInstAnnualBudgetDTO>();

            CreateMap<AddInstTotalWithAnnualBudget, InstAnnualBudget>();

            //ApplicationField
            CreateMap<AddApplicationFieldDTO, ApplicationField>();
            CreateMap<ApplicationField, GetApplicationFieldDTO>()
                .ForMember(DTO => DTO.ParentFieldName, config => config.MapFrom(af => af.SubField.ParentName));
            CreateMap<ApplicationField, ApplicationFieldVO>()
                .ForMember(VO=>VO.FieldId, config => config.MapFrom(af => af.SubField.ParentName))
                .ForMember(VO=>VO.SubFields, config=>config.Ignore());


            CreateMap<AddAnnualBudgetDTO, AnnualBudget>();
            CreateMap<AnnualBudget, GetAnnualBudgetDTO>();

            CreateMap<AddAnnualBudgetItemDTO, AnnualBudgetItem>();
            CreateMap<AnnualBudgetItem, GetAnnualBudgetItemDTO>()
                .ForMember( DTO=>DTO.SubjectName, config=>config.MapFrom( m=>m.Subject.SubjectName ) );

            CreateMap<AddApplicationLogDTO, ApplicationLog>();

            CreateMap<ApplicationLog, GetApplicationLogDTO>()
                .ForMember(DTO => DTO.UserName, config => config.MapFrom(al => al.User.Name));

            CreateMap<ApplicationDoc, GetApplicationDocDTO>();


            //ApplicationStepOneLeft
            CreateMap<Application, ApplicationStepOneLeftDTO>()
                 .ForMember(asod => asod.ApplicationFields, config => config.Ignore())
                 .ForMember(asod => asod.ProjectTypeName, config => config.MapFrom( a=>a.ProjectType.Name))
                 .ForMember(asod => asod.ProjectTypes, config => config.Ignore())
                 .ForMember(asod => asod.SupportCategoryName, config => config.MapFrom(a=>a.SupportCategory.Name))
                 .ForMember(asod => asod.SupportCategorys, config => config.Ignore())
                .ForMember( asod=>asod.ApplicationFields, config=>config.Ignore())
                .ForMember( asod=>asod.Fields, config=>config.Ignore() );

            CreateMap<Application, GetApplicationInquiriesDTO>()
                .ForMember(DTO => DTO.ApplicationId, config => config.MapFrom(a => a.ApplicationId))
                .ForMember(DTO => DTO.ProjectName, config => config.MapFrom(a => a.ProjectName))
                .ForMember(DTO => DTO.Period, config => config.MapFrom(a => a.Period))
                .ForMember(DTO => DTO.ProjectTypeName, config => config.MapFrom(a => a.ProjectType.Name))
                .ForMember(DTO => DTO.SupportCategoryId, config => config.MapFrom(a => a.SupportCategoryId))
                .ForMember(DTO => DTO.SupportCategoryName, config => config.MapFrom(a => a.SupportCategory.Name))
                .ForMember(DTO => DTO.InstituteId, config => config.MapFrom(a => a.InstituteId))
                .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(a => a.Institute.Name))
                .ForMember(DTO => DTO.LeaderId, config => config.MapFrom(a => a.LeaderId))
                .ForMember(DTO => DTO.LeaderName, config => config.MapFrom(a => a.Leader.Name))
                .ForMember(DTO => DTO.LeaderEmail, config => config.MapFrom(a => a.Leader.Email))
                .ForMember(DTO => DTO.LeaderPhone, config => config.MapFrom(a => a.LeaderPhone))
                .ForMember(DTO => DTO.ContactEmail, config => config.MapFrom(a => a.Institute.ContactId))
                .ForMember(DTO => DTO.ContactPhone, config => config.MapFrom(a => a.ContactPhone))
                .ForMember(DTO => DTO.ContactEmail, config => config.MapFrom(a => a.ContactEmail))
                .ForMember(DTO => DTO.TotalBudget, config => config.MapFrom(a => a.TotalBudget))
                .ForMember(DTO => DTO.FirstYearBudget, config => config.MapFrom(a => a.FirstYearBudget))
                .ForMember(DTO => DTO.YearCreated, config => config.MapFrom(a => a.YearCreated))
                .ForMember(DTO => DTO.CurrentYear, config => config.MapFrom(a => a.CurrentYear))
                .ForMember(DTO => DTO.AbstractContent, config => config.MapFrom(a => a.AbstractContent))
                .ForMember(DTO => DTO.DeleageType, config => config.MapFrom(a => a.DeleageType))
                .ForMember(DTO => DTO.Status, config => config.MapFrom(a => a.Status));
        }
    }
}
