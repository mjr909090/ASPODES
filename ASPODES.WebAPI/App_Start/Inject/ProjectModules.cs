using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Controllers.Project;

namespace ASPODES.WebAPI.App_Start.Inject
{
    /// <summary>
    /// 项目模块的依赖注入
    /// </summary>
    public class ProjectModules:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>();
            builder.RegisterType<ProjectDocRepository>().As<IProjectDocRepository>();
            builder.RegisterType<ProjectMemberRepository>().As<IProjectMemberRepository>();
        }
    }
}