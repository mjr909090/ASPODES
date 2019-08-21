using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ASPODES.Model;
using ASPODES.DTO.AnnualTask;

namespace ASPODES.WebAPI.TypeMapping
{
    public class AnnualTaskProfile:Profile
    {
        protected override void Configure()
        {
            CreateMap<AnnualTaskBudgetItem, GetAnnualTaskBudgetItemDTO>()
                .ForMember( DTO=>DTO.SubjectName, config=>config.MapFrom( atbi=>atbi.Subject.SubjectName) );

            //AnnualTaskBudgetItem
            CreateMap<AnnualTaskInstBudget, GetAnnualTaskInstBudgetDTO>()
                .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(atb => atb.Institute.Name));

            CreateMap<AddAnnualTaskBudgetItemDTO, AnnualTaskBudgetItem>()
                .ForMember(ati => ati.AnnualTaskBudgetItemId, config => config.Ignore());
            CreateMap<UpdateAnnualTaskBudgetItemDTO, AnnualTaskBudgetItem>()
                .ForMember(ati => ati.AnnualTaskId, config => config.Ignore())
                .ForMember(ati => ati.SubjectId, config => config.Ignore());

            //AnnualTask
            CreateMap<AnnualTask, GetAnnualTaskDTO>()
                .ForMember( DTO=>DTO.Bsic, config=>config.MapFrom( at=>Mapper.Map<GetAnnualTaskBasicInfoDTO>(at)))
                .ForMember( DTO=>DTO.BudgetItems, config=>config.MapFrom( at=> at.AnnualTaskBudgetItems == null ? null : at.AnnualTaskBudgetItems.Select( Mapper.Map<GetAnnualTaskBudgetItemDTO>).ToList()) )
                .ForMember( DTO=>DTO.InstBudgets, config=>config.MapFrom( at=>at.AnnualTaskInstBudgets == null ? null : at.AnnualTaskInstBudgets.Select(Mapper.Map<GetAnnualTaskInstBudgetDTO>).ToList() ))
                .ForMember( DTO=>DTO.Docs, config=>config.MapFrom( at=>at.AnnualTaskDocs == null ? null: at.AnnualTaskDocs.Select(Mapper.Map<GetAnnualTaskDocDTO>).ToList()));

            CreateMap<AnnualTask, GetAnnualTaskBasicInfoDTO>()
                .ForMember(DTO => DTO.TotalBudget, config => config.MapFrom(at => at.Project.TotalBudget))
                .ForMember(DTO => DTO.ProjectName, config => config.MapFrom(at => at.Project.Name))
                .ForMember(DTO => DTO.LeaderName, config => config.MapFrom(at => at.Leader.Name))
                .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(at => at.Institute.Name))
                .ForMember(DTO=>DTO.ProjectTypeName, config=>config.MapFrom(at=>at.Project.ProjectType.Name));

            CreateMap<AnnualTask, GetAnnualTaskVO>()
                .ForMember( VO=>VO.ProjectName, config=>config.MapFrom( at=>at.Project.Name));

            //AnnualTaskDoc
            CreateMap<AnnualTaskDoc, GetAnnualTaskDocDTO>();


      
            //AnnualTaskInstBudget
            CreateMap<UpdateAnnualTaskInstBudgetDTO, AnnualTaskInstBudget>();
            CreateMap<AddAnnualTaskInstBudgetDTO, AnnualTaskInstBudget>();
           
        }
    }
}