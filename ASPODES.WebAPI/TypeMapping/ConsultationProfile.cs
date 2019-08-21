using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.DTO.Consultation;
using ASPODES.Model;

namespace ASPODES.WebAPI.TypeMapping
{
    public class ConsultationProfile:Profile
    {
        protected virtual void Configure()
         {
             //CreateMap<AddConsultationDTO, Consultation>()
             //    .Include<AddApplicationConsultationDTO, ApplicationConsultation>()
             //    .Include<AddProjectConsultationDTO, ProjectConsultation>();

             //CreateMap<AddApplicationConsultationDTO, ApplicationConsultation>()
             //   .ForMember(ac => ac.DelegateType, config => config.MapFrom(DTO => (DelegateType)Enum.Parse(typeof(DelegateType), DTO.DelegateType)))
             //   .ForMember(ac => ac.Result, config => config.MapFrom(DTO => (ApplicationConsultationResult)Enum.Parse(typeof(ApplicationConsultationResult), DTO.Result, true)))
             //   .ForMember(ac=>ac.Application, config=>config.Ignore())
             //   .ForMember(ac=>ac.Budget, config=>config.MapFrom( DTO=>DTO.TotalBudget ))
             //   .ForMember(ac=>ac.CurrentYearBudget, config=>config.MapFrom( DTO=>DTO.CurrentBudget))
             //   .ForMember(ac=>ac.ApplicationId, config=>config.MapFrom( DTO=>DTO.ApplicationId))
             //   .ForMember(ac=>ac.ConsultationId, config=>config.Ignore())
             //   .ForMember(ac=>ac.Period, config=>config.MapFrom(DTO=>DTO.Period));
                
             //CreateMap<AddProjectConsultationDTO, ProjectConsultation>()
             //    .ForMember(pc => pc.DelegateType, config => config.MapFrom(DTO => (DelegateType)Enum.Parse(typeof(DelegateType), DTO.DelegateType)))
             //    .ForMember(pc => pc.Result, config => config.MapFrom(DTO => (ProjectConsultationResult)Enum.Parse(typeof(ProjectConsultationResult), DTO.Result, true)))
             //    .ForMember(pc => pc.Project, config => config.Ignore())
             //    .ForMember(pc => pc.Budget, config => config.MapFrom(DTO => DTO.TotalBudget))
             //    .ForMember(pc => pc.CurrentYearBudget, config => config.MapFrom(DTO => DTO.CurrentBudget));
            

            CreateMap<Application, GetApplicationConsultationDTO>();

            CreateMap<Project, GetProjectConsultationDTO>();


        }

    }
}