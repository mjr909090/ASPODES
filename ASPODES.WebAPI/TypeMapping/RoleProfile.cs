using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using ASPODES.DTO.Role;
using ASPODES.Model;

namespace ASPODES.WebAPI.TypeMapping
{
    public class RoleProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ApplicationAssignment, GetApplicationAssignmentDTO>()
                .ForMember(DTO => DTO.UserName, config => config.MapFrom(a => a.Authorize.User.Name))
                .ForMember(DTO => DTO.ProjectTypeName, config => config.MapFrom(a => a.ProjectType.Name));
            CreateMap<AddApplicationAssignmentDTO,ApplicationAssignment>(); 

        }
    }
}