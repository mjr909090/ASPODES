using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Service;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.App_Start.Inject
{
    /// <summary>
    /// 统计模块的依赖注入
    /// </summary>
    public class StatisticModules : Module
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            // service层向controller层的依赖注入
            builder.RegisterType<InstStatisticService>().AsSelf().InstancePerRequest();
            builder.RegisterType<DeptStatisticService>().AsSelf().InstancePerRequest();

            // repository层向service层的依赖注入
            builder.RegisterType<ApplicationStatisticRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<ProjectStatisticRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<FundStatisticRepository>().AsSelf().InstancePerRequest();
        }
    }
}