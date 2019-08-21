using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.DTO.Profile;
using ASPODES.Model;
using AutoMapper;

namespace ASPODES.WebAPI
{
    public class UserProfileProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Person, GetProfileDTO>();
        }
    }
}
