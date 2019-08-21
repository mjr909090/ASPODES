using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace ASPODES.WebAPI.TypeMapping
{
    public class AutoMapperConfiguration
    {
        public static void Configuration()
        {
            Mapper.Initialize(cfg => { 
                cfg.AddProfile<ApplicationProfile>();
                cfg.AddProfile<Inst_Person_UserProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<UserProfileProfile>();
                cfg.AddProfile<NoticeProfile>();
                cfg.AddProfile<ReviewProfile>();
                cfg.AddProfile<SystemProfile>();
                cfg.AddProfile<RoleProfile>();
                cfg.AddProfile<ProjectProfile>();
                cfg.AddProfile<AnnualTaskProfile>();
                cfg.AddProfile<StatisticProfile>();
            });
        }
    }
}