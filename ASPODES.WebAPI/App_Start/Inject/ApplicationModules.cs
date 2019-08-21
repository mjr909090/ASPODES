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
    /// 关于申请书的依赖注入
    /// </summary>
    public class ApplicationModules : Module
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            // service层向controller层的依赖注入
            builder.RegisterType<ApplicationService>().AsSelf().InstancePerRequest();

            // repository层向service层的依赖注入
            builder.RegisterType<ApplicationRepository>().AsSelf().InstancePerRequest();
        }
    }
}