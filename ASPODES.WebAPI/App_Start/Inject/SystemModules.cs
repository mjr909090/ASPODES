using ASPODES.WebAPI.Repository;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.App_Start.Inject
{
    /// <summary>
    /// 系统模块模块的依赖注入
    /// </summary>
    public class SystemModules : Module
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            // service层向controller层的依赖注入

            // repository层向service层的依赖注入
            builder.RegisterType<ExportExcelRepository>().AsSelf().InstancePerRequest();
        }
    }
}