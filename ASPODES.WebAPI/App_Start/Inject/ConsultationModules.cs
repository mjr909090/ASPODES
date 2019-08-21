using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using ASPODES.WebAPI.Repository;

namespace ASPODES.WebAPI.App_Start.Inject
{
    /// <summary>
    /// 项目滚动库模块依赖注入
    /// </summary>
    public class ConsultationModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsultationRepository>().As<IConsultationRepository>();
        }
    }
}